using System;
using System.Net;
using System.Threading.Tasks;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.ExternalServices.GraphAPI.Models;
using TQ.Authentication.Services.Interfaces;

namespace TQ.Authentication.Services
{
    public class AzureGroupManager : IAzureGroupManager
    {
        private readonly IGraphApiClient graphApiClient;

        /// <summary>
        /// Initialises a new instance of the <see cref="AzureGroupManager"/> class
        /// </summary>
        /// <param name="graphApiClient">the graph api client</param>
        public AzureGroupManager(IGraphApiClient graphApiClient)
        {
            this.graphApiClient = graphApiClient;
        }

        /// <inheritdoc />
        public async Task<AzureGroup> GetByIdAsync(Guid groupId)
        {
            AzureGroup azureGroup;

            try
            {
                azureGroup = await graphApiClient.Groups.GetGroupAsync(groupId);
            }
            catch (Exception)
            {
                throw new TQAuthenticationException(HttpStatusCode.NotFound, $"Could not find the {nameof(AzureGroup)} with {nameof(groupId)}: {groupId} in Azure AD.");
            }

            return azureGroup;
        }
    }
}