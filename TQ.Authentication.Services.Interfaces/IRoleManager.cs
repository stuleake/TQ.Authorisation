using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.Authentication.Core.Dto.Roles;
using TQ.Authentication.Core.Dto.Users;
using TQ.Authentication.Core.Requests.Roles;
using TQ.Authentication.Core.Requests.Users;

namespace TQ.Authentication.Services.Interfaces
{
    /// <summary>
    /// Defines a Role Manager service that allows to manage Roles.
    /// </summary>
    public interface IRoleManager
    {
        /// <summary>
        /// Creates the role asynchronous.
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns></returns>
        Task<Guid> CreateRoleAsync(CreateRoleRequest request);

        /// <summary>
        /// Updates the role asynchronous.
        /// </summary>
        /// <param name="request">The request that contains the details of the role to update</param>
        /// <param name="roleId">The id of the role to update</param>
        /// <returns>a <see cref="Task"/ that contains the result of the asynchronous action></returns>
        Task UpdateRoleAsync(Guid roleId, UpdateRoleRequest request);

        /// <summary>
        /// Gets the role for the specified role Id
        /// </summary>
        /// <param name="roleId">the role identifier</param>
        /// <returns>a <see cref="Task"/ that contains the result of the async operation></returns>
        Task<GetRoleGroupPermissionDto> GetRoleByIdAsync(Guid roleId);

        /// <summary>
        /// Compares users against the role specified in the url.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns>a <see cref="Task"/></returns>
        Task AddUsersToRoleAsync(Guid roleId, List<Guid> model);

        /// <summary>
        /// Gets the users associated with a given role id asynchronous.
        /// </summary>
        /// <param name="roleId">The role to filter users against.</param>
        /// <returns>a <see cref="Task{PagedUsersDto}"/></returns>
        Task<GetPagedUsersDto> GetUsersByRoleAsync(Guid roleId);
    }
}