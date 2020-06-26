using System;
using System.Linq;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.GroupsRepository
{
    public class GetAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.GroupsRepository groupsRepository;

        public GetAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task GetAsync_ReturnsGroup()
        {
            // Arrange
            groupsRepository = CreateSut();

            // Act
            var group = await groupsRepository.GetAsync(context.Groups.FirstOrDefault().Id);

            // Assert
            Assert.NotNull(group);
        }

        [Fact]
        public async Task GetAsync_GroupNotfound_ReturnsGroup()
        {
            // Arrange
            groupsRepository = CreateSut();

            // Act
            var group = await groupsRepository.GetAsync(Guid.NewGuid());

            // Assert
            Assert.Null(group);
        }

        private Authentication.Data.Repositories.GroupsRepository CreateSut()
        {
            return new Authentication.Data.Repositories.GroupsRepository(context);
        }
    }
}