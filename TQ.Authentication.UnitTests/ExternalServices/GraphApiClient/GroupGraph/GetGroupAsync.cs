using Microsoft.Graph;
using System;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.ExternalServices.GraphAPI;
using Xunit;

namespace TQ.Authentication.UnitTests.ExternalServices.GraphApiClient.GroupGraph
{
    public class GetGroupAsync
    {
        private IGraphServiceClientFactory graphServiceClientFactory;

        [Fact]
        public async Task GetGroupAsync_ReturnsGroup()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            var createdAt = DateTime.Now;

            var graphServiceClient = Mock.Create<IGraphServiceClient>();
            Mock.Arrange(
                () => graphServiceClient.Groups[groupId.ToString()].Request().GetAsync())
                .TaskResult(new Group
                {
                    Id = groupId.ToString(),
                    DisplayName = "Test",
                    Description = "Description",
                    CreatedDateTime = createdAt
                });

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(graphServiceClient);

            var groupGraph = CreateSut();

            // Act
            var group = await groupGraph.GetGroupAsync(groupId);

            // Assert
            Assert.Equal("Test", group.Name);
            Assert.Equal("Description", group.Description);
            Assert.Equal(createdAt, group.CreatedAt);

            Mock.Assert(() => graphServiceClient.Groups, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task GetGroupAsync_FactoryCreatesNull_ThrowsException()
        {
            // Arrange
            var groupId = Guid.NewGuid();

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(Arg.IsNull<IGraphServiceClient>());

            var groupGraph = CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => groupGraph.GetGroupAsync(groupId));

            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task GetGroupAsync_GraphApiFails_ThrowsServiceException()
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
            await Assert.ThrowsAsync<TQAuthenticationException>(() => groupGraph.GetGroupAsync(groupId));

            Mock.Assert(() => graphServiceClient.Groups, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        private Authentication.ExternalServices.GraphAPI.GroupGraph CreateSut()
        {
            return new Authentication.ExternalServices.GraphAPI.GroupGraph(graphServiceClientFactory);
        }
    }
}