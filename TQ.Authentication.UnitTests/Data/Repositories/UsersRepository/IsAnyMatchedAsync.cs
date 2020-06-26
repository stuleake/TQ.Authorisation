using System;
using System.Linq;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.UsersRepository
{
    public class IsAnyMatchedAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.UsersRepository usersRepository;

        public IsAnyMatchedAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task IsAnyMatchedAsync_ReturnsTrue()
        {
            // Arrange
            usersRepository = CreateSut();

            // Act
            var isMatched = await usersRepository.IsAnyMatchedAsync(
                user => user.Id == context.Users.FirstOrDefault().Id);

            // Assert
            Assert.True(isMatched);
        }

        [Fact]
        public async Task IsAnyMatchedAsync_ReturnsFalse()
        {
            // Arrange
            usersRepository = CreateSut();

            // Act
            var isMatched = await usersRepository.IsAnyMatchedAsync(user => user.Id == Guid.NewGuid());

            // Assert
            Assert.False(isMatched);
        }

        private Authentication.Data.Repositories.UsersRepository CreateSut()
        {
            return new Authentication.Data.Repositories.UsersRepository(context);
        }
    }
}