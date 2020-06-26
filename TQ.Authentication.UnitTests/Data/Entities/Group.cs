using System;
using System.Collections.Generic;
using TQ.Authentication.Data.Entities;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Entities
{
    public class Group
    {
        [Fact]
        public void Group_Test()
        {
            var group = new Authentication.Data.Entities.Group
            {
                Id = Guid.NewGuid(),
                ServiceUrl = "https://localhost",
                IsActive = false,
                Permissions = new List<Permission>(),
                UserGroups = new List<Authentication.Data.Entities.UserGroup>()
            };

            Assert.NotNull(group);
            Assert.NotNull(group.Permissions);
            Assert.NotNull(group.UserGroups);
        }
    }
}