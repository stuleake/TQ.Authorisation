using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using TQ.Authentication.Data.Entities;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.RolePermissionsRepository
{
    public class RemoveMultipleAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.RolePermissionsRepository rolePermissionsRepository;

        public RemoveMultipleAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task RemoveMultipleAsync_RemovesMultiple()
        {
            // Arrange
            rolePermissionsRepository = CreateSut();

            var rolePermissions = context.RolePermissions;

            // Act
            await rolePermissionsRepository.RemoveMultipleAsync(rolePermissions);

            // Assert
            Assert.Equal(0, await context.RolePermissions.CountAsync());
        }

        [Fact]
        public async Task RemoveMultipleAsync_EmptyListPassed()
        {
            // Arrange
            rolePermissionsRepository = CreateSut();

            var rolePermissions = new List<RolePermission>();

            var rolePermissionsCountBefore = await context.RolePermissions.CountAsync();

            // Act
            await rolePermissionsRepository.RemoveMultipleAsync(rolePermissions);

            var rolePermissionsCountAfter = await context.RolePermissions.CountAsync();

            // Assert
            Assert.Equal(rolePermissionsCountBefore, rolePermissionsCountAfter);
        }

        private Authentication.Data.Repositories.RolePermissionsRepository CreateSut()
        {
            return new Authentication.Data.Repositories.RolePermissionsRepository(context);
        }
    }
}