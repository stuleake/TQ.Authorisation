using Microsoft.Graph;
using System;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.ExternalServices.GraphAPI;
using Xunit;

namespace TQ.Authentication.UnitTests.ExternalServices.GraphApiClient.UserGraph
{
    public class UserExistsAsync
    {
        private IGraphServiceClientFactory graphServiceClientFactory;

        [Fact]
        public async Task UserExistsAsync_ReturnsTrue()
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
                .TaskResult(Arg.IsAny<User>());

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(graphServiceClient);

            var userGraph = CreateSut();

            // Act
            var exists = await userGraph.UserExistsAsync(userId);

            // Assert
            Assert.True(exists);

            Mock.Assert(() => graphServiceClient.Users, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task UserExistsAsync_UserGraphFails_ResultFalse()
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
                .Throws<Exception>(Arg.IsAny<User>());

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(graphServiceClient);

            var userGraph = CreateSut();

            // Act
            var exists = await userGraph.UserExistsAsync(userId);

            // Assert
            Assert.False(exists);

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