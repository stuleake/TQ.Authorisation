using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.Authentication.ExternalServices.GraphAPI.Models;

namespace TQ.Authentication.ExternalServices.GraphAPI
{
    /// <summary>
    /// Defines an interface for Graph API Groups Management.
    /// </summary>
    public interface IGroupGraph
    {
        /// <summary>
        /// Gets the group asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns>a <see cref="Task{AzureGroup}"/></returns>
        Task<AzureGroup> GetGroupAsync(Guid groupId);

        /// <summary>
        /// Gets the groups asynchronous.
        /// </summary>
        /// <returns></returns>
        /// <returns>a <see cref="Task"/> that contains <see cref="IEnumerable{AzureGroup}"/> </returns>
        Task<IEnumerable<AzureGroup>> GetGroupsAsync();

        /// <summary>
        /// Gets the group by name asynchronous.
        /// Returns null if no group found.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>a <see cref="Task{AzureGroup}"/></returns>
        Task<AzureGroup> GetGroupByNameAsync(string name);

        /// <summary>
        /// Creates the group asynchronous.
        /// </summary>
        /// <param name="newAzureGroup">The new azure group.</param>
        /// <returns>a <see cref="Task{AzureGroup}"/></returns>
        Task<AzureGroup> CreateGroupAsync(AzureGroup newAzureGroup);

        /// <summary>
        /// Updates the group asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>a <see cref="Task"/></returns>
        Task UpdateGroupAsync(AzureGroup model);

        /// <summary>
        /// Deletes the group asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns>a <see cref="Task"/></returns>
        Task DeleteGroupAsync(Guid groupId);

        /// <summary>
        /// Adds the user asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>a <see cref="Task"/></returns>
        Task AddUserAsync(Guid groupId, Guid userId);

        /// <summary>
        /// Removes the user asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>a <see cref="Task"/></returns>
        Task RemoveUserAsync(Guid groupId, Guid userId);

        /// <summary>
        /// Group exists asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns>a <see cref="Task{bool}"/></returns>
        Task<bool> GroupExistsAsync(Guid groupId);

        /// <summary>
        /// Users the exists in group asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>a <see cref="Task{bool}"/></returns>
        Task<bool> UserExistsInGroupAsync(Guid groupId, Guid userId);

        /// <summary>
        /// Gets the azure groups by userid asynchronously.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>a <see cref="Task"/> that contains <see cref="IEnumerable{AzureGroup}"/> </returns>
        Task<IEnumerable<AzureGroup>> GetAzureGroupsByUserIdAsync(Guid userId);
    }
}