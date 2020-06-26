using System;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Entities
{
    public class UserGroup
    {
        [Fact]
        public void UserGroup_Test()
        {
            var user = new Authentication.Data.Entities.UserGroup
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                GroupId = Guid.NewGuid(),
                User = new Authentication.Data.Entities.User(),
                Group = new Authentication.Data.Entities.Group()
            };

            Assert.NotNull(user);
            Assert.NotNull(user.User);
            Assert.NotNull(user.Group);
        }
    }
}