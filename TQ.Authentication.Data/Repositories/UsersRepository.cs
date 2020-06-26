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
    /// Represents a Users repository implementation.
    /// </summary>
    /// <seealso cref="IUsersRepository{User}" />
    public class UsersRepository : IUsersRepository<User>
    {
        // Dependencies
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public UsersRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<Guid> CreateAsync(User entity)
        {
            var user = await context.Users.AddAsync(entity);

            await context.SaveChangesAsync();

            return user.Entity.Id;
        }

        //// <inheritdoc />
        public async Task<User> GetAsync(Guid id)
        {
            return await context.Users.FindAsync(id);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<User>> GetUsersByIdsAsync(IEnumerable<Guid> userIds)
        {
            var users = await context.Users
                .Where(x => userIds.Contains(x.Id))
                .ToListAsync();

            return users;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<User>> GetUsersByRoleIdsAsync(Guid roleId, IEnumerable<Guid> userIds)
        {
            var users = await context.Users
                .Where(x => userIds.Contains(x.Id))
                .Join(
                    context.UserRoles.Where(x => x.RoleId.Equals(roleId)),
                    user => user.Id,
                    userRole => userRole.UserId,
                    (user, userRole) => user
                )
                .ToListAsync();

            return users;
        }

        /// <inheritdoc />
        public Task<bool> IsAnyMatchedAsync(Expression<Func<User, bool>> predicate)
        {
            return Task.FromResult(context.Users.Any(predicate));
        }
    }
}