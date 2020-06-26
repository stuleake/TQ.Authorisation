using System;
using System.Linq;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.PermissionsRepository
{
    public class IsAnyMatchedAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.PermissionsRepository permissionsRepository;

        public IsAnyMatchedAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task IsAnyMatchedAsync_ReturnsTrue()
        {
            // Arrange
            permissionsRepository = CreateSut();

            // Act
            var isMatched = await permissionsRepository.IsAnyMatchedAsync(
                permission => permission.Id == context.Permissions.FirstOrDefault().Id);

            // Assert
            Assert.True(isMatched);
        }

        [Fact]
        public async Task IsAnyMatchedAsync_ReturnsFalse()
        {
            // Arrange
            permissionsRepository = CreateSut();

            // Act
            var isMatched = await permissionsRepository.IsAnyMatchedAsync(
                permission => permission.Id == Guid.NewGuid());

            // Assert
            Assert.False(isMatched);
        }

        private Authentication.Data.Repositories.PermissionsRepository CreateSut()
        {
            return new Authentication.Data.Repositories.PermissionsRepository(context);
        }
    }
}