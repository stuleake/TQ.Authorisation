using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TQ.Authentication.Core.Dto.Permissions;
using TQ.Authentication.Core.Dto.Roles;
using TQ.Authentication.Core.Dto.Users;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.Core.Requests.Roles;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.Data.Repositories;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.ExternalServices.GraphAPI.Models;
using TQ.Authentication.Services.Extensions;
using TQ.Authentication.Services.Interfaces;
using TQ.Authentication.Services.Interfaces.Converters;
using Group = TQ.Authentication.Data.Entities.Group;
using Permission = TQ.Authentication.Data.Entities.Permission;
using RolePermission = TQ.Authentication.Data.Entities.RolePermission;

namespace TQ.Authentication.Services
{
    /// <summary>
    /// Represents a Role Manager service that allows to manage Roles.
    /// </summary>
    /// <seealso cref="TQ.Authentication.Services.Interfaces.IRoleManager" />
    public class RoleManager : IRoleManager
    {
        // Dependencies
        private readonly IGraphApiClient graphApiClient;

        private readonly IRolesRepository<Role> rolesRepository;
        private readonly IRolePermissionsRepository<RolePermission> rolePermissionsRepository;
        private readonly IPermissionsRepository<Permission> permissionsRepository;
        private readonly IGroupsRepository<Group> groupRepository;
        private readonly IGroupManager groupManager;
        private readonly IAzureGroupManager azureGroupManager;
        private readonly IPermissionManager permissionManager;
        private readonly IRoleConverter roleConverter;
        private readonly IPermissionConverter permissionConverter;
        private readonly IUsersRepository<User> usersRepository;
        private readonly IUserGroupsRepository<UserGroup> userGroupsRepository;
        private readonly IUserRolesRepository<UserRole> userRolesRepository;

        /// <summary>
        /// Creates a new instance of the <see cref="RoleManager"/>class>
        /// </summary>
        /// <param name="rolesRepository">the roles repository</param>
        /// <param name="rolePermissionsRepository">the roles permission repository</param>
        /// <param name="permissionsRepository">the permission repository</param>
        /// <param name="groupManager">the group manager</param>
        /// <param name="azureGroupsManager">the azure groups manager</param>
        /// <param name="permissionManager">the permission manager</param>
        /// <param name="roleConverter">the role converter to use</param>
        /// <param name="permissionConverter">the permission converter to use</param>
        public RoleManager(
            IGraphApiClient graphApiClient,
            IRolesRepository<Role> rolesRepository,
            IRolePermissionsRepository<RolePermission> rolePermissionsRepository,
            IPermissionsRepository<Permission> permissionsRepository,
            IGroupsRepository<Group> groupRepository,
            IGroupManager groupManager,
            IAzureGroupManager azureGroupManager,
            IPermissionManager permissionManager,
            IRoleConverter roleConverter,
            IPermissionConverter permissionConverter,
            IUsersRepository<User> usersRepository,
            IUserGroupsRepository<UserGroup> userGroupsRepository,
            IUserRolesRepository<UserRole> userRolesRepository)

        {
            this.graphApiClient = graphApiClient;
            this.rolesRepository = rolesRepository;
            this.rolePermissionsRepository = rolePermissionsRepository;
            this.permissionsRepository = permissionsRepository;
            this.groupManager = groupManager;
            this.azureGroupManager = azureGroupManager;
            this.permissionManager = permissionManager;
            this.roleConverter = roleConverter;
            this.permissionConverter = permissionConverter;
            this.usersRepository = usersRepository;
            this.userGroupsRepository = userGroupsRepository;
            this.userRolesRepository = userRolesRepository;
            this.groupRepository = groupRepository;
        }

        /// <inheritdoc />
        public async Task<Guid> CreateRoleAsync(CreateRoleRequest request)
        {
            // Ensure the GroupId is valid
            await EnsureGroupIdExistsAsync(request.GroupId);

            // Role name should be unique per GroupId
            await EnsureRoleNameIsUniquePerGroupIdAsync(request.GroupId, request.Name);

            // Ensure permissions we are updating with belong to the same group as the role
            await EnsurePermissionsBelongToGroupIdAsync(request.GroupId, request.Permissions);

            // Ensure permissions are all unique in the list
            EnsurePermissionsInListUnique(request.Permissions);

            // Create new role
            var roleId = await rolesRepository.CreateAsync(new Role
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                GroupId = request.GroupId
            });

