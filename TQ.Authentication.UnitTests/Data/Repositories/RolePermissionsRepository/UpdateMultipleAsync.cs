using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using TQ.Authentication.Data.Entities;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.RolePermissionsRepository
{
    public class UpdateMultipleAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.RolePermissionsRepository rolePermissionsRepository;

        public UpdateMultipleAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task UpdateMultipleAsync_EmptyListPassed()
        {
            // Arrange
            var rolePermissions = context.RolePermissions;
            var rolePermissionsCount = await rolePermissions.CountAsync();

            var rolePermission = await rolePermissions.FirstOrDefaultAsync();
            var roleId = rolePermission.RoleId;

            rolePermissionsRepository = CreateSut();

            var updatedRolePermissionsList = new List<RolePermission>();

            // Act
            await rolePermissionsRepository.UpdateMultipleAsync(roleId, updatedRolePermissionsList);

            // Assert
            Assert.NotEqual(rolePermissionsCount, await context.RolePermissions.CountAsync());
        }

        [Fact]
        public async Task UpdateMultipleAsync_RemovesMultiple()
        {
            // Arrange
            var rolePermissions = context.RolePermissions;
            var rolePermissionsCountBefore = await rolePermissions.CountAsync();

            var rolePermission = await rolePermissions.FirstOrDefaultAsync();
            var roleId = rolePermission.RoleId;

            rolePermissionsRepository = CreateSut();

            var updatedRolePermissionsList = new List<RolePermission>
            {
                new RolePermission { Id = Guid.NewGuid(), RoleId = Guid.NewGuid(), PermissionId = Guid.NewGuid() },
                new RolePermission { Id = Guid.NewGuid(), RoleId = Guid.NewGuid(), PermissionId = Guid.NewGuid() }
            };

            // Act
            await rolePermissionsRepository.UpdateMultipleAsync(roleId, updatedRolePermissionsList);

            var rolePermissionsCountAfter = await rolePermissions.CountAsync();

            // Assert
            Assert.Equal(rolePermissionsCountBefore - 2, rolePermissionsCountAfter);
        }

        private Authentication.Data.Repositories.RolePermissionsRepository CreateSut()
        {
            return new Authentication.Data.Repositories.RolePermissionsRepository(context);
        }
    }
}