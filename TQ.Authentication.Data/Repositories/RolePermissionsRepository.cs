using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using TQ.Authentication.Data.Entities;

namespace TQ.Authentication.Data.Repositories
{
    /// <summary>
    /// Represents a Role Permissions Repository implementation.
    /// </summary>
    /// <seealso cref="Repositories.IRolePermissionsRepository" />
    public class RolePermissionsRepository : IRolePermissionsRepository<RolePermission>
    {
        // Dependencies
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RolePermissionsRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public RolePermissionsRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<Guid> CreateAsync(RolePermission entity)
        {
            var result = await context.RolePermissions.AddAsync(entity);

            await context.SaveChangesAsync();

            return result.Entity.Id;
        }

        /// <inheritdoc />
        public async Task<RolePermission> GetAsync(Guid id)
        {
            return await context.RolePermissions.FindAsync(id);
        }

        /// <inheritdoc />
        public Task<bool> IsAnyMatchedAsync(Expression<Func<RolePermission, bool>> predicate)
        {
            return Task.FromResult(context.RolePermissions.Any(predicate));
        }

        /// <inheritdoc />
        public async Task CreateMultipleAsync(IEnumerable<RolePermission> rolePermissions)
        {
            if (!rolePermissions.Any()) return;

            await context.RolePermissions.AddRangeAsync(rolePermissions);

            await context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task RemoveMultipleAsync(IEnumerable<RolePermission> rolePermissions)
        {
            if (!rolePermissions.Any()) return;

            context.RolePermissions.RemoveRange(rolePermissions);

            await context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task UpdateMultipleAsync(Guid roleId, IEnumerable<RolePermission> rolePermissions)
        {
            // Find existing role permissions
            var existingRolePermissions = context.RolePermissions
                .Where(rolePermission => rolePermission.RoleId == roleId);

            if (existingRolePermissions.Any())
            {
                // Remove existing role permissions
                context.RolePermissions.RemoveRange(existingRolePermissions);
            }

            // Add a new list of role permissions
            if (rolePermissions.Any())
            {
                await context.RolePermissions.AddRangeAsync(rolePermissions);
            }

            // Save
            await context.SaveChangesAsync();
        }
    }
}