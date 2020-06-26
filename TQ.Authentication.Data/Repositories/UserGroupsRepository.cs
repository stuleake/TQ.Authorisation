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
    /// Represents a User Groups Repository implementation.
    /// </summary>
    public class UserGroupsRepository : IUserGroupsRepository<UserGroup>
    {
        // Dependencies
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserGroupsRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public UserGroupsRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<Guid> CreateAsync(UserGroup entity)
        {
            var result = await context.UserGroups.AddAsync(entity);

            await context.SaveChangesAsync();

            return result.Entity.Id;
        }

        /// <inheritdoc />
        public async Task<UserGroup> GetAsync(Guid id)
        {
            return await context.UserGroups.FindAsync(id);
        }

        /// <inheritdoc />
        public Task<bool> IsAnyMatchedAsync(Expression<Func<UserGroup, bool>> predicate)
        {
            return Task.FromResult(context.UserGroups.Any(predicate));
        }

        /// <inheritdoc />
        public async Task RemoveUserGroupAsync(Guid groupId, Guid userId)
        {
            // Find a single combination
            var userGroup = context.UserGroups
                .FirstOrDefault(userGroup => userGroup.GroupId == groupId && userGroup.UserId == userId);

            if (userGroup != null)
            {
                // Remove
                context.UserGroups.Remove(userGroup);

                // Save
                await context.SaveChangesAsync();
            }
        }

        /// <inheritdoc />
        public async Task<Guid?> GetUserGroupIdAsync(Guid groupId, Guid userId)
        {
            var userGroup = await context.UserGroups
                .FirstOrDefaultAsync(userGroup => userGroup.GroupId == groupId && userGroup.UserId == userId);

            // Return
            return userGroup?.Id;
        }

        /// <inheritdoc />
        public async Task<List<Guid?>> GetUserGroupIdsByUser(Guid userId)
        {
            var userGroups = await context.UserGroups
                .Where(userGroup => userGroup.UserId == userId).ToListAsync();

            var groupIds = new List<Guid?>();

            foreach (var userGroup in userGroups)
            {
                var groupId = userGroup.GroupId;
                groupIds.Add(groupId);
            }
            // Return
            return groupIds;
        }
    }
}