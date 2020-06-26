using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.Data.Repositories;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.Services.Interfaces;
using TQ.Authentication.Services.Interfaces.Converters;
using Xunit;

namespace TQ.Authentication.UnitTests.Services.RoleManager
{
    public class AddUsersToRoleAsyncTests

    {
        private readonly IGraphApiClient graphApiClient = Mock.Create<IGraphApiClient>();
        private readonly IRolesRepository<Role> rolesRepository = Mock.Create<IRolesRepository<Role>>();
        private readonly IRolePermissionsRepository<RolePermission> rolePermissionsRepository = Mock.Create<IRolePermissionsRepository<RolePermission>>();
        private readonly IPermissionsRepository<Permission> permissionsRepository = Mock.Create<IPermissionsRepository<Permission>>();
        private readonly IGroupsRepository<Group> groupsRepository = Mock.Create<IGroupsRepository<Group>>();
        private readonly IGroupManager groupManager = Mock.Create<IGroupManager>();
        private readonly IAzureGroupManager azureGroupManager = Mock.Create<IAzureGroupManager>();
        private readonly IPermissionManager permissionManager = Mock.Create<IPermissionManager>();
        private readonly IRoleConverter roleConverter = Mock.Create<IRoleConverter>();
        private readonly IPermissionConverter permissionConverter = Mock.Create<IPermissionConverter>();
        private readonly IUsersRepository<User> usersRepository = Mock.Create<IUsersRepository<User>>();
        private readonly IUserGroupsRepository<UserGroup> userGroupsRepository = Mock.Create<IUserGroupsRepository<UserGroup>>();
        private readonly IUserRolesRepository<UserRole> userRolesRepository = Mock.Create<IUserRolesRepository<UserRole>>();

        [Fact]
        public async Task AddUsersToRoleAsyncReturnsSuccess()
        {
            // Arrange
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var userIdWithMatchingGroupId = Guid.NewGuid();
            var roleId = Guid.NewGuid();
            var listOfUsers = new List<Guid>() { userId1, userId2 };
            var listOfUsers2 = new List<Guid?>() { userIdWithMatchingGroupId };

            Mock.Arrange(() => rolesRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Role, bool>>>()))
                .TaskResult(true);

            Mock.Arrange(() => usersRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<User, bool>>>()))
                .TaskResult(true);

            Mock.Arrange(() => userGroupsRepository.GetUserGroupIdsByUser(Arg.IsAny<Guid>()))
                .TaskResult(listOfUsers2);

            Mock.Arrange(() => rolesRepository.GetGroupIdByRoleAsync(Arg.IsAny<Guid>()))
                .TaskResult(userIdWithMatchingGroupId);

            Mock.Arrange(() => userRolesRepository.IsUserAlreadyAssignedToRoleAsync(Arg.IsAny<Guid>(), Arg.IsAny<List<Guid>>()))
                .TaskResult(null);

            var sut = this.CreateSut();

            // Act
            await sut.AddUsersToRoleAsync(roleId, listOfUsers);
            
