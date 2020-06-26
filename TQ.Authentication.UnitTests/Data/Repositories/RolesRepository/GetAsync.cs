using System;
using System.Linq;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.RolesRepository
{
    public class GetAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.RolesRepository rolesRepository;

        public GetAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task GetAsync_ReturnsRole()
        {
            // Arrange
            rolesRepository = CreateSut();

            // Act
            var role = await rolesRepository.GetAsync(context.Roles.FirstOrDefault().Id);

            // Assert
            Assert.NotNull(role);
        }

        [Fact]
        public async Task GetAsync_PermissionNotfound_ReturnsPermission()
        {
            // Arrange
            rolesRepository = CreateSut();

            // Act
            var role = await rolesRepository.GetAsync(Guid.NewGuid());

            // Assert
            Assert.Null(role);
        }

        private Authentication.Data.Repositories.RolesRepository CreateSut()
        {
            return new Authentication.Data.Repositories.RolesRepository(context);
        }
    }
}