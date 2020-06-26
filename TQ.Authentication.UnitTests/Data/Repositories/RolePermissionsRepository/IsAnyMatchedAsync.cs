using System;
using System.Linq;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.RolePermissionsRepository
{
    public class IsAnyMatchedAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.RolePermissionsRepository rolePermissionsRepository;

        public IsAnyMatchedAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task IsAnyMatchedAsync_ReturnsTrue()
        {
            // Arrange
            rolePermissionsRepository = CreateSut();

            // Act
            var isMatched = await rolePermissionsRepository.IsAnyMatchedAsync(
                rolePermission => rolePermission.Id == context.RolePermissions.FirstOrDefault().Id);

            // Assert
            Assert.True(isMatched);
        }

        [Fact]
        public async Task IsAnyMatchedAsync_ReturnsFalse()
        {
            // Arrange
            rolePermissionsRepository = CreateSut();

            // Act
            var isMatched = await rolePermissionsRepository.IsAnyMatchedAsync(
                rolePermission => rolePermission.Id == Guid.NewGuid());

            // Assert
            Assert.False(isMatched);
        }

        private Authentication.Data.Repositories.RolePermissionsRepository CreateSut()
        {
            return new Authentication.Data.Repositories.RolePermissionsRepository(context);
        }
    }
}