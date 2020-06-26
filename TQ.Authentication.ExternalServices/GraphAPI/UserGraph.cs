using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.ExternalServices.GraphAPI.Models;

[assembly: InternalsVisibleTo("TQ.Authentication.UnitTests")]

namespace TQ.Authentication.ExternalServices.GraphAPI
{
    /// <summary>
    /// Represents a Graph API Users Management service.
    /// </summary>
    /// <seealso cref="IUserGraph" />
    public class UserGraph : GraphCommon, IUserGraph
    {
        // Dependencies
        private readonly GraphApiClientConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserGraph"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="graphServiceClientFactory">The graph service client factory.</param>
        public UserGraph(GraphApiClientConfiguration configuration, IGraphServiceClientFactory graphServiceClientFactory)
            : base(graphServiceClientFactory)
        {
            this.configuration = configuration;
        }

        /// <inheritdoc />
        public async Task<AzureUser> GetUserAsync(Guid userId)
        {
            AzureUser result;
            User azureUser;

            try
            {
                // Get the user
                azureUser = await GetAzureUserAsync(userId);
            }
            catch (ServiceException ex)
            {
                throw new TQAuthenticationException(
                    ex.StatusCode,
                    $"{nameof(User)} with {nameof(userId)}: {userId} not found");
            }

            // Build result
            result = new AzureUser
            {
                Id = Guid.Parse(azureUser.Id),
                DisplayName = azureUser.DisplayName,
                GivenName = azureUser.GivenName,
                Surname = azureUser.Surname,
                EmailAddress = azureUser.Identities?.FirstOrDefault(x => x.SignInType == "emailAddress")?.IssuerAssignedId,
                AccountEnabled = azureUser.AccountEnabled,
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
        public async Task<AzurePagedUsers> GetPagedUsersAsync(AzureUserFilter azureUserFilter)
        {
            AzurePagedUsers result;

            // Use or set the default page size
            var recordsPerPage = azureUserFilter.PageSize > 0
                ? azureUserFilter.PageSize
                : configuration.DefaultPageSize;

            // Build Query Options
            var queryOptions = BuildQueryOptions(recordsPerPage, azureUserFilter.NextPageToken);

            // Build Filter
            var filter = BuildFilter(azureUserFilter, out bool multipleFiltersOn);

            try
            {
                // Get Azure Paged Users
                result = await GetAzureUsersAsync(queryOptions, filter);

                // if both fullname and email address filters were set
                // and if no user found by email address (default)
                // re-run the query based on the fullname instead
                if (!result.Users.Any() && multipleFiltersOn)
                {
                    var fullNameFilter = BuildNameFilter(azureUserFilter.FirstName, azureUserFilter.LastName);

                    result = await GetAzureUsersAsync(queryOptions, fullNameFilter);
                }
            }
            catch (ServiceException ex)
            {
                throw new TQAuthenticationException(
                    ex.StatusCode,
                    $"Could not get users from Azure AD");
            }

            // Return
            return result;
        }

        /// <inheritdoc />
        public async Task UpdateUserAsync(AzureUser azureUserModel)
        {
            var userObjectId = azureUserModel.Id.ToString();

            var azureUser = new User
            {
                DisplayName = azureUserModel.DisplayName,
                GivenName = azureUserModel.GivenName,
                Surname = azureUserModel.Surname
            };

            try
            {
                await GraphServiceClient.Users[userObjectId]
                    .Request()
                    .UpdateAsync(azureUser);
            }
            catch (ServiceException ex)
            {
                throw new TQAuthenticationException(
                    ex.StatusCode,
                    $"Could not update {nameof(User)} in Azure AD with Id: {userObjectId}");
            }
        }

        /// <summary>
        /// Users the exists asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<bool> UserExistsAsync(Guid userId)
        {
            var result = true;

            try
            {
                // Get user
                // If not exist, B2C will throw an exception
                await GetAzureUserAsync(userId);
            }
            catch
            {
                result = false;
            }

            // Return
            return result;
        }

        #region Members that currently are not used and need fixing

        /// <inheritdoc />
        public async Task<Guid> CreateUserAsync(AzureUser newAzureUser)
        {
            Guid result;
            User azureUser;

            var user = new User
            {
                AccountEnabled = true,
                CreationType = "LocalAccount",
                DisplayName = newAzureUser.DisplayName,
                GivenName = newAzureUser.GivenName,
                Surname = newAzureUser.Surname,
                PasswordProfile = new PasswordProfile
                {
                    ForceChangePasswordNextSignIn = true,
                    Password = newAzureUser.Password
                },
                Identities = new List<ObjectIdentity>
                {
                    new ObjectIdentity
                    {
                        Issuer = configuration.TenantName,
                        SignInType = "emailAddress",
                        IssuerAssignedId = newAzureUser.EmailAddress
                    }
                }
            };

            try
            {
                // Create user
                azureUser = await GraphServiceClient.Users
                    .Request()
                    .AddAsync(user);
            }
            catch (ServiceException ex)
            {
                throw new TQAuthenticationException(
                    ex.StatusCode,
                    $"Could not create new user in Azure AD: {ex.Message}");
            }

            // Get user's guid
            result = Guid.Parse(azureUser.Id);

            // Return
            return result;
        }

        #endregion Members that currently are not used and need fixing

        #region Private members

        /// <summary>
        /// Gets the azure user asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        private async Task<User> GetAzureUserAsync(Guid userId)
        {
            var userObjectId = userId.ToString();

            // Get the user
            var azureUser = await GraphServiceClient.Users[userObjectId]
                .Request()
                .Select(x => new
                {
                    x.Id,
                    x.DisplayName,
                    x.GivenName,
                    x.Surname,
                    x.UserPrincipalName,
                    x.AccountEnabled,
                    x.Identities,
                    x.BusinessPhones,
                    x.JobTitle,
                    x.MobilePhone,
                    x.OfficeLocation,
                    x.PreferredLanguage
                })
                .GetAsync();

            // Return
            return azureUser;
        }

        /// <summary>
        /// Gets the azure users asynchronous.
        /// </summary>
        /// <param name="queryOptions">The query options.</param>
        /// <param name="filter">The filter.</param>
        /// <remarks>marked as virtual for mocking.</remarks>
        /// <returns></returns>
        private async Task<AzurePagedUsers> GetAzureUsersAsync(List<QueryOption> queryOptions, string filter)
        {
            AzurePagedUsers result;

            // Get users
            var azureUsers = await GraphServiceClient.Users
                    .Request(queryOptions)
                    .Filter(filter)
                    .Select(x => new
                    {
                        x.Id,
                        x.DisplayName,
                        x.GivenName,
                        x.Surname,
                        x.UserPrincipalName,
                        x.AccountEnabled,
                        x.Identities,
                        x.BusinessPhones,
                        x.JobTitle,
                        x.MobilePhone,
                        x.OfficeLocation,
                        x.PreferredLanguage,
                        x.Mail
                    })
                    .GetAsync();

            // Get SkipToken, if exists
            var skipToken = azureUsers
                .NextPageRequest?
                .QueryOptions?
                .FirstOrDefault(
                    x => string.Equals("$skiptoken", x.Name, StringComparison.InvariantCultureIgnoreCase))?
                .Value;

            // Assign the SkipToken to the result
            result = new AzurePagedUsers
            {
                NextPageToken = skipToken
            };

            // Build the object result
            result.Users = azureUsers
                .Select(x => new AzureUser
                {
                    Id = Guid.Parse(x.Id),
                    DisplayName = x.DisplayName,
                    GivenName = x.GivenName,
                    Surname = x.Surname,
                    AccountEnabled = x.AccountEnabled,
                    EmailAddress = x.Identities?.FirstOrDefault(
                        x => x.SignInType == "emailAddress")?.IssuerAssignedId,
                    UserPrincipalName = x.UserPrincipalName,
                    BusinessPhones = x.BusinessPhones,
                    JobTitle = x.JobTitle,
                    MobilePhone = x.MobilePhone,
                    OfficeLocation = x.OfficeLocation,
                    PreferredLanguage = x.PreferredLanguage,
                    Mail = x.Mail
                }) ?? Enumerable.Empty<AzureUser>();

            // Return
            return result;
        }

        /// <summary>
        /// Builds Query Options.
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="nextPageToken"></param>
        /// <returns></returns>
        internal List<QueryOption> BuildQueryOptions(int pageSize, string nextPageToken)
        {
            var queryOptions = new List<QueryOption>
                {
                    new QueryOption("$top", pageSize.ToString())
                };

            if (!string.IsNullOrEmpty(nextPageToken))
            {
                queryOptions.Add(new QueryOption("$skiptoken", nextPageToken));
            }

            return queryOptions;
        }

        /// <summary>
        /// Builds the filter.
        /// </summary>
        /// <param name="azureUserFilter">The azure user filter.</param>
        /// <returns></returns>
        internal string BuildFilter(AzureUserFilter azureUserFilter, out bool multipleFiltersOn)
        {
            string filter = null;
            multipleFiltersOn = false;

            var fullNameConcat = azureUserFilter.FirstName + azureUserFilter.LastName;

            if (!string.IsNullOrWhiteSpace(fullNameConcat) && string.IsNullOrWhiteSpace(azureUserFilter.EmailAddress))
            {
                filter = BuildNameFilter(azureUserFilter.FirstName, azureUserFilter.LastName);
            }
            else if (string.IsNullOrWhiteSpace(fullNameConcat) && !string.IsNullOrWhiteSpace(azureUserFilter.EmailAddress))
            {
                filter = BuildEmailAddressFilter(azureUserFilter.EmailAddress);
            }
            else if (!string.IsNullOrWhiteSpace(fullNameConcat) && !string.IsNullOrWhiteSpace(azureUserFilter.EmailAddress))
            {
                // If both filters are set, default to Email Address filter
                filter = BuildEmailAddressFilter(azureUserFilter.EmailAddress);

                // Mark multiple filters were used
                multipleFiltersOn = true;
            }

            // Return
            return filter;
        }

        /// <summary>
        /// Builds the name filter.
        /// </summary>
        /// <param name="fullname">The fullname.</param>
        /// <returns></returns>
        private string BuildNameFilter(string firstName, string lastName)
        {
            var filterList = new List<string>();

            // Add first name filter
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                var firstNameFilter = $"startswith(givenName, '{firstName}')";

                filterList.Add(firstNameFilter);
            }

            // Add surname filter
            if (!string.IsNullOrWhiteSpace(lastName))
            {
                var lastNameFilter = $"startswith(surname, '{lastName}')";

                filterList.Add(lastNameFilter);
            }

            var joinedFilterString = string.Join(" and ", filterList);

            var filter = !string.IsNullOrWhiteSpace(joinedFilterString)
                ? joinedFilterString
                : null;

            // Return
            return filter;
        }

        /// <summary>
        /// Builds the email address filter.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <returns></returns>
        private string BuildEmailAddressFilter(string emailAddress)
        {
            string filter = null;

            // Add email address filter
            if (!string.IsNullOrWhiteSpace(emailAddress))
            {
                filter = $"Identities/any(id:id/Issuer eq '{configuration.TenantName}' and id/IssuerAssignedId eq '{emailAddress}')";
            }

            // Return
            return filter;
        }

        #endregion Private members
    }
}