using FluentAssertions;
using System;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using TQ.Authentication.Data.Entities;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.RolesRepository
{
    public class GetGroupIdByRoleAsyncTests
    {
        private readonly ApplicationDbContext context;

        public GetGroupIdByRoleAsyncTests()
        {
            context = new InMemoryDbContextFactory(false).GetApplicationDbContext();
        }

        [Fact]
        public async Task GetGroupIdByRoleAsyncReturnsGroupSuccess()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            var groupId = Guid.NewGuid();
            var expectedResult = groupId;

            context.Roles.Add(new Role { Id = roleId, Name = "role1", Description = "role1 description", GroupId = groupId });
            await context.SaveChangesAsync();

            context.Groups.Add(new Group { Id = groupId, IsActive = false });
            await context.SaveChangesAsync();

            var sut = CreateSut();

            // Act
            var result = await sut.GetGroupIdByRoleAsync(roleId);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public async Task GetGroupIdByRoleAsyncReturnsGroupNotEmpty()
        {
            {
                // Arrange
                var roleId = Guid.NewGuid();
                var groupId = Guid.NewGuid();

                context.Roles.Add(new Role { Id = roleId, Name = "role1", Description = "role1 description", GroupId = groupId });
                await context.SaveChangesAsync();

                var sut = CreateSut();

                // Act
                var result = await sut.GetGroupIdByRoleAsync(roleId);

                // Assert
                result.Should().NotBeEmpty();
            }
        }

        [Fact]
        public async Task GetGroupIdByRoleAsyncThrowsArgumentNullExceptionWhenGroupIdIsEmpty()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetGroupIdByRoleAsync(Guid.Empty));
        }

        private Authentication.Data.Repositories.RolesRepository CreateSut()
        {
            return new Authentication.Data.Repositories.RolesRepository(context);
        }
    }
}