            // Assert
            Mock.Assert(() => rolesRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<Role, bool>>>()), Occurs.Once());

            Mock.Assert(() => usersRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<User, bool>>>()), Occurs.AtLeastOnce());

            Mock.Assert(() => userGroupsRepository.GetUserGroupIdsByUser(
                Arg.IsAny<Guid>()), Occurs.AtLeastOnce());

            Mock.Assert(() => rolesRepository.GetGroupIdByRoleAsync(
                Arg.IsAny<Guid>()), Occurs.AtLeastOnce());

            Mock.Assert(() => userRolesRepository.IsUserAlreadyAssignedToRoleAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<List<Guid>>()), Occurs.AtLeastOnce());
        }

        [Fact]
        public async Task AddUsersToRoleAsyncReturnsErrorWhenRoleDoesNotExist()
        {
            // Arrange
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var userIdWithMatchingGroupId = Guid.NewGuid();
            var roleId = Guid.NewGuid();
            var listOfUsers = new List<Guid>() { userId1, userId2 };
            var listOfUsers2 = new List<Guid?>() { userIdWithMatchingGroupId };

            Mock.Arrange(() => rolesRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Role, bool>>>()))
                .TaskResult(false);

            Mock.Arrange(() => usersRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<User, bool>>>()))
                .TaskResult(true);

            Mock.Arrange(() => userGroupsRepository.GetUserGroupIdsByUser(Arg.IsAny<Guid>()))
                .TaskResult(listOfUsers2);

            Mock.Arrange(() => rolesRepository.GetGroupIdByRoleAsync(Arg.IsAny<Guid>()))
                .TaskResult(userIdWithMatchingGroupId);

            Mock.Arrange(() => userRolesRepository.IsUserAlreadyAssignedToRoleAsync(Arg.IsAny<Guid>(), Arg.IsAny<List<Guid>>()))
                .TaskResult(null);

            var sut = this.CreateSut();

            // Act
            await Assert.ThrowsAsync<TQAuthenticationException>(() => sut.AddUsersToRoleAsync(roleId, listOfUsers));

            // Assert
            Mock.Assert(() => rolesRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<Role, bool>>>()), Occurs.Once());

            Mock.Assert(() => usersRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<User, bool>>>()), Occurs.Never());

            Mock.Assert(() => userGroupsRepository.GetUserGroupIdsByUser(
                Arg.IsAny<Guid>()), Occurs.Never());

            Mock.Assert(() => rolesRepository.GetGroupIdByRoleAsync(
                Arg.IsAny<Guid>()), Occurs.Never());

            Mock.Assert(() => userRolesRepository.IsUserAlreadyAssignedToRoleAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<List<Guid>>()), Occurs.Never());
        }

        [Fact]
        public async Task AddUsersToRoleAsyncReturnsErrorWhenUserDoesNotExist()
        {
            // Arrange
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var userIdWithMatchingGroupId = Guid.NewGuid();
            var roleId = Guid.NewGuid();
            var listOfUsers = new List<Guid>() { userId1, userId2 };
            var listOfUsers2 = new List<Guid?>() { userIdWithMatchingGroupId };

            Mock.Arrange(() => rolesRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Role, bool>>>()))
                .TaskResult(true);

            Mock.Arrange(() => usersRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<User, bool>>>()))
                .TaskResult(false);

            Mock.Arrange(() => userGroupsRepository.GetUserGroupIdsByUser(Arg.IsAny<Guid>()))
                .TaskResult(listOfUsers2);

            Mock.Arrange(() => rolesRepository.GetGroupIdByRoleAsync(Arg.IsAny<Guid>()))
                .TaskResult(userIdWithMatchingGroupId);

            Mock.Arrange(() => userRolesRepository.IsUserAlreadyAssignedToRoleAsync(Arg.IsAny<Guid>(), Arg.IsAny<List<Guid>>()))
                .TaskResult(null);

            var sut = this.CreateSut();

            // Act
            await Assert.ThrowsAsync<TQAuthenticationException>(() => sut.AddUsersToRoleAsync(roleId, listOfUsers));

            // Assert
            Mock.Assert(() => rolesRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<Role, bool>>>()), Occurs.Once());

            Mock.Assert(() => usersRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<User, bool>>>()), Occurs.AtLeastOnce());

            Mock.Assert(() => userGroupsRepository.GetUserGroupIdsByUser(
                Arg.IsAny<Guid>()), Occurs.Never());

            Mock.Assert(() => rolesRepository.GetGroupIdByRoleAsync(
                Arg.IsAny<Guid>()), Occurs.Never());

            Mock.Assert(() => userRolesRepository.IsUserAlreadyAssignedToRoleAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<List<Guid>>()), Occurs.Never());
        }

        [Fact]
        public async Task AddUsersToRoleAsyncReturnsErrorWhenUserGroupMismatched()
        {
            // Arrange
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var userIdWithMatchingGroupId = Guid.NewGuid();
            var roleId = Guid.NewGuid();
            var listOfUsers = new List<Guid>() { userId1, userId2 };
            var listOfUsers2 = new List<Guid?>() { userIdWithMatchingGroupId };

            Mock.Arrange(() => rolesRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Role, bool>>>()))
                .TaskResult(true);

            Mock.Arrange(() => usersRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<User, bool>>>()))
                .TaskResult(true);

            Mock.Arrange(() => userGroupsRepository.GetUserGroupIdsByUser(Arg.IsAny<Guid>()))
                .TaskResult(new List<Guid?>());

            Mock.Arrange(() => rolesRepository.GetGroupIdByRoleAsync(Arg.IsAny<Guid>()))
                .TaskResult(userIdWithMatchingGroupId);

            Mock.Arrange(() => userRolesRepository.IsUserAlreadyAssignedToRoleAsync(Arg.IsAny<Guid>(), Arg.IsAny<List<Guid>>()))
                .TaskResult(null);

            var sut = this.CreateSut();

            // Act
            await Assert.ThrowsAsync<TQAuthenticationException>(() => sut.AddUsersToRoleAsync(roleId, listOfUsers));

            // Assert
            Mock.Assert(() => rolesRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<Role, bool>>>()), Occurs.Once());

            Mock.Assert(() => usersRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<User, bool>>>()), Occurs.AtLeastOnce());

            Mock.Assert(() => userGroupsRepository.GetUserGroupIdsByUser(
                Arg.IsAny<Guid>()), Occurs.AtLeastOnce());

            Mock.Assert(() => rolesRepository.GetGroupIdByRoleAsync(
                Arg.IsAny<Guid>()), Occurs.AtLeastOnce());

            Mock.Assert(() => userRolesRepository.IsUserAlreadyAssignedToRoleAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<List<Guid>>()), Occurs.Never());
        }

        [Fact]
        public async Task AddUsersToRoleAsyncReturnsErrorWhenGroupMismatched()
        {
            // Arrange
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var userIdWithMatchingGroupId = Guid.NewGuid();
            var roleId = Guid.NewGuid();
            var listOfUsers = new List<Guid>() { userId1, userId2 };
            var listOfUsers2 = new List<Guid?>() { userIdWithMatchingGroupId };

            Mock.Arrange(() => rolesRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Role, bool>>>()))
                .TaskResult(true);

            Mock.Arrange(() => usersRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<User, bool>>>()))
                .TaskResult(true);

            Mock.Arrange(() => userGroupsRepository.GetUserGroupIdsByUser(Arg.IsAny<Guid>()))
                .TaskResult(listOfUsers2);

            Mock.Arrange(() => rolesRepository.GetGroupIdByRoleAsync(Arg.IsAny<Guid>()))
                .TaskResult(Guid.NewGuid());

            Mock.Arrange(() => userRolesRepository.IsUserAlreadyAssignedToRoleAsync(Arg.IsAny<Guid>(), Arg.IsAny<List<Guid>>()))
                .TaskResult(null);

            var sut = this.CreateSut();

            // Act
            await Assert.ThrowsAsync<TQAuthenticationException>(() => sut.AddUsersToRoleAsync(roleId, listOfUsers));

            // Assert
            Mock.Assert(() => rolesRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<Role, bool>>>()), Occurs.Once());

            Mock.Assert(() => usersRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<User, bool>>>()), Occurs.AtLeastOnce());

            Mock.Assert(() => userGroupsRepository.GetUserGroupIdsByUser(
                Arg.IsAny<Guid>()), Occurs.AtLeastOnce());

            Mock.Assert(() => rolesRepository.GetGroupIdByRoleAsync(
                Arg.IsAny<Guid>()), Occurs.AtLeastOnce());

            Mock.Assert(() => userRolesRepository.IsUserAlreadyAssignedToRoleAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<List<Guid>>()), Occurs.Never());
        }

        [Fact]
        public async Task AddUsersToRoleAsyncReturnsErrorWhenUserAlreadyAssignedRole()
        {
            // Arrange
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var userIdWithMatchingGroupId = Guid.NewGuid();
            var roleId = Guid.NewGuid();
            var listOfUsers = new List<Guid>() { userId1, userId2 };
            var listOfUsers2 = new List<Guid?>() { userIdWithMatchingGroupId };

            Mock.Arrange(() => rolesRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Role, bool>>>()))
                .TaskResult(true);

            Mock.Arrange(() => usersRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<User, bool>>>()))
                .TaskResult(true);

            Mock.Arrange(() => userGroupsRepository.GetUserGroupIdsByUser(Arg.IsAny<Guid>()))
                .TaskResult(listOfUsers2);

            Mock.Arrange(() => rolesRepository.GetGroupIdByRoleAsync(Arg.IsAny<Guid>()))
                .TaskResult(userIdWithMatchingGroupId);

            Mock.Arrange(() => userRolesRepository.IsUserAlreadyAssignedToRoleAsync(Arg.IsAny<Guid>(), Arg.IsAny<List<Guid>>()))
                .TaskResult(userId1);

            var sut = this.CreateSut();

            // Act
            await Assert.ThrowsAsync<TQAuthenticationException>(() => sut.AddUsersToRoleAsync(roleId, listOfUsers));

            // Assert
            Mock.Assert(() => rolesRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<Role, bool>>>()), Occurs.Once());

            Mock.Assert(() => usersRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<User, bool>>>()), Occurs.AtLeastOnce());

            Mock.Assert(() => userGroupsRepository.GetUserGroupIdsByUser(
                Arg.IsAny<Guid>()), Occurs.AtLeastOnce());

            Mock.Assert(() => rolesRepository.GetGroupIdByRoleAsync(
                Arg.IsAny<Guid>()), Occurs.AtLeastOnce());

            Mock.Assert(() => userRolesRepository.IsUserAlreadyAssignedToRoleAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<List<Guid>>()), Occurs.AtLeastOnce());
        }

        private IRoleManager CreateSut()
        {
            return new Authentication.Services.RoleManager(
                graphApiClient,
                rolesRepository,
                rolePermissionsRepository,
                permissionsRepository,
                groupsRepository,
                groupManager,
                azureGroupManager,
                permissionManager,
                roleConverter,
                permissionConverter,
                usersRepository,
                userGroupsRepository,
                userRolesRepository);
        }
    }
}