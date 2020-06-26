using System;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using TQ.Authentication.Data.Entities;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.UserGroupsRepository
{
    public class CreateAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.UserGroupsRepository userGroupsRepository;

        public CreateAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task CreateAsync_ReturnsUserGroupId()
        {
            // Arrange
            userGroupsRepository = CreateSut();

            var userId = Guid.NewGuid();
            var groupId = Guid.NewGuid();

            var userGroup = new UserGroup { Id = Guid.NewGuid(), GroupId = groupId, UserId = userId };

            // Act
            var newUserGroupId = await userGroupsRepository.CreateAsync(userGroup);
            var newUserGroup = await userGroupsRepository.GetAsync(newUserGroupId);

            // Assert
            Assert.Equal(groupId, newUserGroup.GroupId);
            Assert.Equal(userId, newUserGroup.UserId);
        }

        private Authentication.Data.Repositories.UserGroupsRepository CreateSut()
        {
            return new Authentication.Data.Repositories.UserGroupsRepository(context);
        }
    }
}