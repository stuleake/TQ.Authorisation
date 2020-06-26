using System;
using TQ.Authentication.Data.Entities;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Entities
{
    public class RolePermission
    {
        [Fact]
        public void RolePermission_Test()
        {
            var rolePermission = new Authentication.Data.Entities.RolePermission
            {
                Id = Guid.NewGuid(),
                RoleId = Guid.NewGuid(),
                PermissionId = Guid.NewGuid(),
                Role = new Role(),
                Permission = new Permission()
            };

            Assert.NotNull(rolePermission);
            Assert.NotEqual(rolePermission.PermissionId, Guid.Empty);
            Assert.NotNull(rolePermission.Role);
            Assert.NotNull(rolePermission.Permission);
        }
    }
}