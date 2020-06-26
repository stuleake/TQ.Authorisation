using FluentAssertions;
using System;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.Data.Repositories;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.RolesRepository
{
    public class GetByIdIncludeRolePermissionsAsyncTests
    {
        private readonly ApplicationDbContext context;

        public GetByIdIncludeRolePermissionsAsyncTests()
        {
            context = new InMemoryDbContextFactory(false).GetApplicationDbContext();
        }

        [Fact]
        public async Task GetByIdIncludeRolePermissionsAyncReturnsNullWhenRoleNotFound()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetByIdIncludeRolePermissionsAync(Guid.NewGuid());

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdIncludeRolePermissionsAyncReturnsRole()
        {
            // Arrange
            var roleId = Guid.NewGuid();

            var role = new Role { Id = roleId };
            this.context.Roles.Add(role);
            this.context.SaveChanges();

            var sut = this.CreateSut();

            // Act
            var result = await sut.GetByIdIncludeRolePermissionsAync(roleId);

            // Assert
            result.Should().NotBeNull();
        }

        private IRolesRepository<Role> CreateSut()
        {
            return new Authentication.Data.Repositories.RolesRepository(context);
        }
    }
}