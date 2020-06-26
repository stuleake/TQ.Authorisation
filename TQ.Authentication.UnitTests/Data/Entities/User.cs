using System;
using System.Collections.Generic;
using TQ.Authentication.Data.Entities;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Entities
{
    public class User
    {
        [Fact]
        public void User_Test()
        {
            var user = new Authentication.Data.Entities.User
            {
                Id = Guid.NewGuid(),
                UserRoles = new List<Authentication.Data.Entities.UserRole>(),
                UserGroups = new List<Authentication.Data.Entities.UserGroup>()
            };

            Assert.NotNull(user);
            Assert.NotNull(user.UserRoles);
            Assert.NotNull(user.UserGroups);
        }
    }
}