            // Get Role Permissions
            var rolePermissions = GetRolePermissions(roleId, request.Permissions);

            // Assign role permissions
            await rolePermissionsRepository.CreateMultipleAsync(rolePermissions);

            // Return
            return roleId;
        }

        /// <inheritdoc />
        public async Task AddUsersToRoleAsync(Guid roleId, List<Guid> model)
        {
            // Ensure the role exists before trying to add any users to it
            await EnsureRoleIdExistsAsync(roleId);

            // Ensure user exists in TQ-Auth
            await EnsureUserIdExistsAsync(model);

            // Ensure users belong to a group which the role is associated with
            await EnsureUserIdHasSameGroupAsRoleAsync(roleId, model);

            await EnsureUserNotAlreadyAssignedToRoleAsync(roleId, model);

            // Add new users to role
            foreach (var newUserId in model)
            {
                await userRolesRepository.CreateAsync(
                new UserRole
                {
                    Id = Guid.NewGuid(),
                    RoleId = roleId,
                    UserId = newUserId
                });
            }
        }

        /// <inheritdoc />
        public async Task UpdateRoleAsync(Guid roleId, UpdateRoleRequest request)
        {
            // Ensure the GroupId is valid
            await EnsureGroupIdExistsAsync(request.GroupId);

            // Ensure we're updating an existing role
            await EnsureRoleIdExistsAsync(roleId);

            // Ensure the groupId for the role is not updated
            await EnsureGroupIdIsNotChangedAsync(request.GroupId, roleId);

            // Ensure permissions are all unique in the list
            EnsurePermissionsInListUnique(request.Permissions);

            // Check whether the role name is unique per groupId
            var isRoleNameChanged = await RoleNameChangedAsync(roleId, request.Name);

            // If role name is different than it was,
            // we want to make sure it's unique per groupId
            if (isRoleNameChanged)
            {
                await EnsureRoleNameIsUniquePerGroupIdAsync(request.GroupId, request.Name);
            }

            // Ensure permissions we are updating with belong to the same group as the role
            await EnsurePermissionsBelongToGroupIdAsync(request.GroupId, request.Permissions);

            // Update role
            await rolesRepository.UpdateRoleAsync(new Role
            {
                Id = roleId,
                Name = request.Name,
                Description = request.Description
            });

            // Get Role Permissions
            var rolePermissions = GetRolePermissions(roleId, request.Permissions);

            // Update Role Permissions
            await rolePermissionsRepository.UpdateMultipleAsync(roleId, rolePermissions);
        }

        /// <inheritdoc />
        public async Task<GetRoleGroupPermissionDto> GetRoleByIdAsync(Guid roleId)
        {
            // Check that the role exists
            var role = await this.rolesRepository.GetByIdIncludeRolePermissionsAync(roleId);
            if (role == null)
            {
                throw new TQAuthenticationException(HttpStatusCode.NotFound, $"Could not find the {nameof(Role)} with {nameof(role.Id)}: {roleId}");
            }

            // Get the group associated with the role from the TQ.Auth Db
            var group = await this.groupManager.GetGroupByIdAsync(role.GroupId);

            // Get the group from Azure
            var azureGroup = await this.azureGroupManager.GetByIdAsync(group.Id);

            // Get the list of permissions assigned to the role
            List<Permission> permissions = await this.permissionManager.GetRolePermissions(role);

            // Get a nested list of permissions
            var permissionsTree = permissions.ToPermissionTree((parent, child) => child.ParentPermissionId == parent.Id).Children;
            var nestedPermissionDtos = new List<GetPermissionsDto>(this.permissionConverter.GetNestedPermissionsDto(permissionsTree));

            return this.roleConverter.ToRoleGroupPermissionGetDto(role, group, azureGroup, nestedPermissionDtos);
        }

