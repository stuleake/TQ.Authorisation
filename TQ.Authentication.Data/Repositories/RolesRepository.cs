using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TQ.Authentication.Core.Dto.Roles;
using TQ.Authentication.Core.Requests.Paging;
using TQ.Authentication.Data.Contexts;
using TQ.Authentication.Data.Entities;

namespace TQ.Authentication.Data.Repositories
{
    /// <summary>
    /// Represents a Roles repository implementation.
    /// </summary>
    /// <seealso cref="IRolesRepository{Role}" />
    public class RolesRepository : IRolesRepository<Role>
    {
        // Dependencies
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RolesRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public RolesRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<Guid> CreateAsync(Role entity)
        {
            // Add role to Roles context and get the new entity
            var queryResult = await context.Roles.AddAsync(entity);

            // Save
            await context.SaveChangesAsync();

            // Return
            return queryResult.Entity.Id;
        }

        /// <inheritdoc />
        public async Task<Role> GetAsync(Guid id)
        {
            return await context.Roles.FindAsync(id);
        }

        /// <inheritdoc />
        public Task<bool> IsAnyMatchedAsync(Expression<Func<Role, bool>> predicate)
        {
            return Task.FromResult(context.Roles.Any(predicate));
        }

        /// <inheritdoc />
        public async Task<bool> RoleNameExistsAsync(Guid roleId, string roleName)
        {
            var isMatched = await IsAnyMatchedAsync(
                role => role.Name.ToLower() == roleName.ToLower() && role.Id == roleId);

            return isMatched;
        }

        /// <inheritdoc />
        public async Task<Guid> GetGroupIdByRoleAsync(Guid roleId)
        {
            ValidateRoleId(roleId);
            var roleDto = await GetAsync(roleId);
            var groupId = roleDto.GroupId;

            return groupId;
        }

        /// <inheritdoc />
        public async Task<bool> IsRoleNameUniquePerGroupIdAsync(Guid groupId, string roleName)
        {
            var matchedRolesPerGroup = await context.Roles
                .Where(role => role.Name.ToLower() == roleName.ToLower() && role.GroupId == groupId)
                .CountAsync();

            return matchedRolesPerGroup == 0;
        }

        /// <inheritdoc />
        public async Task<bool> GroupAndRoleCombinationExistsAsync(Guid groupId, Guid roleId)
        {
            var isMatched = await IsAnyMatchedAsync(
                role => role.GroupId == groupId && role.Id == roleId);

            return isMatched;
        }

        /// <inheritdoc />
        public async Task UpdateRoleAsync(Role entity)
        {
            // Get existing role
            var role = await context.Roles.FindAsync(entity.Id);

            // Update fields
            // we do not want to allow GroupId updates here
            role.Name = entity.Name;
            role.Description = entity.Description;

            // Update role
            context.Roles.Update(role);

            // Save
            await context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Role>> GetRolesForGroupAsync(Guid groupId)
        {
            ValidateId(groupId, nameof(groupId));

            return await context.Roles.Where(role => role.GroupId == groupId).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Role> GetByIdAsync(Guid roleId)
        {
            // Get the role
            return await context.Roles.FindAsync(roleId);
        }

        /// <inheritdoc />
        public async Task<Role> GetByIdIncludeRolePermissionsAync(Guid roleId)
        {
            return await context.Roles
                .Include(role => role.RolePermissions)
                .ThenInclude(rolePerm => rolePerm.Permission)
                .Where(role => role.Id == roleId)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Role>> GetByIdsIncludeRolePermissionsAsync(List<Guid> roleIds)
        {
            roleIds.ForEach(roleId => ValidateId(roleId, nameof(roleId)));

            return await context.Roles
                .Include(role => role.RolePermissions)
                .ThenInclude(rolePerm => rolePerm.Permission)
                .Where(role => roleIds.ToList().Select(roleId => roleId).Contains(role.Id)).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Role>> GetRolesByUserIdAndGroupsAsync(Guid userId, List<Group> groupsForUser)
        {
            var rolesByUserAndGroups = context.UserRoles.Where(userRole => userRole.UserId.Equals(userId))
                .Join(
                    context.RolePermissions,
                    userRoles => userRoles.RoleId,
                    rolePermission => rolePermission.RoleId,
                    (userRoles, rolePermission) => new { userRoles, rolePermission }
                )
                .Join(
                    context.Roles.Where(role => groupsForUser.ToList().Select(groupForUser => groupForUser.Id).Contains(role.GroupId)),
                    userRolesAndRolePermissions => userRolesAndRolePermissions.userRoles.RoleId,
                    role => role.Id,
                    (userRolesAndRolePermissions, role) => new Role
                    {
                        Id = role.Id,
                        Description = role.Description,
                        GroupId = role.GroupId,
                        Name = role.Name,
                        RolePermissions = role.RolePermissions,
                        UserRoles = role.UserRoles
                    }
                ) as IEnumerable<Role>;

            return await Task.FromResult(rolesByUserAndGroups);
        }

        /// <inheritdoc />
        public PagedList<GetRoleDto> GetPagedRolesForGroupAsync(Guid groupId, GetPagedRequest request)
        {
            return PagedList<GetRoleDto>.ToPagedList(this.context.Roles.Where(role => role.GroupId == groupId)
                                                .Select(role => new GetRoleDto 
                                                { 
                                                    Id = role.Id, 
                                                    Name = role.Name,
                                                    Description = 
                                                    role.Description
                                                }), 
                                                request.PageNumber, 
                                                request.PageSize);
        }

        private static void ValidateId(Guid id, string propertyName)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(propertyName);
            }
        }

        private static void ValidateRoleId(Guid roleId)
        {
            if (roleId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(roleId));
            }
        }
    }
}