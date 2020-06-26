using Microsoft.Graph;
using System;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.ExternalServices.GraphAPI.Models;
using Xunit;

namespace TQ.Authentication.UnitTests.ExternalServices.GraphApiClient.UserGraph
{
    public class UpdateUserAsync
    {
        private IGraphServiceClientFactory graphServiceClientFactory;

        [Fact]
        public async Task UpdateUserAsync_UpdatesUser()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var model = new AzureUser
            {
                Id = userId,
                GivenName = "Test",
                Surname = "Surname Test"
            };

            var graphServiceClient = Mock.Create<IGraphServiceClient>();
            Mock.Arrange(
                () => graphServiceClient.Users[userId.ToString()].Request().UpdateAsync(Arg.IsAny<User>()))
                .TaskResult(new User
                {
                    Id = userId.ToString(),
                    GivenName = "Test",
                    Surname = "Surname Test"
                });

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(graphServiceClient);

            var userGraph = CreateSut();

            // Act
            await userGraph.UpdateUserAsync(model);

            Mock.Assert(() => graphServiceClient.Users, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task UpdateUserAsync_GraphApiFails_ThrowsServiceException()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var model = new AzureUser
            {
                Id = userId,
                GivenName = "Test",
                Surname = "Surname Test"
            };

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
                () => userGraph.UpdateUserAsync(model));

            Mock.Assert(() => graphServiceClient.Users, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task UpdateUserAsync_GraphApiFails_ThrowsSystemException()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var model = new AzureUser
            {
                Id = userId,
                GivenName = "Test",
                Surname = "Surname Test"
            };

            var graphServiceClient = Mock.Create<IGraphServiceClient>();
            Mock.Arrange(
                () => graphServiceClient.Users)
                .Throws(new Exception());

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(graphServiceClient);

            var userGraph = CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => userGraph.UpdateUserAsync(model));

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