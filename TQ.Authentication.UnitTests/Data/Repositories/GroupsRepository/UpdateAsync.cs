using System.Linq;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.GroupsRepository
{
    public class UpdateAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.GroupsRepository groupsRepository;

        public UpdateAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task UpdateAsync_ReturnsGroup()
        {
            // Arrange
            groupsRepository = CreateSut();

            var group = context.Groups.FirstOrDefault();

            group.ServiceUrl = "https://google.com";

            // Act
            await groupsRepository.UpdateGroupAsync(group);
            var updatedGroup = await groupsRepository.GetAsync(group.Id);

            // Assert
            Assert.Equal("https://google.com", updatedGroup.ServiceUrl);
        }

        private Authentication.Data.Repositories.GroupsRepository CreateSut()
        {
            return new Authentication.Data.Repositories.GroupsRepository(context);
        }
    }
}