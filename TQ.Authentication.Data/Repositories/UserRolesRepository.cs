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
    /// Represents a User Roles Repository implementation.
    /// </summary>
    /// <seealso cref="IUserRolesRepository{UserRole}" />
    public class UserRolesRepository : IUserRolesRepository<UserRole>
    {
        // Dependencies
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserGroupsRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public UserRolesRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<Guid> CreateAsync(UserRole entity)
        {
            var userRole = await context.UserRoles.AddAsync(entity);
            await context.SaveChangesAsync();

            return userRole.Entity.Id;
        }

        /// <inheritdoc />
        public Task<bool> IsAnyMatchedAsync(Expression<Func<UserRole, bool>> predicate)
        {
            return Task.FromResult(context.UserRoles.Any(predicate));
        }

        /// <inheritdoc />
        public async Task<UserRole> GetAsync(Guid id)
        {
            return await context.UserRoles.FindAsync(id);
        }

        /// <inheritdoc />
        public async Task<Guid?> IsUserAlreadyAssignedToRoleAsync(Guid roleId, List<Guid> model)
        {
            foreach (var userGuid in model)
            {
                var exists = await IsAnyMatchedAsync(
                userRole => userRole.UserId == userGuid && userRole.RoleId == roleId);
                if (exists)
                {
                    return userGuid;
                }
            }
            return null;
        }
    }
}