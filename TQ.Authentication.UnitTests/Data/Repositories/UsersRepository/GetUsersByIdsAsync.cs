using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.UsersRepository
{
    public class GetUsersByIdsAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.UsersRepository usersRepository;

        public GetUsersByIdsAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task CreateAsync_ReturnsUsers()
        {
            // Arrange
            usersRepository = CreateSut();

            var userIds = context.Users.Take(2).Select(x => x.Id);

            // Act
            var users = await usersRepository.GetUsersByIdsAsync(userIds);

            // Assert
            Assert.Equal(2, users.Count());
        }

        [Fact]
        public async Task CreateAsync_NoUsersFound_ReturnsEmptyCollection()
        {
            // Arrange
            usersRepository = CreateSut();

            var userIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            // Act
            var users = await usersRepository.GetUsersByIdsAsync(userIds);

            // Assert
            Assert.Empty(users);
        }

        private Authentication.Data.Repositories.UsersRepository CreateSut()
        {
            return new Authentication.Data.Repositories.UsersRepository(context);
        }
    }
}