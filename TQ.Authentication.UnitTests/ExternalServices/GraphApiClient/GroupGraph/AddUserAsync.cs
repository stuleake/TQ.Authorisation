using Microsoft.Graph;
using System;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.ExternalServices.GraphAPI;
using Xunit;

namespace TQ.Authentication.UnitTests.ExternalServices.GraphApiClient.GroupGraph
{
    public class AddUserAsync
    {
        private IGraphServiceClientFactory graphServiceClientFactory;

        [Fact]
        public async Task AddUserAsync_AddsUserToGroup()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var groupId = Guid.NewGuid();

            var directoryObject = new DirectoryObject
            {
                Id = userId.ToString()
            };

            var graphServiceClient = Mock.Create<IGraphServiceClient>();
            Mock.Arrange(
                () => graphServiceClient.Groups[Arg.IsAny<string>()]
                    .Members.References.Request()
                    .AddAsync(directoryObject));

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(graphServiceClient);

            var groupGraph = CreateSut();

            // Act & Assert
            await groupGraph.AddUserAsync(groupId, userId);

            Mock.Assert(() => graphServiceClient.Groups, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task AddUserAsync_FactoryCreatesNull_ThrowsException()
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
                () => groupGraph.AddUserAsync(groupId, userId));

            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task AddUserAsync_GraphApiFails_ThrowsServiceException()
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
                () => groupGraph.AddUserAsync(groupId, userId));

            Mock.Assert(() => graphServiceClient.Groups, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task AddUserAsync_GraphApiFails_ThrowsSystemException()
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
                 () => groupGraph.AddUserAsync(groupId, userId));

            Mock.Assert(() => graphServiceClient.Groups, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        private Authentication.ExternalServices.GraphAPI.GroupGraph CreateSut()
        {
            return new Authentication.ExternalServices.GraphAPI.GroupGraph(graphServiceClientFactory);
        }
    }
}