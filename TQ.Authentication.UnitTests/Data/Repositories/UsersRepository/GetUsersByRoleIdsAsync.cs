using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using TQ.Authentication.Data.Entities;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.UsersRepository
{
    public class GetUsersByRoleIdsAsync
    {
        private readonly ApplicationDbContext context;
        private Authentication.Data.Repositories.UsersRepository usersRepository;

        public GetUsersByRoleIdsAsync()
        {
            context = new InMemoryDbContextFactory().GetApplicationDbContext();
        }

        [Fact]
        public async Task GetUsersByRole_ReturnsUsers()
        {
            // Arrange
            usersRepository = CreateSut();
            var roleId = context.Roles.FirstOrDefault().Id;
            var UserIds = context.Users.Where(x => x.UserRoles != null);
            var userRoleIds = context.UserRoles.Where(x => x.RoleId == roleId);

            var userList = new List<Guid>();
            foreach (var user in UserIds) { userList.Add(user.Id); } 

            // Act
            var users = await usersRepository.GetUsersByRoleIdsAsync(roleId, userList);

            // Assert
            users.Should().HaveCount(1);
        }

        [Fact]
        public async Task CreateAsync_NoUsersFound_ReturnsEmptyCollection()
        {
            // Arrange
            usersRepository = CreateSut();
            var roleId = context.Roles.FirstOrDefault().Id;
            var userList = new List<Guid>();

            // Act
            var users = await usersRepository.GetUsersByRoleIdsAsync(roleId, userList);

            // Assert
            users.Should().BeEmpty();
        }

        private Authentication.Data.Repositories.UsersRepository CreateSut()
        {
            return new Authentication.Data.Repositories.UsersRepository(context);
        }
    }
}