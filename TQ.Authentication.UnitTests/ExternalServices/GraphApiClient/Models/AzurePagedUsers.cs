using System;
using System.Collections.Generic;
using System.Linq;
using TQ.Authentication.ExternalServices.GraphAPI.Models;
using Xunit;

namespace TQ.Authentication.UnitTests.ExternalServices.GraphApiClient.Models
{
    public class AzurePagedUsers
    {
        [Fact]
        public void TestsAzurePagedUsers()
        {
            var azurePagedUsers = GetAzurePagedUsers();

            Assert.Equal("sometoken", azurePagedUsers.NextPageToken);
            Assert.True(azurePagedUsers.Users.FirstOrDefault().Id != Guid.Empty);
        }

        public Authentication.ExternalServices.GraphAPI.Models.AzurePagedUsers GetAzurePagedUsers()
        {
            return new Authentication.ExternalServices.GraphAPI.Models.AzurePagedUsers
            {
                NextPageToken = "sometoken",
                Users = new List<AzureUser>
                {
                    new AzureUser
                    {
                        Id = Guid.NewGuid()
                    }
                }
            };
        }
    }
}