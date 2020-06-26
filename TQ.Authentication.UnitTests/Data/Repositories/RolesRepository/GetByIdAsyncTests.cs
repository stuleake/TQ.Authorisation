using FluentAssertions;
using System;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.Data.Repositories;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.RolesRepository
{
    public class GetByIdAsyncTests
    {
        private readonly ApplicationDbContext context;

        public GetByIdAsyncTests()
        {
            context = new InMemoryDbContextFactory(false).GetApplicationDbContext();
        }

        [Fact]
        public async Task GetByIdAsyncReturnsRole()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            this.context.Roles.Add(new Role { Id = roleId });
            await this.context.SaveChangesAsync();

            var sut = this.CreateSut();

            // Act
            var result = await sut.GetByIdAsync(roleId);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByIdAsyncReturnsEmptyRole()
        {
            // Arrange
            this.context.Roles.Add(new Role { Id = Guid.NewGuid() });
            await this.context.SaveChangesAsync();

            var sut = this.CreateSut();

            // Act
            var result = await sut.GetByIdAsync(Guid.NewGuid());

            // Assert
            result.Should().BeNull();
        }

        private IRolesRepository<Role> CreateSut()
        {
            return new Authentication.Data.Repositories.RolesRepository(context);
        }
    }
}