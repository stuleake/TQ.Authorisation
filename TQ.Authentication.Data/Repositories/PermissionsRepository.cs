using Microsoft.EntityFrameworkCore;
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
    /// Represents a Permissions Repository implementation.
    /// </summary>
    /// <seealso cref="IPermissionsRepository{Permission}" />
    public class PermissionsRepository : IPermissionsRepository<Permission>
    {
        // Dependencies
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionsRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public PermissionsRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<Guid> CreateAsync(Permission entity)
        {
            var result = await context.Permissions.AddAsync(entity);

            await context.SaveChangesAsync();

            return result.Entity.Id;
        }

        /// <inheritdoc />
        public async Task<Permission> GetAsync(Guid id)
        {
            return await context.Permissions.FindAsync(id);
        }

        /// <inheritdoc />
        public Task<bool> IsAnyMatchedAsync(Expression<Func<Permission, bool>> predicate)
        {
            return Task.FromResult(context.Permissions.Any(predicate));
        }

        /// <inheritdoc />
        public Task<bool> AllPermissionsBelongToGroupIdAsync(Guid groupId, IEnumerable<Guid> permissions)
        {
            // Initial permission count
            var permissionsCount = permissions.Count();

            var matchedPermissionsCount = context.Permissions
                .Where(permission => permissions.Contains(permission.Id) && permission.GroupId == groupId)
                .Count();

            // Return
            // Existing permissions count should match
            // with permissions count we want to assign/update with.
            return Task.FromResult(matchedPermissionsCount == permissionsCount);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Permission>> GetChildPermissionsAsync(Guid permissionId)
        {
            return await this.context.Permissions.Where(perm => perm.ParentPermissionId == permissionId).ToListAsync();
        }
    }
}