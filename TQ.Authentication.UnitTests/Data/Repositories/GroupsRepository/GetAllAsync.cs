using System.Linq;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.GroupsRepository
{
    public class GetAllAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.GroupsRepository groupsRepository;

        public GetAllAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsGroup()
        {
            // Arrange
            groupsRepository = CreateSut();

            // Act
            var groups = await groupsRepository.GetAllAsync();

            // Assert
            Assert.Equal(4, groups.Count());
        }

        [Fact]
        public async Task GetAllAsync_NoGroups_ReturnsEmptyCollection()
        {
            // Arrange
            groupsRepository = CreateSut();

            // Empty all groups
            context.Groups.RemoveRange(context.Groups);
            context.SaveChanges();

            // Act
            var groups = await groupsRepository.GetAllAsync();

            // Assert
            Assert.Empty(groups);
        }

        private Authentication.Data.Repositories.GroupsRepository CreateSut()
        {
            return new Authentication.Data.Repositories.GroupsRepository(context);
        }
    }
}