using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TQ.Authentication.Core.Dto.Users;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.Core.Requests.Users;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.Data.Repositories;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.ExternalServices.GraphAPI.Models;
using TQ.Authentication.Services.Interfaces;

namespace TQ.Authentication.Services
{
    /// <summary>
    /// Represents a User Manager service that allows to manage Users.
    /// </summary>
    /// <seealso cref="TQ.Authentication.Services.Interfaces.IUserManager" />
    public class UserManager : IUserManager
    {
        // Dependencies
        private readonly IGraphApiClient graphApiClient;

        private readonly IUsersRepository<User> usersRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserManager"/> class.
        /// </summary>
        /// <param name="graphApiClient">The graph API client.</param>
        /// <param name="usersRepository">The users repository.</param>
        public UserManager(IGraphApiClient graphApiClient, IUsersRepository<User> usersRepository)
        {
            this.graphApiClient = graphApiClient;
            this.usersRepository = usersRepository;
        }

        /// <inheritdoc />
        public async Task<GetPagedUsersDto> GetUsersAsync(GetFilteredUsersRequest request)
        {
            // Initialise result
            var result = new GetPagedUsersDto();

            // Get users from Azure
            var azureUserFilter = new AzureUserFilter
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailAddress = request.EmailAddress,
                PageSize = request.PageSize,
                NextPageToken = request.NextPageToken
            };

            var pagedAzureUsers = await graphApiClient.Users.GetPagedUsersAsync(azureUserFilter);

            // Get the list of UserIDs of Azure users
            var azureUserIds = pagedAzureUsers.Users.Select(x => x.Id.Value);

            // Get users from SQL
            var sqlUsers = await usersRepository.GetUsersByIdsAsync(azureUserIds);

            // Set the SkipToken, can be null
            result.NextPageToken = pagedAzureUsers.NextPageToken;

            // Get users
            result.Users = GetUsers(pagedAzureUsers, sqlUsers);

            // Return
            return result;
        }

        /// <inheritdoc />
        public async Task<UserDto> GetUserAsync(Guid userId)
        {
            UserDto result;

            // Get the user from Azure
            var azureUser = await graphApiClient.Users.GetUserAsync(userId);

            // Check if we have the user in database
            var userExistInSql = await usersRepository.IsAnyMatchedAsync(x => x.Id == userId);

            if (!userExistInSql)
            {
                throw new TQAuthenticationException(HttpStatusCode.NotFound, $"{nameof(User)} with {nameof(userId)} not found");
            }

            result = new UserDto
            {
                Id = azureUser.Id,
                DisplayName = azureUser.DisplayName,
                GivenName = azureUser.GivenName,
                Surname = azureUser.Surname,
                EmailAddress = azureUser.EmailAddress,
                AccountEnabled = azureUser.AccountEnabled,
                UserPrincipalName = azureUser.UserPrincipalName,
                BusinessPhones = azureUser.BusinessPhones,
                JobTitle = azureUser.JobTitle,
                MobilePhone = azureUser.MobilePhone,
                OfficeLocation = azureUser.OfficeLocation,
                PreferredLanguage = azureUser.PreferredLanguage,
                Mail = azureUser.Mail
            };

            // Return
            return result;
        }

        /// <inheritdoc />
        public async Task UpdateUserAsync(Guid userId, UpdateUserRequest request)
        {
            var azureUser = new AzureUser
            {
                Id = userId,
                DisplayName = request.DisplayName,
                GivenName = request.GivenName,
                Surname = request.Surname
            };

            await graphApiClient.Users.UpdateUserAsync(azureUser);
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="azurePagedUsers">The azure paged users.</param>
        /// <param name="sqlUsers">The SQL users.</param>
        /// <returns></returns>
        private IEnumerable<UserDto> GetUsers(AzurePagedUsers azurePagedUsers, IEnumerable<User> sqlUsers)
        {
            var result = (from au in azurePagedUsers.Users
                          join su in sqlUsers on
                            au.Id equals su.Id
                          select new UserDto
                          {
                              Id = au.Id,
                              DisplayName = au.DisplayName,
                              EmailAddress = au.EmailAddress,
                              GivenName = au.GivenName,
                              Surname = au.Surname,
                              AccountEnabled = au.AccountEnabled,
                              UserPrincipalName = au.UserPrincipalName,
                              BusinessPhones = au.BusinessPhones,
                              JobTitle = au.JobTitle,
                              MobilePhone = au.MobilePhone,
                              OfficeLocation = au.OfficeLocation,
                              PreferredLanguage = au.PreferredLanguage,
                              Mail = au.Mail
                          }) ?? Enumerable.Empty<UserDto>();

            // Return
            return result;
        }
    }
}