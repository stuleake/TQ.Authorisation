using System;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using TQ.Authentication.Data.Entities;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.UsersRepository
{
    public class CreateAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.UsersRepository usersRepository;

        public CreateAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task CreateAsync_ReturnsGroup()
        {
            // Arrange
            usersRepository = CreateSut();

            var userId = Guid.NewGuid();

            var user = new User { Id = userId };

            // Act
            var newUserId = await usersRepository.CreateAsync(user);
            var newUser = await usersRepository.GetAsync(newUserId);

            // Assert
            Assert.Equal(userId, newUserId);
            Assert.Equal(newUserId, newUser.Id);
        }

        private Authentication.Data.Repositories.UsersRepository CreateSut()
        {
            return new Authentication.Data.Repositories.UsersRepository(context);
        }
    }
}