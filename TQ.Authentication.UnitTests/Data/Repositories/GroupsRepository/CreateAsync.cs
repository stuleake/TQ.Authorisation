using System;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using TQ.Authentication.Data.Entities;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.GroupsRepository
{
    public class CreateAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.GroupsRepository groupsRepository;

        public CreateAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task CreateAsync_ReturnsGroup()
        {
            // Arrange
            groupsRepository = CreateSut();

            var groupId = Guid.NewGuid();

            var group = new Group { Id = groupId, ServiceUrl = "https://localhost", IsActive = true };

            // Act
            var newGroupId = await groupsRepository.CreateAsync(group);

            // Assert
            Assert.Equal(groupId, newGroupId);
        }

        private Authentication.Data.Repositories.GroupsRepository CreateSut()
        {
            return new Authentication.Data.Repositories.GroupsRepository(context);
        }
    }
}