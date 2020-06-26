using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.UserRoleRepository
{
    public class IsUserAlreadyAssignedToRole
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.UserRolesRepository userRolesRepository;

        public IsUserAlreadyAssignedToRole()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task IsUserAlreadyAssignedToRoleAsync_ReturnsFalse()
        {
            // Arrange
            userRolesRepository = CreateSut();
            var roleId = Guid.NewGuid();
            var model = new List<Guid>();

            var userRoleAlreadyAssigned = await userRolesRepository.IsUserAlreadyAssignedToRoleAsync(
                roleId, model);

            Assert.Null(userRoleAlreadyAssigned);
        }

        private Authentication.Data.Repositories.UserRolesRepository CreateSut()
        {
            return new Authentication.Data.Repositories.UserRolesRepository(context);
        }
    }
}