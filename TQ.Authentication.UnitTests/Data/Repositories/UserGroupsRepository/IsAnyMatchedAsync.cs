using System;
using System.Linq;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.UserGroupsRepository
{
    public class IsAnyMatchedAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.UserGroupsRepository userGroupsRepository;

        public IsAnyMatchedAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task IsAnyMatchedAsync_ReturnsTrue()
        {
            // Arrange
            userGroupsRepository = CreateSut();

            // Act
            var isMatched = await userGroupsRepository.IsAnyMatchedAsync(
                userGroup => userGroup.Id == context.UserGroups.FirstOrDefault().Id);

            // Assert
            Assert.True(isMatched);
        }

        [Fact]
        public async Task IsAnyMatchedAsync_ReturnsFalse()
        {
            // Arrange
            userGroupsRepository = CreateSut();

            // Act
            var isMatched = await userGroupsRepository.IsAnyMatchedAsync(
                userGroup => userGroup.Id == Guid.NewGuid());

            // Assert
            Assert.False(isMatched);
        }

        private Authentication.Data.Repositories.UserGroupsRepository CreateSut()
        {
            return new Authentication.Data.Repositories.UserGroupsRepository(context);
        }
    }
}