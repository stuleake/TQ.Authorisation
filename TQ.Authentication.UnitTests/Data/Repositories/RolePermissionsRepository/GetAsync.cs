using System;
using System.Linq;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.RolePermissionsRepository
{
    public class GetAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.RolePermissionsRepository rolePermissionsRepository;

        public GetAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task GetAsync_ReturnsRolePermission()
        {
            // Arrange
            rolePermissionsRepository = CreateSut();

            // Act
            var rolePermission = await rolePermissionsRepository.GetAsync(
                context.RolePermissions.FirstOrDefault().Id);

            // Assert
            Assert.NotNull(rolePermission);
        }

        [Fact]
        public async Task GetAsync_RolePermissionNotfound_ReturnsNull()
        {
            // Arrange
            rolePermissionsRepository = CreateSut();

            // Act
            var rolePermission = await rolePermissionsRepository.GetAsync(Guid.NewGuid());

            // Assert
            Assert.Null(rolePermission);
        }

        private Authentication.Data.Repositories.RolePermissionsRepository CreateSut()
        {
            return new Authentication.Data.Repositories.RolePermissionsRepository(context);
        }
    }
}