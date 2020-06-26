using Microsoft.Graph;
using System;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.ExternalServices.GraphAPI;
using Xunit;

namespace TQ.Authentication.UnitTests.ExternalServices.GraphApiClient.GroupGraph
{
    public class UserExistsInGroupAsync
    {
        private IGraphServiceClientFactory graphServiceClientFactory;

        [Fact]
        public async Task UserExistsInGroupAsync_ReturnsTrue()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var graphServiceClient = Mock.Create<IGraphServiceClient>();
            Mock.Arrange(
                () => graphServiceClient
                    .Groups[groupId.ToString()]
                    .Members[userId.ToString()]
                    .Request()
                    .GetAsync())
                .TaskResult(Arg.IsAny<DirectoryObject>());

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(graphServiceClient);

            var groupGraph = CreateSut();

            // Act
            var exists = await groupGraph.UserExistsInGroupAsync(groupId, userId);

            // Assert
            Assert.True(exists);

            Mock.Assert(() => graphServiceClient.Groups, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task UserExistsInGroupAsync_ReturnsFalse()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var graphServiceClient = Mock.Create<IGraphServiceClient>();
            Mock.Arrange(
                () => graphServiceClient
                    .Groups[groupId.ToString()]
                    .Members[userId.ToString()]
                    .Request()
                    .GetAsync())
                .Throws<Exception>();

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(graphServiceClient);

            var groupGraph = CreateSut();

            // Act
            var exists = await groupGraph.UserExistsInGroupAsync(groupId, userId);

            // Assert
            Assert.False(exists);

            Mock.Assert(() => graphServiceClient.Groups, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        private Authentication.ExternalServices.GraphAPI.GroupGraph CreateSut()
        {
            return new Authentication.ExternalServices.GraphAPI.GroupGraph(graphServiceClientFactory);
        }
    }
}