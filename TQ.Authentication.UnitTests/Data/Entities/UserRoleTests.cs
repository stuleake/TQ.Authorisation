using System;
using Xunit;
using TQ.Authentication.Data.Entities;

namespace TQ.Authentication.UnitTests.Data.Entities
{
    public class UserRoleTests
    {
        [Fact]
        public void UserRole_Test()
        {
            var user = new UserRole
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                RoleId = Guid.NewGuid(),
                User = new Authentication.Data.Entities.User(),
                Role = new Role()
            };

            Assert.NotNull(user);
            Assert.NotNull(user.User);
            Assert.NotNull(user.Role);
        }
    }
}