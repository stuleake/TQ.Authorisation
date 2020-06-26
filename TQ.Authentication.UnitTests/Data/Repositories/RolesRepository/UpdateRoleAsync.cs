using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.RolesRepository
{
    public class UpdateRoleAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.RolesRepository rolesRepository;

        public UpdateRoleAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task UpdateRoleAsync_UpdatesRole()
        {
            // Arrange
            rolesRepository = CreateSut();

            var name = "Name 1";
            var description = "Description 1";

            var role = await context.Roles.FirstOrDefaultAsync();
            role.Name = name;
            role.Description = description;

            // Act
            await rolesRepository.UpdateRoleAsync(role);
            var updatedRole = await context.Roles.FindAsync(role.Id);

            // Assert
            Assert.Equal(name, updatedRole.Name);
            Assert.Equal(description, updatedRole.Description);
        }

        private Authentication.Data.Repositories.RolesRepository CreateSut()
        {
            return new Authentication.Data.Repositories.RolesRepository(context);
        }
    }
}