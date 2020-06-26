using TQ.Authentication.ExternalServices.GraphAPI.Models;
using Xunit;

namespace TQ.Authentication.UnitTests.ExternalServices.GraphApiClient.Models
{
    public class AzureUserTests
    {
        [Fact]
        public void GetAzureUser_TestsAzureUser()
        {
            var azureUser = GetAzureUser();

            Assert.Equal("Display name", azureUser.DisplayName);
            Assert.Equal("email@address.com", azureUser.EmailAddress);
            Assert.Equal("pwd", azureUser.Password);
        }

        public AzureUser GetAzureUser()
        {
            return new AzureUser
            {
                DisplayName = "Display name",
                EmailAddress = "email@address.com",
                Password = "pwd"
            };
        }
    }
}