using System;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using TQ.Authentication.Data.Entities;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.RolePermissionsRepository
{
    public class CreateAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.RolePermissionsRepository rolePermissionsRepository;

        public CreateAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task CreateAsync_ReturnsRolePermission()
        {
            // Arrange
            rolePermissionsRepository = CreateSut();

            var rolePermissionId = Guid.NewGuid();

            var rolePermission = new RolePermission { Id = rolePermissionId };

            // Act
            var newRolePermissionId = await rolePermissionsRepository.CreateAsync(rolePermission);

            // Assert
            Assert.Equal(rolePermissionId, newRolePermissionId);
        }

        private Authentication.Data.Repositories.RolePermissionsRepository CreateSut()
        {
            return new Authentication.Data.Repositories.RolePermissionsRepository(context);
        }
    }
}