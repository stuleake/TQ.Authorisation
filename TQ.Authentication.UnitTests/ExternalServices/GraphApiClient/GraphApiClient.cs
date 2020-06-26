using Telerik.JustMock;
using TQ.Authentication.ExternalServices.GraphAPI;
using Xunit;

namespace TQ.Authentication.UnitTests.ExternalServices.GraphApiClient
{
    public class GraphApiClient
    {
        [Fact]
        public void GraphApiClient_UsersObjectIsSet()
        {
            // Arrange
            var userGraph = Mock.Create<IUserGraph>();
            var groupGraph = Mock.Create<IGroupGraph>();

            // Act
            var graphApiClient =
                new Authentication.ExternalServices.GraphAPI.GraphApiClient(groupGraph, userGraph);

            // Assert
            Assert.NotNull(graphApiClient.Users);
        }

        [Fact]
        public void GraphApiClient_GroupsObjectIsSet()
        {
            // Arrange
            var userGraph = Mock.Create<IUserGraph>();
            var groupGraph = Mock.Create<IGroupGraph>();

            // Act
            var graphApiClient =
                new Authentication.ExternalServices.GraphAPI.GraphApiClient(groupGraph, userGraph);

            // Assert
            Assert.NotNull(graphApiClient.Groups);
        }
    }
}