using System;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using TQ.Authentication.Data.Entities;
using Xunit;
using Repo = TQ.Authentication.Data.Repositories;

namespace TQ.Authentication.UnitTests.Data.Repositories.UserRolesRepository
{
    public class CreateAsync
    {
        private readonly ApplicationDbContext context;
        private Repo.UserRolesRepository userRolesRepository;

        public CreateAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task CreateAsync_ReturnsUserRole()
        {
            // Arrange
            userRolesRepository = CreateSut();
            var userRole = new UserRole { Id = Guid.NewGuid(), RoleId = Guid.NewGuid(), UserId = Guid.NewGuid() };
            var userRoleId = userRole.Id;

            // Act
            var newUserRoleId = await userRolesRepository.CreateAsync(userRole);
            var newUserRole = await userRolesRepository.GetAsync(userRoleId);

            // Assert
            Assert.Equal(userRoleId, newUserRoleId);
            Assert.Equal(newUserRoleId, newUserRole.Id);
        }

        private Repo.UserRolesRepository CreateSut()
        {
            return new Repo.UserRolesRepository(context);
        }
    }
}