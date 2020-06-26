using System;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using Xunit;
using Repo = TQ.Authentication.Data.Repositories;

namespace TQ.Authentication.UnitTests.Data.Repositories.UserRolesRepository
{
    public class GetAsync
    {
        private readonly ApplicationDbContext context;
        private Repo.UserRolesRepository userRolesRepository;

        public GetAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task GetAsync_ReturnsUserRole()
        {
            // Arrange
            userRolesRepository = CreateSut();
            var user = context.UserRoles.Add(new Authentication.Data.Entities.UserRole { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), RoleId = Guid.NewGuid() });

            // Act
            var userRole = await userRolesRepository.GetAsync(user.Entity.Id);

            // Assert
            Assert.NotNull(userRole);
        }

        [Fact]
        public async Task GetAsync_UserRoleNotfound_ReturnsNull()
        {
            // Arrange
            userRolesRepository = CreateSut();

            // Act
            var userRole = await userRolesRepository.GetAsync(Guid.NewGuid());

            // Assert
            Assert.Null(userRole);
        }

        private Repo.UserRolesRepository CreateSut()
        {
            return new Repo.UserRolesRepository(context);
        }
    }
}