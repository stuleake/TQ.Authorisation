using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.Authentication.Core.Dto.Groups;
using TQ.Authentication.Core.Dto.Permissions;
using TQ.Authentication.Core.Requests.Groups;
using TQ.Authentication.Core.Requests.Paging;
using TQ.Authentication.Data;
using TQ.Authentication.Core.Dto.Roles;
using TQ.Authentication.Data.Entities;

namespace TQ.Authentication.Services.Interfaces
{
    /// <summary>
    /// Defines a Group Manager service that allows to manage Groups.
    /// </summary>
    public interface IGroupManager
    {
        /// <summary>
        /// Gets the groups asynchronous.
        /// </summary>
        /// <returns>a <see cref="Task"/> that contains <see cref="IEnumerable{GroupGetDto}"/></returns>
        Task<IEnumerable<GetGroupGetDto>> GetGroupsAsync();

        /// <summary>
        /// Gets the group asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns>a <see cref="Task{GroupGetDto}"/></returns>
        Task<GetGroupGetDto> GetGroupAsync(Guid groupId);

        /// <summary>
        /// Creates the group asynchronous.
        /// </summary>
        /// <param name="groupDto">The group.</param>
        /// <returns>a <see cref="Task{GroupGetDto}"/></returns>
        Task<GetGroupGetDto> CreateGroupAsync(CreateGroupRequest request);

        /// <summary>
        /// Updates the group asynchronous.
        /// </summary>
        /// <param name="request">The request that contains the details of the group to update</param>
        /// <param name="groupId">The id of the group to update</param>
        /// <returns>a <see cref="Task"/ that contains the result of the asynchronous action></returns>
        Task UpdateGroupAsync(Guid groupId, UpdateGroupRequest request);

        /// <summary>
        /// Adds the user asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>a <see cref="Task{UserGroupDto}"/></returns>
        Task<UserGroupDto> AddUserAsync(Guid groupId, Guid userId);

        /// <summary>
        /// Removes the user asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>a <see cref="Task"/></returns>
        Task RemoveUserAsync(Guid groupId, Guid userId);

        /// <summary>
        /// Gets the Group from the supplied Id
        /// </summary>
        /// <param name="groupId">the group id</param>
        /// <returns>a <see cref="Task"/ that contains the result of the async operation></returns>
        Task<Group> GetGroupByIdAsync(Guid groupId);

        /// <summary>
        /// Gets the group permissions asynchronously.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns>a <see cref="Task{IEnumerable{GetPermissionsDto}}"/></returns>
        Task<IEnumerable<GetPermissionsDto>> GetGroupPermissionsAsync(Guid groupId);

        /// <summary>
        /// Gets the roles for the group that match the supplied Id
        /// </summary>
        /// <param name="groupId">the group id</param>
        /// <param name="request">the request that includes the paging properties</param>
        /// <returns>a <see cref="Task"/ that contains the result of the async operation></returns>
        Task<PagedList<GetRoleDto>> GetRolesAsync(Guid groupId, GetPagedRequest request);
    }
}