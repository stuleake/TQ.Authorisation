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
    public class DeleteGroupAsync
    {
        private IGraphServiceClientFactory graphServiceClientFactory;

        [Fact]
        public async Task DeleteGroupAsync_DeletesGroup()
        {
            // Arrange
            var groupId = Guid.NewGuid();

            var graphServiceClient = Mock.Create<IGraphServiceClient>();
            Mock.Arrange(
                () => graphServiceClient
                    .Groups[groupId.ToString()]
                    .Request()
                    .DeleteAsync(Arg.IsAny<CancellationToken>()));

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(graphServiceClient);

            var groupGraph = CreateSut();

            // Act
            await groupGraph.DeleteGroupAsync(groupId);

            Mock.Assert(() => graphServiceClient.Groups, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task DeleteGroupAsync_GraphApiFails_ThrowsServiceException()
        {
            // Arrange
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
                () => groupGraph.DeleteGroupAsync(groupId));

            Mock.Assert(() => graphServiceClient.Groups, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task DeleteGroupAsync_GraphApiFails_ThrowsSystemException()
        {
            // Arrange
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
            await Assert.ThrowsAsync<Exception>(() => groupGraph.DeleteGroupAsync(groupId));

            Mock.Assert(() => graphServiceClient.Groups, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        private Authentication.ExternalServices.GraphAPI.GroupGraph CreateSut()
        {
            return new Authentication.ExternalServices.GraphAPI.GroupGraph(graphServiceClientFactory);
        }
    }
}