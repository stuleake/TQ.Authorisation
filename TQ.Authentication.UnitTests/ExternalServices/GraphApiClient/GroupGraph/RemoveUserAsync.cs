using Microsoft.Graph;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.ExternalServices.GraphAPI;
using Xunit;

namespace TQ.Authentication.UnitTests.ExternalServices.GraphApiClient.GroupGraph
{
    public class RemoveUserAsync
    {
        private IGraphServiceClientFactory graphServiceClientFactory;

        [Fact]
        public async Task RemoveUserAsync_AddsUserToGroup()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var groupId = Guid.NewGuid();

            var graphServiceClient = Mock.Create<IGraphServiceClient>();
            Mock.Arrange(
                () => graphServiceClient.Groups[groupId.ToString()]
                    .Members[userId.ToString()].Reference.Request()
                    .DeleteAsync(Arg.IsAny<CancellationToken>()));

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(graphServiceClient);

            var groupGraph = CreateSut();

            // Act & Assert
            await groupGraph.RemoveUserAsync(groupId, userId);

            Mock.Assert(() => graphServiceClient.Groups, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task RemoveUserAsync_FactoryCreatesNull_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var groupId = Guid.NewGuid();

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(Arg.IsNull<IGraphServiceClient>());

            var groupGraph = CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(
                () => groupGraph.RemoveUserAsync(groupId, userId));

            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task RemoveUserAsync_GraphApiFails_ThrowsServiceException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var groupId = Guid.NewGuid();

            var graphServiceClient = Mock.Create<IGraphServiceClient>();
            Mock.Arrange(
                () => graphServiceClient.Groups)
                .Throws(new ServiceException(null, null));

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(graphServiceClient);

            var groupGraph = CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<TQAuthenticationException>(
                () => groupGraph.RemoveUserAsync(groupId, userId));

            Mock.Assert(() => graphServiceClient.Groups, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task RemoveUserAsync_GraphApiFails_ThrowsSystemException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var groupId = Guid.NewGuid();

            var graphServiceClient = Mock.Create<IGraphServiceClient>();
            Mock.Arrange(
                () => graphServiceClient.Groups)
                .Throws(new Exception());

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(graphServiceClient);

            var groupGraph = CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                 () => groupGraph.RemoveUserAsync(groupId, userId));

            Mock.Assert(() => graphServiceClient.Groups, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        private Authentication.ExternalServices.GraphAPI.GroupGraph CreateSut()
        {
            return new Authentication.ExternalServices.GraphAPI.GroupGraph(graphServiceClientFactory);
        }
    }
}