using System.Linq;
using Telerik.JustMock;
using TQ.Authentication.ExternalServices.GraphAPI;
using Xunit;

namespace TQ.Authentication.UnitTests.ExternalServices.GraphApiClient.UserGraph
{
    public class BuildQueryOptions
    {
        [Fact]
        public void BuildQueryOptions_PageSize0_TokenNull()
        {
            // Arrange
            var userGraph = CreateSut();

            // Act
            var options = userGraph.BuildQueryOptions(0, null);

            // Assert
            Assert.Equal(1, options.Count(x => x.Name == "$top" && x.Value == "0"));
            Assert.Equal(0, options.Count(x => x.Name == "$skiptoken"));
        }

        [Fact]
        public void BuildQueryOptions_PageSize1_TokenNotNull()
        {
            // Arrange
            var userGraph = CreateSut();

            // Act
            var options = userGraph.BuildQueryOptions(1, "tokenstring");

            // Assert
            Assert.Equal(1, options.Count(x => x.Name == "$top" && x.Value == "1"));
            Assert.Equal(1, options.Count(x => x.Name == "$skiptoken" && x.Value == "tokenstring"));
        }

        private Authentication.ExternalServices.GraphAPI.UserGraph CreateSut()
        {
            var graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();

            return new Authentication.ExternalServices.GraphAPI.UserGraph(
                new GraphApiClientConfiguration(), graphServiceClientFactory);
        }
    }
}