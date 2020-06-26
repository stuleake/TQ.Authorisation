using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TQ.Authentication.Core.Dto.Permissions;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.Data.Repositories;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.ExternalServices.GraphAPI.Models;
using TQ.Authentication.Services.Extensions;
using TQ.Authentication.Services.Interfaces;

namespace TQ.Authentication.Services
{
    /// <summary>
    /// Represents a Permission Manager service to manage permissions.
    /// </summary>
    /// <seealso cref="TQ.Authentication.Services.Interfaces.IPermissionManager" />
    public class PermissionManager : IPermissionManager
    {
        // Dependencies
        private readonly IGraphApiClient graphApiClient;

        private readonly IUsersRepository<User> usersRepository;
        private readonly IGroupsRepository<Group> groupsRepository;
        private readonly IPermissionsRepository<Permission> permissionsRepository;
        private readonly IRolesRepository<Role> rolesRepository;

        /// <summary>
        /// Creates a new instance of the <see cref="PermissionManager"/ class>
        /// </summary>
        /// <param name="permissionsRepository">the permissions repository to use</param>
        public PermissionManager(IPermissionsRepository<Permission> permissionsRepository)
        {
            this.permissionsRepository = permissionsRepository;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionManager"/> class.
        /// </summary>
        /// <param name="graphApiClient">The graph API client.</param>
        /// <param name="usersRepository">The users repository.</param
        /// <param name="groupsRepository">The groups repository.</param
        /// <param name="permissionsRepository">The permissions repository.</param>
        /// <param name="rolesRepository">the roles repository.</param>
        public PermissionManager(IGraphApiClient graphApiClient,
            IUsersRepository<User> usersRepository,
            IGroupsRepository<Group> groupsRepository,
            IPermissionsRepository<Permission> permissionsRepository,
            IRolesRepository<Role> rolesRepository)
        {
            this.graphApiClient = graphApiClient;
            this.usersRepository = usersRepository;
            this.groupsRepository = groupsRepository;
            this.permissionsRepository = permissionsRepository;
            this.rolesRepository = rolesRepository;
        }

        /// <inheritdoc />
        public async Task<List<Permission>> GetRolePermissions(Role role)
        {
            var rootLevelPermissions = new List<Permission>();
            var rolePermissions = new List<Permission>();

            if (role.RolePermissions?.Any() != true)
            {
                return rootLevelPermissions;
            }

            // Get all of the root level permissions
            var rootLevelParents = role.RolePermissions.Where(rolePermission => !rolePermission.Permission.ParentPermissionId.HasValue).ToList();
            rootLevelParents.ForEach(rp => rootLevelPermissions.Add(rp.Permission));

            // Get the nested permissions for each root level permission
            foreach (Permission permission in rootLevelPermissions)
            {
                var descendants = await GetDescendantPermissions(permission, new List<Permission>());
                rolePermissions.AddRange(descendants);
            }

            return rolePermissions;
        }

        private async Task<List<Permission>> GetDescendantPermissions(Permission permission, ICollection<Permission> returnList)
        {
            returnList.Add(permission);
            var childPermissions = await this.permissionsRepository.GetChildPermissionsAsync(permission.Id);
            foreach (Permission child in childPermissions)
            {
                if (child.Id != permission.Id)
                {
                    await GetDescendantPermissions(child, returnList);
                }
            }
            return returnList.ToList();
        }

        /// <inheritdoc />
        public async Task<List<Permission>> GetDistinctPermissionsForRoles(List<Role> roles)
        {
            var rootLevelPermissions = new List<Permission>();
            var rolePermissions = new List<Permission>();

            if (roles.Any() != true)
            {
                return rootLevelPermissions;
            }

            // Get all of the root level permissions
            var rootlevelRoles = roles.Where(role => role.RolePermissions.Any(rolePermission => !rolePermission.Permission.ParentPermissionId.HasValue)).ToList();

            foreach (Role role in rootlevelRoles)
            {
                var rootLevelParents = role.RolePermissions.Where(rolePermission => !rolePermission.Permission.ParentPermissionId.HasValue).ToList();
                rootLevelParents.ForEach(rp => rootLevelPermissions.Add(rp.Permission));
            }

            // Get the nested permissions for each root level permission
            foreach (Permission permission in rootLevelPermissions)
            {
                var descendants = await GetDescendantPermissions(permission, new List<Permission>());
                rolePermissions.AddRange(descendants);
            }

            return rolePermissions.Distinct().ToList();
        }

        /// <inheritdoc />
        public async Task<object> GetUserPermissionsAsync(Guid userId)
        {
            // Check if we have the user in the database
            var isUserInDatabase = await usersRepository.IsAnyMatchedAsync(x => x.Id == userId);

            if (!isUserInDatabase)
            {
                throw new TQAuthenticationException(
                    HttpStatusCode.NotFound,
                    $"{nameof(User)} with {nameof(userId)} : {userId} does not exist in the database");
            }

            // Check if we have the user in Azure (required else subsequent check for Azure groups will fail)
            if (!await graphApiClient.Users.UserExistsAsync(userId))
            {
                throw new TQAuthenticationException(
                    HttpStatusCode.NotFound,
                    $"{nameof(User)} with {nameof(userId)} : {userId} does not exist in Azure AD");
            }

            // Get Azure groups for this user (this also returns the DisplayName and stores in Name)
            var azureGroupsForUser = await graphApiClient.Groups.GetAzureGroupsByUserIdAsync(userId);

            // no groups found in Azure
            if (!azureGroupsForUser.Any())
            {
                return new List<GroupPermissionNamesDto>().ToBearerTokenFormat();
            }

            // Get Database groups for user
            var databaseGroupsForUser = groupsRepository.GetGroupsByUserId(userId);

            // no groups found in Database
            if (!databaseGroupsForUser.Any())
            {
                return new List<GroupPermissionNamesDto>().ToBearerTokenFormat();
            }

            // only process groups that are in both Azure and the database
            var azureGroupIds = azureGroupsForUser.Select(x => x.Id ?? default).ToList();
            var groupsForUser = databaseGroupsForUser.Where(x => azureGroupIds.Any(y => y.Equals(x.Id)));

            // no groups which are in both Azure and the database
            if (!groupsForUser.Any())
            {
                return new List<GroupPermissionNamesDto>().ToBearerTokenFormat();
            }

            // Get roles for this user and their groups
            var rolesForUser = await rolesRepository.GetRolesByUserIdAndGroupsAsync(userId, groupsForUser.ToList());

            var permissionsForUser = new List<Permission>();

            // If any roles found, get permissions for them
            if (rolesForUser.Any())
            {
                // Get roles and role permissions
                var rolesAndPermissionsForUser = await this.rolesRepository.GetByIdsIncludeRolePermissionsAsync(rolesForUser.Select(x => x.Id).ToList());

                if (!rolesAndPermissionsForUser.Any())
                {
                    return new List<GroupPermissionNamesDto>().ToBearerTokenFormat();
                }

                // Get the list of permissions assigned to these roles including nested permissions
                permissionsForUser = await GetDistinctPermissionsForRoles(rolesAndPermissionsForUser.ToList());
            }

            // create Azure DisplayName lookup for groups in Azure and Database (discard other Azure groups)
            var azureNameLookup = new Dictionary<Guid, string>();
            azureGroupsForUser.Where(azureGroup => groupsForUser.Any(groupForUser => groupForUser.Id.Equals(azureGroup.Id)))
                .ToList()
                .ForEach(group => azureNameLookup.Add(group.Id ?? Guid.Empty, group.Name));

            // Map groups to permissions
            // If user is assigned to a group with no permissions, an empty permissions array should be returned
            var groupPermissionNamesDto = from groups in groupsForUser 
                                      from perms in permissionsForUser.GroupBy(grp => grp.GroupId)
                                                                      .Where(perm => perm.Key.Equals(groups.Id))
                                                                      .DefaultIfEmpty()
                                      select new GroupPermissionNamesDto
                                      {
                                          GroupName = GetGroupName(groups.Id, azureNameLookup),
                                          PermissionNames = perms?.Select(perm => perm.Name) ?? new List<string>()
                                      };

            return groupPermissionNamesDto.ToBearerTokenFormat();
        }

        /// <inheritdoc />
        public async Task<Permission> GetByIdAsync(Guid permissionId)
        {
            return await this.permissionsRepository.GetAsync(permissionId);
        }

        /// <summary>
        /// Gets the group name from an azure name lookup
        /// </summary>
        /// <param name="groupId">The group identifier to search with.</param>
        /// <param name="azureNameLookup">The azure name lookup to search.</param>
        /// <returns>the group name</returns>
        private string GetGroupName(Guid groupId, Dictionary<Guid, string> azureNameLookup)
        {
            return (azureNameLookup.TryGetValue(groupId, out var groupName) ? groupName : string.Empty);
        }
    }
}