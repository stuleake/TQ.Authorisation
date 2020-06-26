using System;
using System.Threading.Tasks;
using TQ.Authentication.ExternalServices.GraphAPI.Models;

namespace TQ.Authentication.ExternalServices.GraphAPI
{
    /// <summary>
    /// Defines an interface for Graph API Users Management.
    /// </summary>
    public interface IUserGraph
    {
        /// <summary>
        /// Gets the user asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>a <see cref="Task{AzureUser}"/></returns>
        Task<AzureUser> GetUserAsync(Guid userId);

        /// <summary>
        /// Gets the users asynchronous.
        /// </summary>
        /// <param name="azureUserFilter">The azure user filter.</param>
        /// <returns>a <see cref="Task{AzurePagedUsers}"/></returns>
        Task<AzurePagedUsers> GetPagedUsersAsync(AzureUserFilter azureUserFilter);

        /// <summary>
        /// Updates the user asynchronous.
        /// </summary>
        /// <param name="azureUserModel">The azure user model.</param>
        /// <returns>a <see cref="Task"/></returns>
        Task UpdateUserAsync(AzureUser azureUserModel);

        /// <summary>
        /// TODO: currently not used, will be fixed or removed in the future.
        /// </summary>
        /// <param name="newAzureUser">The new azure user.</param>
        /// <returns>a <see cref="Task{Guid}"/></returns>
        Task<Guid> CreateUserAsync(AzureUser newAzureUser);

        /// <summary>
        /// Checks whether a User exists in the AD B2C.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>a <see cref="Task{bool}"/></returns>
        Task<bool> UserExistsAsync(Guid userId);
    }
}