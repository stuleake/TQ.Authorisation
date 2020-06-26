using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TQ.Authentication.Data;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.Core.Dto.Groups;
using TQ.Authentication.Core.Dto.Permissions;
using TQ.Authentication.Core.Dto.Roles;
using TQ.Authentication.Core.Requests.Groups;
using TQ.Authentication.Core.Requests.Paging;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.Data.Repositories;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.ExternalServices.GraphAPI.Models;
using TQ.Authentication.Services.Extensions;
using TQ.Authentication.Services.Interfaces;
using TQ.Authentication.Services.Interfaces.Converters;

[assembly: InternalsVisibleTo("TQ.Authentication.UnitTests")]

namespace TQ.Authentication.Services
{
    /// <summary>
    /// Represents a Group Manager service that allows to manage Groups.
    /// </summary>
    /// <seealso cref="IGroupManager" />
    public class GroupManager : IGroupManager
    {
        // Dependencies
        private readonly IGraphApiClient graphApiClient;

        private readonly IGroupsRepository<Group> groupsRepository;
        private readonly IUserGroupsRepository<UserGroup> userGroupsRepository;
        private readonly IRolesRepository<Role> rolesRepository;
        private readonly IRoleConverter roleConverter;
        private readonly IAzureGroupManager azureGroupManager;
        private readonly IPermissionManager permissionManager;
        private readonly IPermissionConverter permissionConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupManager"/> class.
        /// </summary>
        /// <param name="graphApiClient">The graph API client.</param>
        /// <param name="groupsRepository">The groups repository.</param>
        /// <param name="userGroupsRepository">The user groups repository.</param>
        /// <param name="rolesRepository">The roles repository.</param>
        /// <param name="roleConverter">The role converter.</param>
        /// <param name="azureGroupManager">The azure group manager.</param>
        /// <param name="permissionManager">the permission manager</param>
        /// <param name="permissionConverter">the permission converter</param>
        public GroupManager(
            IGraphApiClient graphApiClient,
            IGroupsRepository<Group> groupsRepository,
            IUserGroupsRepository<UserGroup> userGroupsRepository,
            IRolesRepository<Role> rolesRepository,
            IRoleConverter roleConverter,
            IAzureGroupManager azureGroupManager,
            IPermissionManager permissionManager,
            IPermissionConverter permissionConverter)
        {
            this.graphApiClient = graphApiClient;
            this.groupsRepository = groupsRepository;
            this.userGroupsRepository = userGroupsRepository;
            this.rolesRepository = rolesRepository;
            this.roleConverter = roleConverter;
            this.azureGroupManager = azureGroupManager;
            this.permissionManager = permissionManager;
            this.permissionConverter = permissionConverter;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<GetGroupGetDto>> GetGroupsAsync()
        {
            // Get groups from Azure
            var azureGroups = await graphApiClient.Groups.GetGroupsAsync();

            // Get groups from SQL
            var sqlGroups = await groupsRepository.GetAllAsync();

            // Build group result
            var result = GetGroups(azureGroups, sqlGroups);

            // Return
            return result;
        }

        /// <inheritdoc />
        public async Task<GetGroupGetDto> GetGroupAsync(Guid groupId)
        {
            // Get group from Azure
            var azureGroup = await this.azureGroupManager.GetByIdAsync(groupId);

            // Check if we have the group in database
            var group = await groupsRepository.GetAsync(groupId);
            if (group == null)
            {
                throw new TQAuthenticationException(HttpStatusCode.NotFound, $"Could not find the {nameof(Group)} with {nameof(groupId)}: {groupId} in the database.");
            }

            // Return the list of roles assigned to the group
            var roles = this.rolesRepository.GetRolesForGroupAsync(groupId);

            // Build result
            var result = new GetGroupGetDto
            {
                Id = azureGroup.Id.Value,
                Name = azureGroup.Name,
                Description = azureGroup.Description,
                ServiceUrl = group.ServiceUrl,
                IsActive = group.IsActive,
                CreatedAt = azureGroup.CreatedAt.Value,
                Roles = this.roleConverter.ToRoleGetDtos(roles.Result)
            };

            // Return
            return result;
        }

        /// <inheritdoc />
        public async Task<GetGroupGetDto> CreateGroupAsync(CreateGroupRequest request)
        {
            await EnsureGroupNameIsUniqueAsync(request);

            GetGroupGetDto result;
            Guid? azureGroupId = null;
            Guid? sqlGroupId = null;

            try
            {
                // Create group in Azure
                var azureGroup = await graphApiClient.Groups.CreateGroupAsync(new AzureGroup
                {
                    Name = request.Name,
                    Description = request.Description
                });

                azureGroupId = azureGroup.Id;

                // Create group in database
                sqlGroupId = await groupsRepository.CreateAsync(new Group
                {
                    Id = azureGroup.Id.Value,
                    ServiceUrl = request.ServiceUrl,
                    IsActive = true
                });

                // Return the new group as a result
                result = new GetGroupGetDto
                {
                    Id = azureGroup.Id.Value,
                    Name = azureGroup.Name,
                    Description = azureGroup.Description,
                    IsActive = request.IsActive,
                    CreatedAt = azureGroup.CreatedAt.Value
                };
            }
            catch (Exception ex)
            {
                string exceptionMessage = null;

                if (sqlGroupId == null && azureGroupId != null)
                {
                    exceptionMessage = $"An error has occured while creating group: {azureGroupId}. ";

                    // Clean-up Azure AD
                    await graphApiClient.Groups.DeleteGroupAsync(azureGroupId.Value);
                }

                exceptionMessage += ex.Message;

                throw new TQAuthenticationException(HttpStatusCode.InternalServerError, exceptionMessage);
            }

            // Return
            return result;
        }

        /// <inheritdoc />
        public async Task UpdateGroupAsync(Guid groupId, UpdateGroupRequest request)
        {
            var azureGroup = new AzureGroup
            {
                Id = groupId,
                Name = request.Name,
                Description = request.Description
            };

            await graphApiClient.Groups.UpdateGroupAsync(azureGroup);

            // Check if we have the group in database
            var groupExist = await groupsRepository.IsAnyMatchedAsync(group => group.Id == groupId);

            if (!groupExist)
            {
                throw new TQAuthenticationException(HttpStatusCode.NotFound, $"Could not find the {nameof(Group)} with {nameof(groupId)}: {groupId} in the database.");
            }

            await groupsRepository.UpdateGroupAsync(new Group
            {
                Id = groupId,
                ServiceUrl = request.ServiceUrl,
                IsActive = request.IsActive
            });
        }

        /// <inheritdoc />
        public async Task<UserGroupDto> AddUserAsync(Guid groupId, Guid userId)
        {
            // Ensure User and Group exist
            await EnsureGroupExist(groupId);
            await EnsureUserExist(userId);

            UserGroupDto result;

            // Try to get DTO and return straightaway
            result = await GetDtoIfAlreadyExistsInAzureAsync(groupId, userId);

            if (result != null) return result;

            // Add user in Azure B2C
            await graphApiClient.Groups.AddUserAsync(groupId, userId);

            // Create the DTO from SQL
            result = new UserGroupDto
            {
                UserGroupId = await GetUserGroupIdFromSqlAsync(groupId, userId)
            };

            // Return
            return result;
        }

        /// <inheritdoc />
        public async Task RemoveUserAsync(Guid groupId, Guid userId)
        {
            // Ensure User and Group exist
            await EnsureGroupExist(groupId);
            await EnsureUserExist(userId);

            // Check whether user already exists within the group in AD B2C
            var userAlreadyExistsInB2CGroup = await graphApiClient.Groups.UserExistsInGroupAsync(groupId, userId);

            if (userAlreadyExistsInB2CGroup)
            {
                await graphApiClient.Groups.RemoveUserAsync(groupId, userId);

                await userGroupsRepository.RemoveUserGroupAsync(groupId, userId);
            }
        }

        /// <inheritdoc />
        public async Task<Group> GetGroupByIdAsync(Guid groupId)
        {
            var group = await groupsRepository.GetAsync(groupId);

            if (group == null)
            {
                throw new TQAuthenticationException(HttpStatusCode.NotFound, $"Could not find the {nameof(Group)} with {nameof(groupId)}: {groupId} in the database.");
            }

            return group;
        }

        /// <inheritdoc />
        public async Task<PagedList<GetRoleDto>> GetRolesAsync(Guid groupId, GetPagedRequest request)
        {
            var group = await groupsRepository.GetAsync(groupId);
            if (group == null)
            {
                throw new TQAuthenticationException(HttpStatusCode.NotFound, $"{nameof(Group)} {groupId} not found.");
            }

            return this.rolesRepository.GetPagedRolesForGroupAsync(groupId, request);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<GetPermissionsDto>> GetGroupPermissionsAsync(Guid groupId)
        {
            // Get the group from the TQ.Auth Db
            var group = await this.GetGroupByIdAsync(groupId);

            // Return the list of roles assigned to the group
            var roles = this.rolesRepository.GetRolesForGroupAsync(groupId);

            // If no roles found for this group, return empty list
            if (!roles.Result.Any())
            {
                return new List<GetPermissionsDto>();
            }

            // Get roles and role permissions
            var rolesAndPermissions = await this.rolesRepository.GetByIdsIncludeRolePermissionsAsync(roles.Result.Select(x => x.Id).ToList());

            // If no roles and permissions found, return empty list
            if (!rolesAndPermissions.Any())
            {
                return new List<GetPermissionsDto>();
            }

            // Get the list of permissions assigned to these roles including nested permissions
            var permissions = await this.permissionManager.GetDistinctPermissionsForRoles(rolesAndPermissions.ToList());

            // Get a nested list of permissions
            var permissionsTree = permissions.ToPermissionTree((parent, child) => child.ParentPermissionId == parent.Id).Children;
            var nestedPermissionDtos = new List<GetPermissionsDto>(this.permissionConverter.GetNestedPermissionsDto(permissionsTree));

            return nestedPermissionDtos;
        }
        
        #region Private members

        /// <summary>
        /// Build result, GroupId must be present in SQL database and Azure AD.
        /// </summary>
        /// <param name="azureGroups">The azure groups.</param>
        /// <param name="sqlGroups">The SQL groups.</param>
        /// <returns>a <see cref="Task"/> that contains <see cref="IEnumerable{GroupGetDto}"/></returns>
        private IEnumerable<GetGroupGetDto> GetGroups(IEnumerable<AzureGroup> azureGroups, IEnumerable<Group> sqlGroups)
        {
            var result = (from ag in azureGroups
                          join sg in sqlGroups on
                            ag.Id equals sg.Id
                          select new GetGroupGetDto
                          {
                              Id = ag.Id.Value,
                              Name = ag.Name,
                              Description = ag.Description,
                              ServiceUrl = sg.ServiceUrl,
                              CreatedAt = ag.CreatedAt.Value,
                              IsActive = sg.IsActive
                          }).ToList();

            // Return
            return result;
        }

        // TODO: move this to the Graph API client instead
        /// <summary>
        /// Ensures the group name is unique.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <exception cref="Exception">Group name already exists. Group name: {group.Name}</exception>
        /// <returns>a <see cref="Task"/></returns>
        private async Task EnsureGroupNameIsUniqueAsync(CreateGroupRequest group)
        {
            var matchedGroup = await graphApiClient.Groups.GetGroupByNameAsync(group.Name);

            if (matchedGroup != null)
            {
                throw new TQAuthenticationException(
                    HttpStatusCode.BadRequest,
                    $"Group name already exists. Group name: {group.Name}");
            }
        }

        /// <summary>
        /// Ensures the group exist.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <exception cref="GraphApiClientException">Group {groupId} does not exist in Azure AD.</exception>
        /// <returns>a <see cref="Task"/></returns>
        private async Task EnsureGroupExist(Guid groupId)
        {
            if (!await graphApiClient.Groups.GroupExistsAsync(groupId))
            {
                throw new TQAuthenticationException(
                    HttpStatusCode.NotFound,
                    $"Group {groupId} does not exist in Azure AD.");
            }
        }

        /// <summary>
        /// Ensures the user exist.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <exception cref="GraphApiClientException">User {userId} does not exist in Azure AD.</exception>
        /// <returns>a <see cref="Task"/></returns>
        private async Task EnsureUserExist(Guid userId)
        {
            if (!await graphApiClient.Users.UserExistsAsync(userId))
            {
                throw new TQAuthenticationException(
                    HttpStatusCode.NotFound,
                    $"User {userId} does not exist in Azure AD.");
            }
        }

        /// <summary>
        /// Gets the dto if already exists in azure asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>a <see cref="Task{UserGroupDto}"/></returns>
        private async Task<UserGroupDto> GetDtoIfAlreadyExistsInAzureAsync(Guid groupId, Guid userId)
        {
            UserGroupDto userGroupDto = null;

            // Check whether user already exists within the group in AD B2C
            var userAlreadyExistsInB2CGroup = await graphApiClient.Groups.UserExistsInGroupAsync(groupId, userId);

            // Build result if user already exists within the group in AD B2C
            if (userAlreadyExistsInB2CGroup)
            {
                userGroupDto = new UserGroupDto
                {
                    UserGroupId = await userGroupsRepository.GetUserGroupIdAsync(groupId, userId),
                    UserAlreadyInGroup = true
                };
            }

            // Return
            return userGroupDto;
        }

        /// <summary>
        /// Gets the user group identifier from SQL asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>a <see cref="Task{Guid}"/></returns>
        private async Task<Guid> GetUserGroupIdFromSqlAsync(Guid groupId, Guid userId)
        {
            Guid? userGroupId;

            // Check whether the relationship exist in SQL by any chance
            var userGroupExistInSql = await userGroupsRepository.IsAnyMatchedAsync(
                userGroup => userGroup.GroupId == groupId && userGroup.UserId == userId);

            // Create the relationship or get the existing
            if (userGroupExistInSql)
            {
                userGroupId = await userGroupsRepository.GetUserGroupIdAsync(groupId, userId);
            }
            else
            {
                userGroupId = await CreateUserGroupAsync(groupId, userId);
            }

            // Return
            return userGroupId.Value;
        }

        /// <summary>
        /// Creates the user group asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>a <see cref="Task{Guid}"/></returns>
        private async Task<Guid> CreateUserGroupAsync(Guid groupId, Guid userId)
        {
            // Assign user to group in SQL
            var userGroupId = await userGroupsRepository.CreateAsync(new UserGroup
            {
                Id = Guid.NewGuid(),
                GroupId = groupId,
                UserId = userId
            });

            // Return
            return userGroupId;
        }

        #endregion Private members
    }
}