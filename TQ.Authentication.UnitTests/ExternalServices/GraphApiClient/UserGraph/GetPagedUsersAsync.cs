using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.ExternalServices.GraphAPI.Models;
using Xunit;

namespace TQ.Authentication.UnitTests.ExternalServices.GraphApiClient.UserGraph
{
    public class GetPagedUsersAsync
    {
        private IGraphServiceClientFactory graphServiceClientFactory;

        [Fact]
        public async Task GetPagedUsersAsync_ReturnsUsers()
        {
            // Arrange
            var graphServiceClient = Mock.Create<IGraphServiceClient>();

            Mock.Arrange(
                () => graphServiceClient.Users
                    .Request(Arg.IsAny<List<QueryOption>>())
                    .Filter(Arg.IsAny<string>())
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
                    }).GetAsync())
                .TaskResult(new GraphServiceUsersCollectionPage
                {
                    new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        DisplayName = "Display Name 1",
                        GivenName = "Given Name 1",
                        Surname = "Surname",
                        UserPrincipalName = "principal1@email.com",
                        AccountEnabled = true,
                        Identities = new List<ObjectIdentity>
                        {
                            new ObjectIdentity
                            {
                                Issuer = "issuer",
                                IssuerAssignedId = "my1@emailAddress.com",
                                SignInType = "emailAddress"
                            }
                        },
                        BusinessPhones = new List<string>(),
                        JobTitle = "Job title 1",
                        MobilePhone = "111",
                        OfficeLocation = "United Kingdom",
                        PreferredLanguage = "en-GB"
                    },
                    new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        DisplayName = "Display Name 2",
                        GivenName = "Given Name 2",
                        Surname = "Surname",
                        UserPrincipalName = "principal2@email.com",
                        AccountEnabled = true,
                        Identities = new List<ObjectIdentity>
                        {
                            new ObjectIdentity
                            {
                                Issuer = "issuer",
                                IssuerAssignedId = "my2@emailAddress.com",
                                SignInType = "emailAddress"
                            }
                        },
                        BusinessPhones = new List<string>(),
                        JobTitle = "Job title 2",
                        MobilePhone = "111",
                        OfficeLocation = "United Kingdom",
                        PreferredLanguage = "en-GB"
                    }
                });

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(graphServiceClient);

            var userGraph = CreateSut();

            // Act
            var azurePagedUsers = await userGraph.GetPagedUsersAsync(
                new AzureUserFilter
                {
                    PageSize = 1,
                    EmailAddress = "my2@emailAddress.com"
                });

            // Assert
            Assert.Equal(2, azurePagedUsers.Users.Count());

            Mock.Assert(() => graphServiceClient.Users, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task GetPagedUsersAsync_GraphApiFails_ThrowsServiceException()
        {
            // Arrange
            var graphServiceClient = Mock.Create<IGraphServiceClient>();
            Mock.Arrange(
                () => graphServiceClient.Users)
                .Throws(new ServiceException(null, null));

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(graphServiceClient);

            var userGraph = CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<TQAuthenticationException>(
                () => userGraph.GetPagedUsersAsync(new AzureUserFilter()));

            Mock.Assert(() => graphServiceClient.Users, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task GetPagedUsersAsync_GraphApiFails_ThrowsSystemException()
        {
            // Arrange
            var graphServiceClient = Mock.Create<IGraphServiceClient>();
            Mock.Arrange(
                () => graphServiceClient.Users)
                .Throws(new Exception());

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(graphServiceClient);

            var userGraph = CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                () => userGraph.GetPagedUsersAsync(new AzureUserFilter()));

            Mock.Assert(() => graphServiceClient.Users, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        private Authentication.ExternalServices.GraphAPI.UserGraph CreateSut()
        {
            return new Authentication.ExternalServices.GraphAPI.UserGraph(
                new GraphApiClientConfiguration
                {
                    TenantName = "terraquest",
                    DefaultPageSize = 1
                },
                graphServiceClientFactory);
        }
    }
}