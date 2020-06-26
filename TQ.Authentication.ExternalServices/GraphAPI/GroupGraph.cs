using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.ExternalServices.GraphAPI.Models;

namespace TQ.Authentication.ExternalServices.GraphAPI
{
    /// <summary>
    /// Represents a Graph API Groups Management service.
    /// </summary>
    /// <seealso cref="GraphCommon" />
    /// <seealso cref="IGroupGraph" />
    public class GroupGraph : GraphCommon, IGroupGraph
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupGraph"/> class.
        /// </summary>
        /// <param name="graphServiceClientFactory">The graph service client factory.</param>
        public GroupGraph(IGraphServiceClientFactory graphServiceClientFactory)
            : base(graphServiceClientFactory)
        { }

        /// <inheritdoc />
        public async Task<AzureGroup> GetGroupAsync(Guid groupId)
        {
            AzureGroup result;
            Group azureGroup;

            try
            {
                // Get group
                azureGroup = await GetAzureGroupAsync(groupId);
            }
            catch (ServiceException ex)
            {
                throw new TQAuthenticationException(
                    ex.StatusCode,
                    $"{nameof(Group)} with {nameof(groupId)} not found");
            }

            // Build the object result
            result = new AzureGroup
            {
                Id = Guid.Parse(azureGroup.Id),
                Name = azureGroup.DisplayName,
                Description = azureGroup.Description,
                CreatedAt = azureGroup.CreatedDateTime.Value.DateTime
            };

            // Return
            return result;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<AzureGroup>> GetGroupsAsync()
        {
            IEnumerable<AzureGroup> result;
            IGraphServiceGroupsCollectionPage azureGroups;

            try
            {
                // Get groups
                azureGroups = await GraphServiceClient.Groups
                    .Request()
                    .GetAsync();
            }
            catch (ServiceException ex)
            {
                throw new TQAuthenticationException(
                    ex.StatusCode,
                    $"Could not get groups from Azure AD");
            }

            // Build the object result
            result = azureGroups
                .Select(x => new AzureGroup
                {
                    Id = Guid.Parse(x.Id),
                    Name = x.DisplayName,
                    Description = x.Description,
                    CreatedAt = x.CreatedDateTime.Value.DateTime
                }) ?? Enumerable.Empty<AzureGroup>();

            // Return
            return result;
        }

        /// <inheritdoc />
        public async Task<AzureGroup> GetGroupByNameAsync(string name)
        {
            AzureGroup result = null;
            IGraphServiceGroupsCollectionPage azureGroups;

            try
            {
                // Get group
                azureGroups = await GraphServiceClient.Groups
                    .Request()
                    .Filter($"displayName eq '{name}'")
                    .GetAsync();

                if (azureGroups.Count == 0)
                {
                    return result;
                }
            }
            catch (ServiceException ex)
            {
                throw new TQAuthenticationException(
                    ex.StatusCode,
                    $"Could not find group {name} Azure AD: {ex.Message}");
            }

            // Build the object result
            // Although Azure AD allows having multiple groups
            // with the same name, they always should be unique
            result = azureGroups
                .Select(x => new AzureGroup
                {
                    Id = Guid.Parse(x.Id),
                    Name = x.DisplayName,
                    Description = x.Description,
                    CreatedAt = x.CreatedDateTime.Value.DateTime
                }).FirstOrDefault();

            // Return
            return result;
        }

        /// <inheritdoc />
        public async Task<AzureGroup> CreateGroupAsync(AzureGroup newAzureGroup)
        {
            AzureGroup result;
            Group azureGroup;

            var group = new Group
            {
                DisplayName = newAzureGroup.Name,
                Description = newAzureGroup.Description,
                GroupTypes = null,
                MailEnabled = false,
                MailNickname = "NoReply",
                SecurityEnabled = true
            };

            try
            {
                // Create group in Azure AD
                azureGroup = await GraphServiceClient.Groups
                    .Request()
                    .AddAsync(group);
            }
            catch (ServiceException ex)
            {
                throw new TQAuthenticationException(
                    ex.StatusCode,
                    $"Could not create new group in Azure AD: {ex.Message}");
            }

            // Get the Guid of the new group in Azure AD
            result = new AzureGroup
            {
                Id = Guid.Parse(azureGroup.Id),
                Name = azureGroup.DisplayName,
                Description = azureGroup.Description,
                CreatedAt = azureGroup.CreatedDateTime.Value.DateTime
            };

            // Return
            return result;
        }

        /// <inheritdoc />
        public async Task UpdateGroupAsync(AzureGroup model)
        {
            var groupObjectId = model.Id.ToString();

            var azureGroup = new Group
            {
                DisplayName = model.Name,
                Description = model.Description
            };

            try
            {
                await GraphServiceClient.Groups[groupObjectId]
                    .Request()
                    .UpdateAsync(azureGroup);
            }
            catch (ServiceException ex)
            {
                throw new TQAuthenticationException(
                    ex.StatusCode,
                    $"Could not update group in Azure AD: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task DeleteGroupAsync(Guid groupId)
        {
            var groupObjectId = groupId.ToString();

            try
            {
                await GraphServiceClient.Groups[groupObjectId]
                    .Request()
                    .DeleteAsync();
            }
            catch (ServiceException ex)
            {
                throw new TQAuthenticationException(
                    ex.StatusCode,
                    $"Could not delete group from Azure AD: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task AddUserAsync(Guid groupId, Guid userId)
        {
            // Convert guids to strings
            var userObjectId = userId.ToString();
            var groupObjectId = groupId.ToString();

            // Create directory object
            var directoryObject = new DirectoryObject
            {
                Id = userObjectId
            };

            try
            {
                // Add user to group
                await GraphServiceClient.Groups[groupObjectId].Members.References
                    .Request()
                    .AddAsync(directoryObject);
            }
            catch (ServiceException ex)
            {
                throw new TQAuthenticationException(
                    ex.StatusCode,
                    $"Could not add user {userObjectId} to group {groupObjectId} in Azure AD: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task RemoveUserAsync(Guid groupId, Guid userId)
        {
            // Convert guids to strings
            var userObjectId = userId.ToString();
            var groupObjectId = groupId.ToString();

            try
            {
                // Remove user from group
                await GraphServiceClient.Groups[groupObjectId].Members[userObjectId].Reference
                    .Request()
                    .DeleteAsync();
            }
            catch (ServiceException ex)
            {
                throw new TQAuthenticationException(
                    ex.StatusCode,
                    $"Could not remove user {userObjectId} from group {groupObjectId} in Azure AD: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<bool> GroupExistsAsync(Guid groupId)
        {
            var result = true;

            try
            {
                await GetAzureGroupAsync(groupId);
            }
            catch
            {
                result = false;
            }

            // Return
            return result;
        }

        /// <inheritdoc />
        public async Task<bool> UserExistsInGroupAsync(Guid groupId, Guid userId)
        {
            var result = true;

            var groupObjectId = groupId.ToString();
            var userObjectId = userId.ToString();

            try
            {
                // If group or user does not exist, the B2C will throw an exception.
                await GraphServiceClient
                    .Groups[groupObjectId]
                    .Members[userObjectId]
                    .Request()
                    .GetAsync();
            }
            catch
            {
                result = false;
            }

            // Return
            return result;
        }

        /// <summary>
        /// Gets the azure group asynchronous.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns></returns>
        private async Task<Group> GetAzureGroupAsync(Guid groupId)
        {
            var azureGroupId = groupId.ToString();

            // Get group
            // If group does not exist, the B2C will throw an exception
            var azureGroup = await GraphServiceClient.Groups[azureGroupId]
                    .Request()
                    .GetAsync();

            // Return
            return azureGroup;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<AzureGroup>> GetAzureGroupsByUserIdAsync(Guid userId)
        {
            IUserMemberOfCollectionWithReferencesPage members;

            var azureUserId = userId.ToString();

            try
            {
                members = await GraphServiceClient.Users[azureUserId].MemberOf.Request().GetAsync();
            }
            catch (ServiceException ex)
            {
                throw new TQAuthenticationException(
                    ex.StatusCode,
                    $"Could not get groups from Azure AD: {ex.Message}");
            }

            // filter out any type which is not a Group e.g. a DirectoryRole
            var groups = members.ToList().OfType<Group>();

            var result = groups.Select(group =>
                new AzureGroup()
                {
                    Id = Guid.Parse(group.Id),
                    Name = group.DisplayName,
                    Description = group.Description,
                    CreatedAt = group.CreatedDateTime.Value.DateTime
                }) ?? Enumerable.Empty<AzureGroup>();

            return result;
        }
    }
}