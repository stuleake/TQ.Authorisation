using System;
using System.Threading.Tasks;
using TQ.Authentication.Core.Dto.Users;
using TQ.Authentication.Core.Requests.Users;

namespace TQ.Authentication.Services.Interfaces
{
    /// <summary>
    /// Defines a User Manager service that allows to manage Users.
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// Gets the users asynchronous.
        /// </summary>
        /// <param name="request">The request that contains the user filters to apply</param>
        /// <returns>a <see cref="Task{PagedUsersDto}"/></returns>
        Task<GetPagedUsersDto> GetUsersAsync(GetFilteredUsersRequest request);

        /// <summary>
        /// Gets the user asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>a <see cref="Task{UserDto}"/></returns>
        Task<UserDto> GetUserAsync(Guid userId);

        /// <summary>
        /// Updates the user asynchronous.
        /// </summary>
        /// <param name="request">The request that contains the details of the user to update</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>a <see cref="Task"/ that contains the result of the asynchronous action></returns>
        Task UpdateUserAsync(Guid userId, UpdateUserRequest request);
    }
}