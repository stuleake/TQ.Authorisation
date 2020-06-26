using System;
using System.Linq;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.UserRoleRepository
{
    public class IsAnyMatchedAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.UserRolesRepository userRolesRepository;

        public IsAnyMatchedAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task IsAnyMatchedAsync_ReturnsTrue()
        {
            // Arrange
            userRolesRepository = CreateSut();

            // Act
            var isMatched = await userRolesRepository.IsAnyMatchedAsync(
                userRole => userRole.Id == context.UserRoles.FirstOrDefault().Id);

            // Assert
            Assert.True(isMatched);
        }

        [Fact]
        public async Task IsAnyMatchedAsync_ReturnsFalse()
        {
            // Arrange
            userRolesRepository = CreateSut();

            // Act
            var isMatched = await userRolesRepository.IsAnyMatchedAsync(userRole => userRole.Id == Guid.NewGuid());

            // Assert
            Assert.False(isMatched);
        }

        private Authentication.Data.Repositories.UserRolesRepository CreateSut()
        {
            return new Authentication.Data.Repositories.UserRolesRepository(context);
        }
    }
}