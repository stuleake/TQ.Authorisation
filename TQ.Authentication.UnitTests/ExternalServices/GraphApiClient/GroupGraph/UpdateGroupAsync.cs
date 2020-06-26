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
    public class UpdateGroupAsync
    {
        private IGraphServiceClientFactory graphServiceClientFactory;

        [Fact]
        public async Task UpdateGroupAsync_CreatesGroup()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            var createdAt = DateTime.Now;

            var model = new AzureGroup
            {
                Name = "Test",
                Description = "Description",
            };

            var graphServiceClient = Mock.Create<IGraphServiceClient>();
            Mock.Arrange(
                () => graphServiceClient.Groups[groupId.ToString()].Request().UpdateAsync(Arg.IsAny<Group>()))
                .TaskResult(new Group
                {
                    Id = groupId.ToString(),
                    DisplayName = model.Name,
                    Description = model.Description,
                    CreatedDateTime = createdAt
                });

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(graphServiceClient);

            var groupGraph = CreateSut();

            // Act
            await groupGraph.UpdateGroupAsync(model);

            Mock.Assert(() => graphServiceClient.Groups, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task UpdateGroupAsync_GraphApiFails_ThrowsServiceException()
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
            await Assert.ThrowsAsync<TQAuthenticationException>(
                () => groupGraph.UpdateGroupAsync(model));

            Mock.Assert(() => graphServiceClient.Groups, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task UpdateGroupAsync_GraphApiFails_ThrowsSystemException()
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
            await Assert.ThrowsAsync<Exception>(() => groupGraph.UpdateGroupAsync(model));

            Mock.Assert(() => graphServiceClient.Groups, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        private Authentication.ExternalServices.GraphAPI.GroupGraph CreateSut()
        {
            return new Authentication.ExternalServices.GraphAPI.GroupGraph(graphServiceClientFactory);
        }
    }
}