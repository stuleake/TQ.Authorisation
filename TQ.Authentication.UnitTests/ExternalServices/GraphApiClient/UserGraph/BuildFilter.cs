using Telerik.JustMock;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.ExternalServices.GraphAPI.Models;
using Xunit;

namespace TQ.Authentication.UnitTests.ExternalServices.GraphApiClient.UserGraph
{
    public class BuildFilter
    {
        [Fact]
        public void BuildFilter_FilterDefaultValues_ReturnsNull_MultipleFiltersOff()
        {
            // Arrange
            var userGraph = CreateSut();

            var azureFilter = new AzureUserFilter();

            // Act
            var filter = userGraph.BuildFilter(azureFilter, out var multipleFiltersOn);

            // Assert
            Assert.Null(filter);
            Assert.False(multipleFiltersOn);
        }

        [Fact]
        public void BuildFilter_FilterEmptyStringValues_ReturnsNull_MultipleFiltersOff()
        {
            // Arrange
            var userGraph = CreateSut();

            var azureFilter = new AzureUserFilter
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                EmailAddress = string.Empty
            };

            // Act
            var filter = userGraph.BuildFilter(azureFilter, out var multipleFiltersOn);

            // Assert
            Assert.Null(filter);
            Assert.False(multipleFiltersOn);
        }

        [Fact]
        public void BuildFilter_FilterEmailNull_ReturnsNameFilter_MultipleFiltersOff()
        {
            // Arrange
            var userGraph = CreateSut();

            var azureFilter = new AzureUserFilter
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                EmailAddress = null
            };

            // Act
            var filter = userGraph.BuildFilter(azureFilter, out var multipleFiltersOn);

            // Assert
            Assert.Contains("startswith", filter);
            Assert.False(multipleFiltersOn);
        }

        [Fact]
        public void BuildFilter_LastNameNull_FilterEmailNull_ReturnsNameFilter_MultipleFiltersOff()
        {
            // Arrange
            var userGraph = CreateSut();

            var azureFilter = new AzureUserFilter
            {
                FirstName = "TestFirstName",
                LastName = null,
                EmailAddress = null
            };

            // Act
            var filter = userGraph.BuildFilter(azureFilter, out var multipleFiltersOn);

            // Assert
            Assert.Contains("startswith", filter);
            Assert.False(multipleFiltersOn);
        }

        [Fact]
        public void BuildFilter_FirstNameNull_FilterEmailNull_ReturnsNameFilter_MultipleFiltersOff()
        {
            // Arrange
            var userGraph = CreateSut();

            var azureFilter = new AzureUserFilter
            {
                FirstName = null,
                LastName = "TestLastName",
                EmailAddress = null
            };

            // Act
            var filter = userGraph.BuildFilter(azureFilter, out var multipleFiltersOn);

            // Assert
            Assert.Contains("startswith", filter);
            Assert.False(multipleFiltersOn);
        }

        [Fact]
        public void BuildFilter_NamesNull_FilterEmailNotNull_ReturnsEmailFilter_MultipleFiltersOff()
        {
            // Arrange
            var userGraph = CreateSut();

            var azureFilter = new AzureUserFilter
            {
                FirstName = null,
                LastName = null,
                EmailAddress = "email@space.com"
            };

            // Act
            var filter = userGraph.BuildFilter(azureFilter, out var multipleFiltersOn);

            // Assert
            Assert.Contains("IssuerAssignedId", filter);
            Assert.False(multipleFiltersOn);
        }

        [Fact]
        public void BuildFilter_NamesEmptyString_FilterEmailNotNull_ReturnsEmailFilter_MultipleFiltersOff()
        {
            // Arrange
            var userGraph = CreateSut();

            var azureFilter = new AzureUserFilter
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                EmailAddress = "email@space.com"
            };

            // Act
            var filter = userGraph.BuildFilter(azureFilter, out var multipleFiltersOn);

            // Assert
            Assert.Contains("IssuerAssignedId", filter);
            Assert.False(multipleFiltersOn);
        }

        [Fact]
        public void BuildFilter_NamesSet_EmailSet_ReturnsEmailFilter_MultipleFiltersOn()
        {
            // Arrange
            var userGraph = CreateSut();

            var azureFilter = new AzureUserFilter
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                EmailAddress = "email@space.com"
            };

            // Act
            var filter = userGraph.BuildFilter(azureFilter, out var multipleFiltersOn);

            // Assert
            Assert.Contains("IssuerAssignedId", filter);
            Assert.True(multipleFiltersOn);
        }

        private Authentication.ExternalServices.GraphAPI.UserGraph CreateSut()
        {
            var graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();

            return new Authentication.ExternalServices.GraphAPI.UserGraph(
                new GraphApiClientConfiguration(), graphServiceClientFactory);
        }
    }
}