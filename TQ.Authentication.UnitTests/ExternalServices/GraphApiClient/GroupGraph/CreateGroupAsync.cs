using Microsoft.Graph;
using System;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.ExternalServices.GraphAPI.Models;
using Xunit;

namespace TQ.Authentication.UnitTests.ExternalServices.GraphApiClient.GroupGraph
{
    public class CreateGroupAsync
    {
        private IGraphServiceClientFactory graphServiceClientFactory;

        [Fact]
        public async Task CreateGroupAsync_CreatesGroup()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            var createdAt = DateTime.Now;

            var model = new AzureGroup
            {
                Name = "Test",
                Description = "Description",
                CreatedAt = createdAt
            };

            var graphServiceClient = Mock.Create<IGraphServiceClient>();
            Mock.Arrange(
                () => graphServiceClient.Groups.Request().AddAsync(Arg.IsAny<Group>()))
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

            var graphApiClient = CreateSut();

            // Act
            var newGroup = await graphApiClient.CreateGroupAsync(model);

            // Assert
            Assert.Equal(groupId, newGroup.Id);
            Assert.Equal("Test", newGroup.Name);
            Assert.Equal("Description", newGroup.Description);

            Mock.Assert(() => graphServiceClient.Groups, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task CreateGroupAsync_GraphApiFails_ThrowsServiceException()
        {
            // Arrange
            var createdAt = DateTime.Now;

            var model = new AzureGroup
            {
                Name = "Test",
                Description = "Description",
                CreatedAt = createdAt
            };

            var graphServiceClient = Mock.Create<IGraphServiceClient>();
            Mock.Arrange(
                () => graphServiceClient.Groups)
                .Throws(new ServiceException(null, null));

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(graphServiceClient);

            var groupGraph = CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<TQAuthenticationException>(() => groupGraph.CreateGroupAsync(model));

            Mock.Assert(() => graphServiceClient.Groups, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task CreateGroupAsync_GraphApiFails_ThrowsSystemException()
        {
            // Arrange
            var createdAt = DateTime.Now;

            var model = new AzureGroup
            {
                Name = "Test",
                Description = "Description",
                CreatedAt = createdAt
            };

            var graphServiceClient = Mock.Create<IGraphServiceClient>();
            Mock.Arrange(
                () => graphServiceClient.Groups)
                .Throws(new Exception());

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(graphServiceClient);

            var groupGraph = CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => groupGraph.CreateGroupAsync(model));

            Mock.Assert(() => graphServiceClient.Groups, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        private Authentication.ExternalServices.GraphAPI.GroupGraph CreateSut()
        {
            return new Authentication.ExternalServices.GraphAPI.GroupGraph(graphServiceClientFactory);
        }
    }
}