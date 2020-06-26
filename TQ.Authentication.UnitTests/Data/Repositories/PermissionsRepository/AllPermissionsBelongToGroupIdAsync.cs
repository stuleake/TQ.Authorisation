using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.PermissionsRepository
{
    public class AllPermissionsBelongToGroupIdAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.PermissionsRepository permissionsRepository;

        public AllPermissionsBelongToGroupIdAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task AllPermissionsBelongToGroupIdAsync_ReturnsTrue()
        {
            // Arrange
            permissionsRepository = CreateSut();

            var permissions = await context.Permissions.ToListAsync();
            var permission = permissions.FirstOrDefault();
            var groupId = permission.GroupId;

            // Act
            var result = await permissionsRepository
                .AllPermissionsBelongToGroupIdAsync(groupId, permissions.Select(permission => permission.Id));

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task AllPermissionsBelongToGroupIdAsync_ReturnsFalse()
        {
            // Arrange
            permissionsRepository = CreateSut();

            var permissions = await context.Permissions.ToListAsync();
            var permission = permissions.FirstOrDefault();
            var groupId = permission.GroupId;

            // Change groupId
            permission.GroupId = Guid.NewGuid();
            await context.SaveChangesAsync();

            // Act
            var result = await permissionsRepository
                .AllPermissionsBelongToGroupIdAsync(groupId, permissions.Select(permission => permission.Id));

            // Assert
            Assert.False(result);
        }

        private Authentication.Data.Repositories.PermissionsRepository CreateSut()
        {
            return new Authentication.Data.Repositories.PermissionsRepository(context);
        }
    }
}