using System;
using System.Linq;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.UserGroupsRepository
{
    public class GetUserGroupIdAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.UserGroupsRepository userGroupsRepository;

        public GetUserGroupIdAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task GetUserGroupIdAsync_ReturnsId()
        {
            // Arrange
            userGroupsRepository = CreateSut();

            // Act
            var id = await userGroupsRepository.GetUserGroupIdAsync(
                context.UserGroups.FirstOrDefault().GroupId,
                context.UserGroups.FirstOrDefault().UserId);

            // Assert
            Assert.NotNull(id);
        }

        [Fact]
        public async Task GetUserGroupIdAsync_ReturnsNull()
        {
            // Arrange
            var usersGroupsRepository = new Authentication.Data.Repositories.UserGroupsRepository(context);

            // Act
            var id = await usersGroupsRepository.GetUserGroupIdAsync(
                Guid.NewGuid(),
                Guid.NewGuid());

            // Assert
            Assert.Null(id);
        }

        private Authentication.Data.Repositories.UserGroupsRepository CreateSut()
        {
            return new Authentication.Data.Repositories.UserGroupsRepository(context);
        }
    }
}