using System;
using TQ.Authentication.ExternalServices.GraphAPI.Models;
using Xunit;

namespace TQ.Authentication.UnitTests.ExternalServices.GraphApiClient.Models
{
    public class AzureGroupTests
    {
        private DateTime createdAt = DateTime.Parse("2020-04-03");

        [Fact]
        public void GetAzureGroup_ReturnsAzureGroup()
        {
            var azureGroup = GeturnsAzureGroup();

            Assert.Equal("Group name", azureGroup.Name);
            Assert.Equal("Description", azureGroup.Description);
            Assert.Equal(createdAt, azureGroup.CreatedAt);
        }

        [Fact]
        public AzureGroup GeturnsAzureGroup()
        {
            return new AzureGroup
            {
                Id = Guid.NewGuid(),
                Name = "Group name",
                Description = "Description",
                CreatedAt = createdAt
            };
        }
    }
}