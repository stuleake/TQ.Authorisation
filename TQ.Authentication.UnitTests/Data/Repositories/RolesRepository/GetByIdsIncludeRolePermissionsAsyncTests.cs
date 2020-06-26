using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.Data.Repositories;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.RolesRepository
{
    public class GetByIdsIncludeRolePermissionsAsyncTests
    {
        private readonly ApplicationDbContext context;

        public GetByIdsIncludeRolePermissionsAsyncTests()
        {
            context = new InMemoryDbContextFactory(false).GetApplicationDbContext();
        }

        [Fact]
        public async Task GetByIdsIncludeRolePermissionsAyncReturnsNullWhenRoleNotFound()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetByIdIncludeRolePermissionsAync(Guid.NewGuid());

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdsIncludeRolePermissionsAyncReturnsRoles()
        {
            // Arrange
            var roleIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            foreach (Guid roleId in roleIds)
            {
                var role = new Role { Id = roleId };
                this.context.Roles.Add(role);
            }

            this.context.SaveChanges();

            var sut = this.CreateSut();

            // Act
            var result = await sut.GetByIdsIncludeRolePermissionsAsync(roleIds);

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
        }

        private IRolesRepository<Role> CreateSut()
        {
            return new Authentication.Data.Repositories.RolesRepository(context);
        }
    }
}