using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TQ.Authentication.Data.Repositories
{
    /// <summary>
    /// Defines a Users Repository interface.
    /// </summary>
    /// <seealso cref="IRepository{Entities.User, Guid}" />
    public interface IUsersRepository<User> : IRepository<User, Guid>
    {
        /// <summary>
        /// Gets the users by ids asynchronous.
        /// </summary>
        /// <param name="userIds">The user ids.</param>
        /// <returns>a <see cref="Task"/> that contains <see cref="IEnumerable{User}"/></returns>
        Task<IEnumerable<User>> GetUsersByIdsAsync(IEnumerable<Guid> userIds);

        /// <summary>
        /// Gets the users by role asynchronous.
        /// </summary>
        /// <param name="roleId">The role id to filter on.</param>
        /// <param name="userIds">The user ids.</param>
        /// <returns>a <see cref="Task"/> that contains <see cref="IEnumerable{User}"/></returns>
        Task<IEnumerable<User>> GetUsersByRoleIdsAsync(Guid roleId, IEnumerable<Guid> userIds);
    }
}