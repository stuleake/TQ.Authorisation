using System;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using TQ.Authentication.Data.Entities;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.RolesRepository
{
    public class CreateAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.RolesRepository rolesRepository;

        public CreateAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task CreateAsync_ReturnsRole()
        {
            // Arrange
            rolesRepository = CreateSut();

            var roleId = Guid.NewGuid();

            var role = new Role
            {
                Id = roleId,
                Name = "Role 1",
                Description = "Role 1 Description",
                GroupId = Guid.NewGuid()
            };

            // Act
            var newRoleId = await rolesRepository.CreateAsync(role);

            // Assert
            Assert.Equal(roleId, newRoleId);
        }

        private Authentication.Data.Repositories.RolesRepository CreateSut()
        {
            return new Authentication.Data.Repositories.RolesRepository(context);
        }
    }
}