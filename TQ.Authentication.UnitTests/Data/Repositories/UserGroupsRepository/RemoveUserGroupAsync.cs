using System;
using System.Linq;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.UserGroupsRepository
{
    public class RemoveUserGroupAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.UserGroupsRepository userGroupsRepository;

        public RemoveUserGroupAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task RemoveUserGroupAsync_RemovesUserGroup()
        {
            // Arrange
            userGroupsRepository = CreateSut();

            // Act
            var userGroupsCountBefore = context.UserGroups.Count();

            await userGroupsRepository.RemoveUserGroupAsync(
                context.UserGroups.FirstOrDefault().GroupId,
                context.UserGroups.FirstOrDefault().UserId);

            // Assert
            Assert.Equal(userGroupsCountBefore - 1, context.UserGroups.Count());
        }

        [Fact]
        public async Task RemoveUserGroupAsync_ShouldNotRemoveUserGroup()
        {
            // Arrange
            userGroupsRepository = CreateSut();

            // Act
            var userGroupsCountBefore = context.UserGroups.Count();

            await userGroupsRepository.RemoveUserGroupAsync(
                Guid.NewGuid(),
                Guid.NewGuid());

            // Assert
            Assert.Equal(userGroupsCountBefore, context.UserGroups.Count());
        }

        private Authentication.Data.Repositories.UserGroupsRepository CreateSut()
        {
            return new Authentication.Data.Repositories.UserGroupsRepository(context);
        }
    }
}