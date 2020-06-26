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
    /// Represents a Groups repository implementation.
    /// </summary>
    /// <typeparam name="Group">The type of the roup.</typeparam>
    /// <seealso cref="IRepository{Group, Guid}" />
    public class GroupsRepository : IGroupsRepository<Group>
    {
        // Dependencies
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupsRepository{Group}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public GroupsRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<Group> GetAsync(Guid id)
        {
            return await context.Groups.FindAsync(id);
        }

        /// <inheritdoc />
        public Task<IEnumerable<Group>> GetAllAsync()
        {
            var groups = context.Groups as IEnumerable<Group>;

            return Task.FromResult(groups);
        }

        /// <inheritdoc />
        public Task<bool> IsAnyMatchedAsync(Expression<Func<Group, bool>> predicate)
        {
            return Task.FromResult(context.Groups.Any(predicate));
        }

        /// <inheritdoc />
        public async Task<Guid> CreateAsync(Group entity)
        {
            // Add group to Groups context and get the new entity
            var queryResult = await context.Groups.AddAsync(entity);

            // Save
            await context.SaveChangesAsync();

            // Return
            return queryResult.Entity.Id;
        }

        /// <inheritdoc />
        public async Task UpdateGroupAsync(Group entity)
        {
            // Get existing group
            var group = await context.Groups.FindAsync(entity.Id);

            // Only update specific fields
            group.ServiceUrl = entity.ServiceUrl;
            group.IsActive = entity.IsActive;

            // Update group
            context.Groups.Update(group);

            // Save
            await context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public IEnumerable<Group> GetGroupsByUserId(Guid userId)
        {
            return context.UserGroups.Where(userGroup => userGroup.UserId.Equals(userId))
                .Join(
                    context.Groups,
                    userGroups => userGroups.GroupId,
                    groups => groups.Id,
                    (userGroups, groups) => new Group
                    {
                        Id = groups.Id,
                        ServiceUrl = groups.ServiceUrl
                    }) as IEnumerable<Group>;
        }
    }
}