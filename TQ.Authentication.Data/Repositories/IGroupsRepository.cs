using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TQ.Authentication.Data.Repositories
{
    /// <summary>
    /// Defines a Groups Repository interface.
    /// </summary>
    /// <typeparam name="Group">The type of the group.</typeparam>
    /// <seealso cref="IRepository{Group, Guid}" />
    public interface IGroupsRepository<Group> : IRepository<Group, Guid>
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>a <see cref="Task"/> that contains <see cref="IEnumerable{Group}"/></returns>
        Task<IEnumerable<Group>> GetAllAsync();

        /// <summary>
        /// Updates the group asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>a <see cref="Task"/></returns>
        Task UpdateGroupAsync(Group entity);

        /// <summary>
        /// Gets all the groups for a given user.
        /// </summary>
        /// <param name="userId">The userId.</param>
        /// <returns>a <see cref="IEnumerable{Group}"/></returns>
        IEnumerable<Group> GetGroupsByUserId(Guid userId);
    }
}