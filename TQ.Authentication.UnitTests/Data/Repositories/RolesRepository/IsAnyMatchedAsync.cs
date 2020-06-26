using System;
using System.Linq;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.RolesRepository
{
    public class IsAnyMatchedAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.RolesRepository rolesRepository;

        public IsAnyMatchedAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task IsAnyMatchedAsync_ReturnsTrue()
        {
            // Arrange
            rolesRepository = CreateSut();

            // Act
            var isMatched = await rolesRepository.IsAnyMatchedAsync(
                role => role.Id == context.Roles.FirstOrDefault().Id);

            // Assert
            Assert.True(isMatched);
        }

        [Fact]
        public async Task IsAnyMatchedAsync_ReturnsFalse()
        {
            // Arrange
            rolesRepository = CreateSut();

            // Act
            var isMatched = await rolesRepository.IsAnyMatchedAsync(
                role => role.Id == Guid.NewGuid());

            // Assert
            Assert.False(isMatched);
        }

        private Authentication.Data.Repositories.RolesRepository CreateSut()
        {
            return new Authentication.Data.Repositories.RolesRepository(context);
        }
    }
}