        /// <inheritdoc />
        public async Task<GetPagedUsersDto> GetUsersByRoleAsync(Guid roleId)
        {
            // Ensure roleId exists in db
            await EnsureRoleIdExistsAsync(roleId);

            // Initialise result
            var result = new GetPagedUsersDto();

            var pagedAzureUsers = await graphApiClient.Users.GetPagedUsersAsync(new AzureUserFilter());

            // Get the list of UserIDs of Azure users
            var azureUserIds = pagedAzureUsers.Users.Select(x => x.Id.Value);

            // Get users from SQL by roleId
            var sqlUsers = await usersRepository.GetUsersByRoleIdsAsync(roleId, azureUserIds);

            // Set the SkipToken, can be null
            result.NextPageToken = pagedAzureUsers.NextPageToken;

            // Get users
            result.Users = GetUsers(pagedAzureUsers, sqlUsers);

            // Return
            return result;
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="azurePagedUsers">The azure paged users.</param>
        /// <param name="sqlUsers">The SQL users.</param>
        /// <returns>a <see cref="IEnumerable{UserDto}"/></returns>
        private IEnumerable<UserDto> GetUsers(AzurePagedUsers azurePagedUsers, IEnumerable<User> sqlUsers)
        {
            var result = (from au in azurePagedUsers.Users
                          join su in sqlUsers on
                            au.Id equals su.Id
                          select new UserDto
                          {
                              Id = au.Id,
                              DisplayName = au.DisplayName,
                              EmailAddress = au.EmailAddress,
                              GivenName = au.GivenName,
                              Surname = au.Surname,
                              AccountEnabled = au.AccountEnabled,
                              UserPrincipalName = au.UserPrincipalName,
                              BusinessPhones = au.BusinessPhones,
                              JobTitle = au.JobTitle,
                              MobilePhone = au.MobilePhone,
                              OfficeLocation = au.OfficeLocation,
                              PreferredLanguage = au.PreferredLanguage,
                              Mail = au.Mail
                          }) ?? Enumerable.Empty<UserDto>();

            // Return
            return result;
        }

        #region Private members

        /// <summary>
        /// Ensures the role name is unique per groupId.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <exception cref="ArgumentException">Role name {roleName} with GroupId {groupId} already exists.</exception>
        private async Task EnsureRoleNameIsUniquePerGroupIdAsync(Guid groupId, string roleName)
        {
            var roleIsUnique = await rolesRepository.IsRoleNameUniquePerGroupIdAsync(groupId, roleName);

            if (!roleIsUnique)
            {
                throw new TQAuthenticationException(HttpStatusCode.Conflict,
                    $"{nameof(Role)} {nameof(Role.Name)} '{roleName}' already exists in {nameof(groupId)} {groupId}");
            }
        }

        private async Task EnsureUserNotAlreadyAssignedToRoleAsync(Guid roleId, List<Guid> model)
        {
            foreach (var userGuid in model)
            {
                var existingUserWithRole = await userRolesRepository.IsUserAlreadyAssignedToRoleAsync(roleId, model);

                if (existingUserWithRole.HasValue)
                {
                    throw new TQAuthenticationException(
                    HttpStatusCode.Conflict,
                    $"RoleId {roleId} already assigned to user {existingUserWithRole}.");
                }
            }
        }

        /// <summary>
        /// Roles the name changed asynchronous.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        private async Task<bool> RoleNameChangedAsync(Guid roleId, string roleName)
        {
            var isMatched = await rolesRepository.RoleNameExistsAsync(roleId, roleName);

            return !isMatched;
        }

        /// <summary>
        /// Ensures the group identifier is not changed asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <exception cref="ArgumentException">GroupId {groupId} for the role cannot be changed.</exception>
        private async Task EnsureGroupIdIsNotChangedAsync(Guid groupId, Guid roleId)
        {
            var groupIdValid = await rolesRepository.GroupAndRoleCombinationExistsAsync(groupId, roleId);

            if (!groupIdValid)
            {
                throw new ArgumentException("GroupId for the role cannot be changed.");
            }
        }

        /// <summary>
        /// Ensures the group identifier exists.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <exception cref="TQAuthenticationException">GroupId {groupId} does not exist.</exception>
        private async Task EnsureGroupIdExistsAsync(Guid groupId)
        {
            var groupIdExists = await groupRepository.IsAnyMatchedAsync(group => group.Id == groupId);

            if (!groupIdExists)
            {
                throw new TQAuthenticationException(HttpStatusCode.NotFound, $"GroupId {groupId} does not exist.");
            }
        }

        /// <summary>
        /// Ensures the permissions with valid group identifier.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="permissions">The permissions.</param>
        /// <exception cref="ArgumentException">All permissions must belong to group: {groupId}.</exception>
        private async Task EnsurePermissionsBelongToGroupIdAsync(Guid groupId, IEnumerable<Guid> permissions)
        {
            var allPermissionsValid = await permissionsRepository.AllPermissionsBelongToGroupIdAsync(groupId, permissions);

            if (!allPermissionsValid)
            {
                throw new ArgumentException($"All permissions must belong to group: {groupId}.");
            }
        }

        /// <summary>
        /// Ensures the permissions in list unique.
        /// </summary>
        /// <param name="permissions">The permissions.</param>
        /// <exception cref="ArgumentException">Permissions in the list must be unique.</exception>
        private void EnsurePermissionsInListUnique(IEnumerable<Guid> permissions)
        {
            var permissionsCount = permissions.Count();
            var distinctPermissionsCount = permissions.Distinct().Count();

            if (permissionsCount != distinctPermissionsCount)
            {
                throw new ArgumentException("Permissions in the list must be unique.");
            }
        }

        /// <summary>
        /// Gets the role permissions.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <param name="permissions">The permissions.</param>
        /// <returns>a <see cref="IEnumerable{RolePermission}"/></returns>
        private IEnumerable<RolePermission> GetRolePermissions(Guid roleId, IEnumerable<Guid> permissions)
        {
            // Build RolePermissions
            var rolePermissions = permissions.Select(permission => new RolePermission
            {
                Id = Guid.NewGuid(),
                RoleId = roleId,
                PermissionId = permission
            });

            // Return
            return rolePermissions;
        }

        /// <summary>
        /// Ensures the role identifier exists.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <exception cref="ArgumentException">RoleId {roleId} does not exist.</exception>
        private async Task EnsureRoleIdExistsAsync(Guid roleId)
        {
            var roleIdExists = await rolesRepository
                .IsAnyMatchedAsync(role => role.Id == roleId);

            if (!roleIdExists)
            {
                throw new TQAuthenticationException(
                    HttpStatusCode.NotFound,
                    $"Could not find the {nameof(Role)} with {nameof(roleId)}: {roleId}");
            }
        }

        /// <summary>
        /// Ensures the user identifier exists.
        /// </summary>
        /// <param name="model">The list of user identifiers.</param>
        /// <exception cref="ArgumentException">UserId {userId} does not exist.</exception>
        private async Task EnsureUserIdExistsAsync(List<Guid> model)
        {
            foreach (var userId in model)
            {
                // Check if we have the user in database
                var userIdExistInSql = await usersRepository
                    .IsAnyMatchedAsync(user => user.Id == userId);

                if (!userIdExistInSql)
                {
                    throw new TQAuthenticationException(
                    HttpStatusCode.NotFound,
                    ($"Could not find the {nameof(User)} with {nameof(userId)}: {userId}"));
                }
            }
        }

        /// <summary>
        /// Ensures the user Id has same group Id associated as the role
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// /// <param name="model">The list of user identifiers.</param>
        /// <exception cref="ArgumentException">RoleId {roleId} does not exist.</exception>
        private async Task EnsureUserIdHasSameGroupAsRoleAsync(Guid roleId, List<Guid> model)
        {
            foreach (var userId in model)
            {
                // Check if the group on the user is the same as that on the role
                var groupsForUser = await userGroupsRepository.GetUserGroupIdsByUser(userId);

                var groupForRole = await rolesRepository.GetGroupIdByRoleAsync(roleId);

                if (!groupsForUser.Contains(groupForRole))
                {
                    throw new TQAuthenticationException(
                    HttpStatusCode.Conflict,
                    $"User {userId} is not part of the Group associated with the Role {roleId}.");
                }
            }
        }

        #endregion Private members
    }
}