using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.RolesRepository
{
    public class GroupAndRoleCombinationExistsAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.RolesRepository rolesRepository;

        public GroupAndRoleCombinationExistsAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task GroupAndRoleCombinationExistsAsync_ReturnsTrue()
        {
            // Arrange
            rolesRepository = CreateSut();

            var role = await context.Roles.FirstOrDefaultAsync();

            var isGroupAndRoleCorrect = await rolesRepository.GroupAndRoleCombinationExistsAsync(
                role.GroupId, role.Id);

            Assert.True(isGroupAndRoleCorrect);
        }

        [Fact]
        public async Task GroupAndRoleCombinationExistsAsync_ReturnsFalse()
        {
            // Arrange
            rolesRepository = CreateSut();

            var role = await context.Roles.FirstOrDefaultAsync();

            var isGroupAndRoleCorrect = await rolesRepository.GroupAndRoleCombinationExistsAsync(
                Guid.NewGuid(), role.Id);

            Assert.False(isGroupAndRoleCorrect);
        }

        private Authentication.Data.Repositories.RolesRepository CreateSut()
        {
            return new Authentication.Data.Repositories.RolesRepository(context);
        }
    }
}