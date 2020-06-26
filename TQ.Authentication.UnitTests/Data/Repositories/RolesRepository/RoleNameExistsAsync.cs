using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.RolesRepository
{
    public class RoleNameExistsAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.RolesRepository rolesRepository;

        public RoleNameExistsAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task RoleNameExists_ReturnsTrue()
        {
            // Arrange
            rolesRepository = CreateSut();

            var role = await context.Roles.FirstOrDefaultAsync();

            var roleExist = await rolesRepository.RoleNameExistsAsync(role.Id, role.Name);

            Assert.True(roleExist);
        }

        [Fact]
        public async Task RoleNameExists_ReturnsFalse()
        {
            // Arrange
            rolesRepository = CreateSut();

            var role = await context.Roles.FirstOrDefaultAsync();

            var roleExist = await rolesRepository.RoleNameExistsAsync(role.Id, "some non-existing name");

            Assert.False(roleExist);
        }

        private Authentication.Data.Repositories.RolesRepository CreateSut()
        {
            return new Authentication.Data.Repositories.RolesRepository(context);
        }
    }
}