using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.ExternalServices.GraphAPI;
using Xunit;

namespace TQ.Authentication.UnitTests.ExternalServices.GraphApiClient.UserGraph
{
    public class GetUserAsync
    {
        private IGraphServiceClientFactory graphServiceClientFactory;

        [Fact]
        public async Task GetUserAsync_ReturnsUser()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var graphServiceClient = Mock.Create<IGraphServiceClient>();

            Mock.Arrange(
                () => graphServiceClient.Users[userId.ToString()].Request().Select(x => new
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
                .TaskResult(new User
                {
                    Id = userId.ToString(),
                    DisplayName = "Display Name",
                    GivenName = "Given Name",
                    Surname = "Surname",
                    UserPrincipalName = "principal@email.com",
                    AccountEnabled = true,
                    Identities = new List<ObjectIdentity>
                    {
                        new ObjectIdentity
                        {
                            Issuer = "issuer",
                            IssuerAssignedId = "my@emailAddress.com",
                            SignInType = "emailAddress"
                        }
                    },
                    BusinessPhones = new List<string>(),
                    JobTitle = "Job title",
                    MobilePhone = "111",
                    OfficeLocation = "United Kingdom",
                    PreferredLanguage = "en-GB"
                });

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(graphServiceClient);

            var userGraph = CreateSut();

            // Act
            var user = await userGraph.GetUserAsync(userId);

            // Assert
            Assert.Equal("Display Name", user.DisplayName);
            Assert.Equal("Given Name", user.GivenName);
            Assert.Equal("Surname", user.Surname);
            Assert.True(user.AccountEnabled);
            Assert.NotNull(user.BusinessPhones);
            Assert.Equal("my@emailAddress.com", user.EmailAddress);
            Assert.Equal("Job title", user.JobTitle);
            Assert.Equal("111", user.MobilePhone);
            Assert.Equal("United Kingdom", user.OfficeLocation);
            Assert.Equal("en-GB", user.PreferredLanguage);

            Mock.Assert(() => graphServiceClient.Users, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task GetUserAsync_GraphApiFails_ThrowsServiceException()
        {
            // Arrange
            var userId = Guid.NewGuid();

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
                () => userGraph.GetUserAsync(userId));

            Mock.Assert(() => graphServiceClient.Users, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task GetUserAsync_GraphApiFails_ThrowsSystemException()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var graphServiceClient = Mock.Create<IGraphServiceClient>();
            Mock.Arrange(
                () => graphServiceClient.Users)
                .Throws(new Exception());

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(graphServiceClient);

            var userGraph = CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => userGraph.GetUserAsync(userId));

            Mock.Assert(() => graphServiceClient.Users, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        private Authentication.ExternalServices.GraphAPI.UserGraph CreateSut()
        {
            return new Authentication.ExternalServices.GraphAPI.UserGraph(
                new GraphApiClientConfiguration(), graphServiceClientFactory);
        }
    }
}