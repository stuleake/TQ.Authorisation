using System;
using System.Threading.Tasks;
using TQ.Authentication.ExternalServices.GraphAPI.Models;

namespace TQ.Authentication.Services.Interfaces
{
    public interface IAzureGroupManager
    {
        /// <summary>
        /// Gets the Azure group from the supplied Id
        /// </summary>
        /// <param name="groupId">the group id</param>
        /// <returns>a <see cref="Task"/ that contains the result of the async operation></returns>
        Task<AzureGroup> GetByIdAsync(Guid groupId);
    }
}