using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TQ.Authentication.Data.Repositories
{
    /// <summary>
    /// Defines a User Groups Repository interface.
    /// </summary>
    /// <typeparam name="UserGroup"></typeparam>
    public interface IUserGroupsRepository<UserGroup> : IRepository<UserGroup, Guid>
    {
        /// <summary>
        /// Removes the user group asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>a <see cref="Task"/></returns>
        Task RemoveUserGroupAsync(Guid groupId, Guid userId);

        /// <summary>
        /// Gets the user group identifier asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>a <see cref="Task{Guid?}"/></returns>
        Task<Guid?> GetUserGroupIdAsync(Guid groupId, Guid userId);

        /// <summary>
        /// Gets the user group identifier using only user Id asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>a <see cref="Task{Guid?}"/></returns>
        Task<List<Guid?>> GetUserGroupIdsByUser(Guid userId);
    }
}