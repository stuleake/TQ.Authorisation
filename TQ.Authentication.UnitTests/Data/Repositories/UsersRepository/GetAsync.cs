using System;
using System.Linq;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.UsersRepository
{
    public class GetAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.UsersRepository usersRepository;

        public GetAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task GetAsync_ReturnsUser()
        {
            // Arrange
            usersRepository = CreateSut();

            // Act
            var user = await usersRepository.GetAsync(context.Users.FirstOrDefault().Id);

            // Assert
            Assert.NotNull(user);
        }

        [Fact]
        public async Task GetAsync_UserNotfound_ReturnsNull()
        {
            // Arrange
            usersRepository = CreateSut();

            // Act
            var user = await usersRepository.GetAsync(Guid.NewGuid());

            // Assert
            Assert.Null(user);
        }

        private Authentication.Data.Repositories.UsersRepository CreateSut()
        {
            return new Authentication.Data.Repositories.UsersRepository(context);
        }
    }
}