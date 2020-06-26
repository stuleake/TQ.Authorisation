using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.Core.Requests.Roles;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.Data.Repositories;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.Services.Interfaces;
using TQ.Authentication.Services.Interfaces.Converters;
using Xunit;

namespace TQ.Authentication.UnitTests.Services.RoleManager
{
    public class CreateRoleAsync
    {
        [Fact]
        public async Task CreateRoleAsync_CreatesRole()
        {
            // Arrange
            var roleId = Guid.NewGuid();

            var rolesRepository = Mock.Create<IRolesRepository<Role>>();
            Mock.Arrange(() => rolesRepository.CreateAsync(Arg.IsAny<Role>()))
                .TaskResult(roleId);

            Mock.Arrange(() => rolesRepository.IsRoleNameUniquePerGroupIdAsync(Arg.IsAny<Guid>(), Arg.IsAny<string>()))
                .TaskResult(true);

            var rolePermissionsRepository = Mock.Create<IRolePermissionsRepository<RolePermission>>();
            Mock.Arrange(() => rolePermissionsRepository.CreateMultipleAsync(Arg.IsAny<IEnumerable<RolePermission>>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var permissionsRepository = Mock.Create<IPermissionsRepository<Permission>>();
            Mock.Arrange(() => permissionsRepository.AllPermissionsBelongToGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<Guid>>()))
                .TaskResult(true);

            var groupsRepository = Mock.Create<IGroupsRepository<Group>>();
            Mock.Arrange(() => groupsRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Group, bool>>>()))
                .TaskResult(true);

            var groupManager = Mock.Create<IGroupManager>();
            var azureGroupManager = Mock.Create<IAzureGroupManager>();
            var permissionManager = Mock.Create<IPermissionManager>();
            var roleConverter = Mock.Create<IRoleConverter>();
            var permissionConverter = Mock.Create<IPermissionConverter>();

            var userGroupsRepository = Mock.Create<IUserGroupsRepository<UserGroup>>();

            var userRolesRepository = Mock.Create<IUserRolesRepository<UserRole>>();

            var userRepository = Mock.Create<IUsersRepository<User>>();
            var graphApiClient = Mock.Create<IGraphApiClient>();
            var roleManager = new Authentication.Services.RoleManager(
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
                userRepository,
                userGroupsRepository,
                userRolesRepository);

            var request = new CreateRoleRequest
            {
                Name = "Role 1",
                Description = "Role 1 Description",
                GroupId = Guid.NewGuid(),
                Permissions = new List<Guid>
                {
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                }
            };

            // Act
            var newRoleId = await roleManager.CreateRoleAsync(request);

            // Assert
            Assert.Equal(roleId, newRoleId);

            Mock.Assert(() => rolesRepository.CreateAsync(Arg.IsAny<Role>()), Occurs.Once());

            Mock.Assert(() => rolesRepository.IsRoleNameUniquePerGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<string>()), Occurs.Once());

            Mock.Assert(() => rolePermissionsRepository.CreateMultipleAsync(
                Arg.IsAny<IEnumerable<RolePermission>>()), Occurs.Once());

            Mock.Assert(() => permissionsRepository.AllPermissionsBelongToGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<Guid>>()), Occurs.Once());

