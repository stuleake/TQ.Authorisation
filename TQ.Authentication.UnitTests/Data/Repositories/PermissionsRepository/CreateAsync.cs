using System;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using TQ.Authentication.Data.Entities;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.PermissionsRepository
{
    public class CreateAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.PermissionsRepository permissionsRepository;

        public CreateAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task CreateAsync_ReturnsGroup()
        {
            // Arrange
            permissionsRepository = CreateSut();

            var permissionId = Guid.NewGuid();

            var permission = new Permission { Id = permissionId };

            // Act
            var newPermissionId = await permissionsRepository.CreateAsync(permission);

            // Assert
            Assert.Equal(permissionId, newPermissionId);
        }

        private Authentication.Data.Repositories.PermissionsRepository CreateSut()
        {
            return new Authentication.Data.Repositories.PermissionsRepository(context);
        }
    }
}