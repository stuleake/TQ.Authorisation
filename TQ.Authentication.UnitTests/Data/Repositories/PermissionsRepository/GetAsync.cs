using System;
using System.Linq;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.PermissionsRepository
{
    public class GetAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.PermissionsRepository permissionsRepository;

        public GetAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task GetAsync_ReturnsPermission()
        {
            // Arrange
            permissionsRepository = CreateSut();

            // Act
            var permission = await permissionsRepository.GetAsync(context.Permissions.FirstOrDefault().Id);

            // Assert
            Assert.NotNull(permission);
        }

        [Fact]
        public async Task GetAsync_PermissionNotfound_ReturnsPermission()
        {
            // Arrange
            permissionsRepository = CreateSut();

            // Act
            var permission = await permissionsRepository.GetAsync(Guid.NewGuid());

            // Assert
            Assert.Null(permission);
        }

        private Authentication.Data.Repositories.PermissionsRepository CreateSut()
        {
            return new Authentication.Data.Repositories.PermissionsRepository(context);
        }
    }
}