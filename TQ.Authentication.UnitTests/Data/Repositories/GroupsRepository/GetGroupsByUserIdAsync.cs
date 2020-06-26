using FluentAssertions;
using System;
using System.Linq;
using TQ.Authentication.Data.Contexts;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.GroupsRepository
{
    public class GetGroupsByUserIdAsync
    {
        private readonly ApplicationDbContext context;

        public GetGroupsByUserIdAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public void GetGroupsByUserIdForExistingUserReturnsGroup()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetGroupsByUserId(context.Users.FirstOrDefault().Id);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetGroupsByUserIdAsyncForNewUserReturnsNoGroup()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetGroupsByUserId(Guid.NewGuid());

            // Assert
            result.ToList().Should().BeEmpty();
        }

        private Authentication.Data.Repositories.GroupsRepository CreateSut()
        {
            return new Authentication.Data.Repositories.GroupsRepository(context);
        }
    }
}