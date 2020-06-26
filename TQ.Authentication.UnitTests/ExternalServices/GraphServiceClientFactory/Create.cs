using System;
using TQ.Authentication.ExternalServices.GraphAPI;
using Xunit;

namespace TQ.Authentication.UnitTests.ExternalServices.GraphServiceClientFactory
{
    public class Create
    {
        [Fact]
        public void Create_ConfigurationIsNull_ThrowsException()
        {
            // Arrange
            GraphApiClientConfiguration configuration = null;

            var factory = new Authentication.ExternalServices.GraphAPI.GraphServiceClientFactory(configuration);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => factory.Create());
        }
    }
}