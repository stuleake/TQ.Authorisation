using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using TQ.Authentication.Data.Entities;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.RolesRepository
{
    public class GetRolesForGroupAsyncTests
    {
        private readonly ApplicationDbContext context;

        public GetRolesForGroupAsyncTests()
        {
            context = new InMemoryDbContextFactory(false).GetApplicationDbContext();
        }

        [Fact]
        public async Task GetRolesForGroupAsyncReturnsRolesSuccess()
        {
            // Arrange
            var expectedRoleCount = 2;
            var groupId = Guid.NewGuid();
            this.context.Groups.Add(new Group { Id = groupId, IsActive = false });
            await this.context.SaveChangesAsync();

            this.context.Roles.Add(new Role { Id = Guid.NewGuid(), Name = "role1", Description = "role1 description", GroupId = groupId });
            this.context.Roles.Add(new Role { Id = Guid.NewGuid(), Name = "role2", Description = "role2 description", GroupId = groupId });
            await this.context.SaveChangesAsync();

            var sut = this.CreateSut();

            // Act
            var result = await sut.GetRolesForGroupAsync(groupId);

            // Assert
            result.Count().Should().Be(expectedRoleCount);
        }

        [Fact]
        public async Task GetRolesForGroupAsyncReturnsEmptyRolesSuccess()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            this.context.Groups.Add(new Authentication.Data.Entities.Group { Id = groupId, IsActive = false });
            await this.context.SaveChangesAsync();

            var sut = this.CreateSut();

            // Act
            var result = await sut.GetRolesForGroupAsync(groupId);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetRolesForGroupAsyncThrowsArgumentNullExceptionWhenGroupIdIsEmpty()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetRolesForGroupAsync(Guid.Empty));
        }

        private Authentication.Data.Repositories.RolesRepository CreateSut()
        {
            return new Authentication.Data.Repositories.RolesRepository(context);
        }
    }
}