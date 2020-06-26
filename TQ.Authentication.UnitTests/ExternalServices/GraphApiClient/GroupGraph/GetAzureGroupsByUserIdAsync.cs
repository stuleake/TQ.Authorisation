using FluentAssertions;
using Microsoft.Graph;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.ExternalServices.GraphAPI.Models;
using Xunit;

namespace TQ.Authentication.UnitTests.ExternalServices.GraphApiClient.GroupGraph
{
    public class GetAzureGroupsByUserIdAsync
    {
        private IGraphServiceClientFactory graphServiceClientFactory;

        [Fact]
        public async Task GetAzureGroupsByUserIdAsyncReturnsGroups()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var groupId = Guid.NewGuid();

            var azureGroup = new Group
            { 
                Id = groupId.ToString(), 
                DisplayName = "GroupName",
                Description = "Group Description",
                CreatedDateTime = DateTime.Now
            };

            var azureGroups = new UserMemberOfCollectionWithReferencesPage() { azureGroup };

            var expectedResult = new AzureGroup()
            {
                Id = groupId,
                Name = azureGroup.DisplayName,
                Description = azureGroup.Description,
                CreatedAt = azureGroup.CreatedDateTime.Value.DateTime
            };

            var graphServiceClient = Mock.Create<IGraphServiceClient>();
            Mock.Arrange(
                () => graphServiceClient
                    .Users[userId.ToString()]
                    .MemberOf
                    .Request()
                    .GetAsync())
                    .TaskResult(azureGroups);

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create()).Returns(graphServiceClient);

            var groupGraph = CreateSut();

            // Act
            var result = await groupGraph.GetAzureGroupsByUserIdAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.ToList().FirstOrDefault().Should().BeEquivalentTo(expectedResult);

            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task GetAzureGroupsByUserIdAsyncReturnsNoGroups()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var groupId = Guid.NewGuid();
            var azureGroups = new UserMemberOfCollectionWithReferencesPage();

            var graphServiceClient = Mock.Create<IGraphServiceClient>();
            Mock.Arrange(
                () => graphServiceClient
                    .Users[userId.ToString()]
                    .MemberOf
                    .Request()
                    .GetAsync())
                    .TaskResult(azureGroups);

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create()).Returns(graphServiceClient);

            var groupGraph = CreateSut();

            // Act
            var result = await groupGraph.GetAzureGroupsByUserIdAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(0);

            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }
        [Fact]
        public async Task GetAzureGroupsByUserIdAsyncIgnoresRoles()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var azureRoles = new UserMemberOfCollectionWithReferencesPage()
            {
                new DirectoryRole
                {
                    Id = Guid.NewGuid().ToString(),
                    DisplayName = "RoleName",
                    Description = "Role Description"
                }
            };

            var graphServiceClient = Mock.Create<IGraphServiceClient>();
            Mock.Arrange(
                () => graphServiceClient
                    .Users[userId.ToString()]
                    .MemberOf
                    .Request()
                    .GetAsync())
                    .TaskResult(azureRoles);

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create()).Returns(graphServiceClient);

            var groupGraph = CreateSut();

            // Act
            var result = await groupGraph.GetAzureGroupsByUserIdAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(0);
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task GetAzureGroupsByUserIdAsyncThrowsTQAuthenticationExceptionWhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var graphServiceClient = Mock.Create<IGraphServiceClient>();
            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create()).Returns(graphServiceClient);
            Mock.Arrange(() => graphServiceClient.Users).Throws(new ServiceException(new Error()));

            var groupGraph = CreateSut();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<TQAuthenticationException>(() => groupGraph.GetAzureGroupsByUserIdAsync(userId));
            exception.Message.Should().Be($"Could not get groups from Azure AD: ");
        }

        private Authentication.ExternalServices.GraphAPI.GroupGraph CreateSut()
        {
            return new Authentication.ExternalServices.GraphAPI.GroupGraph(graphServiceClientFactory);
        }
    }
}