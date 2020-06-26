using System;
using System.Linq;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.GroupsRepository
{
    public class IsAnyMatchedAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.GroupsRepository groupsRepository;

        public IsAnyMatchedAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task IsAnyMatchedAsync_ReturnsTrue()
        {
            // Arrange
            groupsRepository = CreateSut();

            // Act
            var isMatched = await groupsRepository.IsAnyMatchedAsync(
                group => group.Id == context.Groups.FirstOrDefault().Id);

            // Assert
            Assert.True(isMatched);
        }

        [Fact]
        public async Task IsAnyMatchedAsync_ReturnsFalse()
        {
            // Arrange
            groupsRepository = CreateSut();

            // Act
            var isMatched = await groupsRepository.IsAnyMatchedAsync(group => group.Id == Guid.NewGuid());

            // Assert
            Assert.False(isMatched);
        }

        private Authentication.Data.Repositories.GroupsRepository CreateSut()
        {
            return new Authentication.Data.Repositories.GroupsRepository(context);
        }
    }
}