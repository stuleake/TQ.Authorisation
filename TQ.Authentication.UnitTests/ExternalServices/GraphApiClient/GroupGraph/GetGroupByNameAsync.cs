using Microsoft.Graph;
using System;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.ExternalServices.GraphAPI;
using Xunit;

namespace TQ.Authentication.UnitTests.ExternalServices.GraphApiClient.GroupGraph
{
    public class GetGroupByNameAsync
    {
        private IGraphServiceClientFactory graphServiceClientFactory;

        [Fact]
        public async Task GetGroupByNameAsync_NameFilter_ReturnsGroup()
        {
            // Arrange
            var groupName = "Test 1";

            var graphServiceClient = Mock.Create<IGraphServiceClient>();
            Mock.Arrange(
                () => graphServiceClient.Groups.Request().Filter(Arg.AnyString).GetAsync())
                .TaskResult(new GraphServiceGroupsCollectionPage
                {
                    new Group
                    {
                        Id = Guid.NewGuid().ToString(),
                        DisplayName = "Test 1",
                        Description = "Description 1",
                        CreatedDateTime = DateTimeOffset.Now
                    },
                    new Group
                    {
                        Id = Guid.NewGuid().ToString(),
                        DisplayName = "Test 2",
                        Description = "Description 2",
                        CreatedDateTime = DateTimeOffset.Now
                    }
                });

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(graphServiceClient);

            var groupGraph = CreateSut();

            // Act
            var group = await groupGraph.GetGroupByNameAsync(groupName);

            // Assert
            Assert.Equal("Test 1", group.Name);

            Mock.Assert(() => graphServiceClient.Groups, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task GetGroupByNameAsync_NameFilter_ReturnsEmptyGroupsCollection()
        {
            // Arrange
            var groupName = "Test 1";

            var graphServiceClient = Mock.Create<IGraphServiceClient>();
            Mock.Arrange(
                () => graphServiceClient.Groups.Request().Filter(Arg.AnyString).GetAsync())
                .TaskResult(new GraphServiceGroupsCollectionPage());

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(graphServiceClient);

            var groupGraph = CreateSut();

            // Act
            var group = await groupGraph.GetGroupByNameAsync(groupName);

            // Assert
            Assert.Null(group);

            Mock.Assert(() => graphServiceClient.Groups, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task GetGroupByNameAsync_GraphApiFails_ThrowsServiceException()
        {
            // Arrange
            var groupName = "Test 1";

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
                () => groupGraph.GetGroupByNameAsync(groupName));

            Mock.Assert(() => graphServiceClient.Groups, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task GetGroupByNameAsync_FactoryCreatesNull_ThrowsException()
        {
            // Arrange
            var name = "Group name";

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(Arg.IsNull<GraphServiceClient>());

            var groupGraph = CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(
                () => groupGraph.GetGroupByNameAsync(name));

            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        private Authentication.ExternalServices.GraphAPI.GroupGraph CreateSut()
        {
            return new Authentication.ExternalServices.GraphAPI.GroupGraph(graphServiceClientFactory);
        }
    }
}