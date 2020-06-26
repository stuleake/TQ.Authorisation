using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using TQ.Authentication.Data.Entities;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.RolePermissionsRepository
{
    public class CreateMultipleAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.RolePermissionsRepository rolePermissionsRepository;

        public CreateMultipleAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task CreateMultipleAsync_CreatesMultiple()
        {
            // Arrange
            rolePermissionsRepository = CreateSut();

            var rolePermissionId1 = Guid.NewGuid();
            var rolePermissionId2 = Guid.NewGuid();

            var rolePermissions = new List<RolePermission>
            {
                new RolePermission { Id = rolePermissionId1, PermissionId = Guid.NewGuid(), RoleId = Guid.NewGuid() },
                new RolePermission { Id = rolePermissionId2, PermissionId = Guid.NewGuid(), RoleId = Guid.NewGuid() }
            };

            // Act
            await rolePermissionsRepository.CreateMultipleAsync(rolePermissions);

            var newPermissionRole1 = await context.RolePermissions.FindAsync(rolePermissionId1);
            var newPermissionRole2 = await context.RolePermissions.FindAsync(rolePermissionId2);

            // Assert
            Assert.Equal(rolePermissionId1, newPermissionRole1.Id);
            Assert.Equal(rolePermissionId2, newPermissionRole2.Id);
        }

        [Fact]
        public async Task CreateMultipleAsync_EmptyListPassed()
        {
            // Arrange
            rolePermissionsRepository = CreateSut();

            var rolePermissions = new List<RolePermission>();

            var rolePermissionsCountBefore = await context.RolePermissions.CountAsync();

            // Act
            await rolePermissionsRepository.CreateMultipleAsync(rolePermissions);

            var rolePermissionsCountAfter = await context.RolePermissions.CountAsync();

            // Assert
            Assert.Equal(rolePermissionsCountBefore, rolePermissionsCountAfter);
        }

        private Authentication.Data.Repositories.RolePermissionsRepository CreateSut()
        {
            return new Authentication.Data.Repositories.RolePermissionsRepository(context);
        }
    }
}