            Mock.Assert(() => groupsRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<Group, bool>>>()), Occurs.Once());
        }

        [Fact]
        public async Task CreateRoleAsync_GroupIdNotExist_ThrowsArgumentException()
        {
            //  Arrange
            var roleId = Guid.NewGuid();

            var rolesRepository = Mock.Create<IRolesRepository<Role>>();
            Mock.Arrange(() => rolesRepository.CreateAsync(Arg.IsAny<Role>()))
                .TaskResult(roleId);

            Mock.Arrange(() => rolesRepository.IsRoleNameUniquePerGroupIdAsync(Arg.IsAny<Guid>(), Arg.IsAny<string>()))
                .TaskResult(true);

            var rolePermissionsRepository = Mock.Create<IRolePermissionsRepository<RolePermission>>();
            Mock.Arrange(() => rolePermissionsRepository.CreateMultipleAsync(Arg.IsAny<IEnumerable<RolePermission>>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var permissionsRepository = Mock.Create<IPermissionsRepository<Permission>>();
            Mock.Arrange(() => permissionsRepository.AllPermissionsBelongToGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<Guid>>()))
                .TaskResult(true);

            var groupsRepository = Mock.Create<IGroupsRepository<Group>>();
            Mock.Arrange(() => groupsRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Group, bool>>>()))
                .TaskResult(false);

            var userGroupsRepository = Mock.Create<IUserGroupsRepository<UserGroup>>();

            var userRolesRepository = Mock.Create<IUserRolesRepository<UserRole>>();

            var usersRepository = Mock.Create<IUsersRepository<User>>();

            var groupManager = Mock.Create<IGroupManager>();
            var azureGroupManager = Mock.Create<IAzureGroupManager>();
            var permissionManager = Mock.Create<IPermissionManager>();
            var roleConverter = Mock.Create<IRoleConverter>();
            var graphApiClient = Mock.Create<IGraphApiClient>();
            var permissionConverter = Mock.Create<IPermissionConverter>();

            var roleManager = new Authentication.Services.RoleManager(
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

            var request = new CreateRoleRequest
            {
                Name = "Role 1",
                Description = "Role 1 Description",
                GroupId = Guid.NewGuid(),
                Permissions = new List<Guid>()
            };

            // Act & Assert
            await Assert.ThrowsAsync<TQAuthenticationException>(() => roleManager.CreateRoleAsync(request));

            Mock.Assert(() => rolesRepository.CreateAsync(Arg.IsAny<Role>()), Occurs.Never());

            Mock.Assert(() => rolesRepository.IsRoleNameUniquePerGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<string>()), Occurs.Never());

            Mock.Assert(() => rolePermissionsRepository.CreateMultipleAsync(
                Arg.IsAny<IEnumerable<RolePermission>>()), Occurs.Never());

            Mock.Assert(() => permissionsRepository.AllPermissionsBelongToGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<Guid>>()), Occurs.Never());

            Mock.Assert(() => groupsRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<Group, bool>>>()), Occurs.Once());
        }

        [Fact]
        public async Task CreateRoleAsync_InvalidGroupId_ThrowsArgumentException()
        {
            // Arrange
            var roleId = Guid.NewGuid();

            var rolesRepository = Mock.Create<IRolesRepository<Role>>();
            Mock.Arrange(() => rolesRepository.CreateAsync(Arg.IsAny<Role>()))
                .TaskResult(roleId);

            Mock.Arrange(() => rolesRepository.IsRoleNameUniquePerGroupIdAsync(Arg.IsAny<Guid>(), Arg.IsAny<string>()))
                .TaskResult(true);

            var rolePermissionsRepository = Mock.Create<IRolePermissionsRepository<RolePermission>>();
            Mock.Arrange(() => rolePermissionsRepository.CreateMultipleAsync(Arg.IsAny<IEnumerable<RolePermission>>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var permissionsRepository = Mock.Create<IPermissionsRepository<Permission>>();
            Mock.Arrange(() => permissionsRepository.AllPermissionsBelongToGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<Guid>>()))
                .TaskResult(false);

            var groupsRepository = Mock.Create<IGroupsRepository<Group>>();
            Mock.Arrange(() => groupsRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Group, bool>>>()))
                .TaskResult(true);

            var groupManager = Mock.Create<IGroupManager>();
            var azureGroupManager = Mock.Create<IAzureGroupManager>();
            var permissionManager = Mock.Create<IPermissionManager>();
            var roleConverter = Mock.Create<IRoleConverter>();
            var permissionConverter = Mock.Create<IPermissionConverter>();

            var userGroupsRepository = Mock.Create<IUserGroupsRepository<UserGroup>>();

            var userRolesRepository = Mock.Create<IUserRolesRepository<UserRole>>();
            var graphApiClient = Mock.Create<IGraphApiClient>();

            var usersRepository = Mock.Create<IUsersRepository<User>>();

            var roleManager = new Authentication.Services.RoleManager(
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

            var request = new CreateRoleRequest
            {
                Name = "Role 1",
                Description = "Role 1 Description",
                GroupId = Guid.NewGuid(),
                Permissions = new List<Guid>
                {
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                }
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => roleManager.CreateRoleAsync(request));

            Mock.Assert(() => rolesRepository.CreateAsync(Arg.IsAny<Role>()), Occurs.Never());

            Mock.Assert(() => rolesRepository.IsRoleNameUniquePerGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<string>()), Occurs.Once());

            Mock.Assert(() => rolePermissionsRepository.CreateMultipleAsync(
                Arg.IsAny<IEnumerable<RolePermission>>()), Occurs.Never());

            Mock.Assert(() => permissionsRepository.AllPermissionsBelongToGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<Guid>>()), Occurs.Once());

            Mock.Assert(() => groupsRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<Group, bool>>>()), Occurs.Once());
        }

        [Fact]
        public async Task CreateRoleAsync_PermissionsListNotUnique_ThrowsArgumentException()
        {
            // Arrange
            var roleId = Guid.NewGuid();

            var rolesRepository = Mock.Create<IRolesRepository<Role>>();
            Mock.Arrange(() => rolesRepository.CreateAsync(Arg.IsAny<Role>()))
                .TaskResult(roleId);

            Mock.Arrange(() => rolesRepository.IsRoleNameUniquePerGroupIdAsync(Arg.IsAny<Guid>(), Arg.IsAny<string>()))
                .TaskResult(true);

            var rolePermissionsRepository = Mock.Create<IRolePermissionsRepository<RolePermission>>();
            Mock.Arrange(() => rolePermissionsRepository.CreateMultipleAsync(Arg.IsAny<IEnumerable<RolePermission>>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var permissionsRepository = Mock.Create<IPermissionsRepository<Permission>>();
            Mock.Arrange(() => permissionsRepository.AllPermissionsBelongToGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<Guid>>()))
                .TaskResult(true);

            var groupsRepository = Mock.Create<IGroupsRepository<Group>>();
            Mock.Arrange(() => groupsRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Group, bool>>>()))
                .TaskResult(true);

            var groupManager = Mock.Create<IGroupManager>();
            var azureGroupManager = Mock.Create<IAzureGroupManager>();
            var permissionManager = Mock.Create<IPermissionManager>();
            var roleConverter = Mock.Create<IRoleConverter>();
            var permissionConverter = Mock.Create<IPermissionConverter>();

            var userGroupsRepository = Mock.Create<IUserGroupsRepository<UserGroup>>();

            var userRolesRepository = Mock.Create<IUserRolesRepository<UserRole>>();
            var graphApiClient = Mock.Create<IGraphApiClient>();

            var usersRepository = Mock.Create<IUsersRepository<User>>();

            var roleManager = new Authentication.Services.RoleManager(
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

            var duplicatePermissionId = Guid.NewGuid();

            var request = new CreateRoleRequest
            {
                Name = "Role 1",
                Description = "Role 1 Description",
                GroupId = Guid.NewGuid(),
                Permissions = new List<Guid>
                {
                    Guid.NewGuid(), Guid.NewGuid(), duplicatePermissionId, duplicatePermissionId
                }
            };

            // Act & Assert

            await Assert.ThrowsAsync<ArgumentException>(() => roleManager.CreateRoleAsync(request));

            Mock.Assert(() => rolesRepository.IsRoleNameUniquePerGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<string>()), Occurs.Once());

            Mock.Assert(() => rolePermissionsRepository.CreateMultipleAsync(
                Arg.IsAny<IEnumerable<RolePermission>>()), Occurs.Never());

            Mock.Assert(() => permissionsRepository.AllPermissionsBelongToGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<Guid>>()), Occurs.Once());

            Mock.Assert(() => groupsRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<Group, bool>>>()), Occurs.Once());
        }
    }
}