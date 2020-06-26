using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.RolesRepository
{
    public class IsRoleNameUniquePerGroupIdAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.RolesRepository rolesRepository;

        public IsRoleNameUniquePerGroupIdAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task IsRoleNameUniquePerGroupIdAsync_ReturnsFalse()
        {
            // Arrange
            rolesRepository = CreateSut();

            var role = await context.Roles.FirstOrDefaultAsync();

            var roleNameUnique = await rolesRepository.IsRoleNameUniquePerGroupIdAsync(
                role.GroupId, role.Name);

            Assert.False(roleNameUnique);
        }

        [Fact]
        public async Task IsRoleNameUniquePerGroupIdAsync_ReturnsTrue()
        {
            // Arrange
            rolesRepository = CreateSut();

            var role = await context.Roles.FirstOrDefaultAsync();

            var roleNameUnique = await rolesRepository.IsRoleNameUniquePerGroupIdAsync(
                role.GroupId, "some non-existing name");

            Assert.True(roleNameUnique);
        }

        private Authentication.Data.Repositories.RolesRepository CreateSut()
        {
            return new Authentication.Data.Repositories.RolesRepository(context);
        }
    }
}