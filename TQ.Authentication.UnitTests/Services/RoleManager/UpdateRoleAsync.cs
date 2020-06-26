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
    public class UpdateRoleAsync
    {
        [Fact]
        public async Task UpdateRoleAsync_UpdatesRole()
        {
            // Arrange
            var roleId = Guid.NewGuid();

            var rolesRepository = Mock.Create<IRolesRepository<Role>>();
            Mock.Arrange(() => rolesRepository.UpdateRoleAsync(Arg.IsAny<Role>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var groupsRepository = Mock.Create<IGroupsRepository<Group>>();
            Mock.Arrange(() => groupsRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Group, bool>>>()))
                .TaskResult(true);

            Mock.Arrange(() => rolesRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Role, bool>>>()))
                .TaskResult(true);

            Mock.Arrange(() => rolesRepository.RoleNameExistsAsync(Arg.IsAny<Guid>(), Arg.IsAny<string>()))
                .TaskResult(true);

            Mock.Arrange(() => rolesRepository.IsRoleNameUniquePerGroupIdAsync(Arg.IsAny<Guid>(), Arg.IsAny<string>()))
                .TaskResult(true);

            Mock.Arrange(() => rolesRepository.GroupAndRoleCombinationExistsAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .TaskResult(true);

            var rolePermissionsRepository = Mock.Create<IRolePermissionsRepository<RolePermission>>();
            Mock.Arrange(() => rolePermissionsRepository.UpdateMultipleAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<RolePermission>>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var permissionsRepository = Mock.Create<IPermissionsRepository<Permission>>();
            Mock.Arrange(() => permissionsRepository.AllPermissionsBelongToGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<Guid>>()))
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

            var request = new UpdateRoleRequest
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
            await roleManager.UpdateRoleAsync(roleId, request);

            // Assert
            Mock.Assert(() => rolesRepository.UpdateRoleAsync(Arg.IsAny<Role>()), Occurs.Once());

            Mock.Assert(() => rolesRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<Role, bool>>>()), Occurs.Once());

            Mock.Assert(() => rolePermissionsRepository.UpdateMultipleAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<RolePermission>>()),
                Occurs.Once());

            Mock.Assert(() => rolesRepository.RoleNameExistsAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<string>()), Occurs.Once());

            Mock.Assert(() => rolesRepository.IsRoleNameUniquePerGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<string>()), Occurs.Never());

            Mock.Assert(() => rolesRepository.GroupAndRoleCombinationExistsAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<Guid>()), Occurs.Once());

            Mock.Assert(() => permissionsRepository.AllPermissionsBelongToGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<Guid>>()), Occurs.Once());

            Mock.Assert(() => groupsRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<Group, bool>>>()), Occurs.Once());
        }

        [Fact]
        public async Task UpdateRoleAsync_RoleIdNotExist_UpdatesRole()
        {
            // Arrange
            var roleId = Guid.NewGuid();

            var rolesRepository = Mock.Create<IRolesRepository<Role>>();
            Mock.Arrange(() => rolesRepository.UpdateRoleAsync(Arg.IsAny<Role>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var groupsRepository = Mock.Create<IGroupsRepository<Group>>();
            Mock.Arrange(() => groupsRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Group, bool>>>()))
                .TaskResult(true);

            Mock.Arrange(() => rolesRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Role, bool>>>()))
                .TaskResult(false);

            var rolePermissionsRepository = Mock.Create<IRolePermissionsRepository<RolePermission>>();
            Mock.Arrange(() => rolePermissionsRepository.UpdateMultipleAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<RolePermission>>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var permissionsRepository = Mock.Create<IPermissionsRepository<Permission>>();
            Mock.Arrange(() => permissionsRepository.AllPermissionsBelongToGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<Guid>>()))
                .TaskResult(true);

            var userGroupsRepository = Mock.Create<IUserGroupsRepository<UserGroup>>();

            var userRolesRepository = Mock.Create<IUserRolesRepository<UserRole>>();

            var usersRepository = Mock.Create<IUsersRepository<User>>();

            var groupManager = Mock.Create<IGroupManager>();
            var azureGroupManager = Mock.Create<IAzureGroupManager>();
            var permissionManager = Mock.Create<IPermissionManager>();
            var roleConverter = Mock.Create<IRoleConverter>();
            var permissionConverter = Mock.Create<IPermissionConverter>();
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
                usersRepository,
                userGroupsRepository,
                userRolesRepository);

            var request = new UpdateRoleRequest
            {
                Name = "Role 1",
                Description = "Role 1 Description",
                GroupId = Guid.NewGuid(),
                Permissions = new List<Guid>
                {
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                }
            };

            // Act && Assert
            await Assert.ThrowsAsync<TQAuthenticationException>(() => roleManager.UpdateRoleAsync(roleId, request));

            Mock.Assert(() => rolesRepository.UpdateRoleAsync(Arg.IsAny<Role>()), Occurs.Never());

            Mock.Assert(() => rolesRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<Role, bool>>>()), Occurs.Once());

            Mock.Assert(() => rolePermissionsRepository.UpdateMultipleAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<RolePermission>>()),
                Occurs.Never());

            Mock.Assert(() => permissionsRepository.AllPermissionsBelongToGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<Guid>>()), Occurs.Never());

            Mock.Assert(() => groupsRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<Group, bool>>>()), Occurs.Once());
        }

        [Fact]
        public async Task UpdateRoleAsync_RoleNameChanged_UpdatesRole()
        {
            // Arrange
            var roleId = Guid.NewGuid();

            var rolesRepository = Mock.Create<IRolesRepository<Role>>();
            Mock.Arrange(() => rolesRepository.UpdateRoleAsync(Arg.IsAny<Role>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var groupsRepository = Mock.Create<IGroupsRepository<Group>>();
            Mock.Arrange(() => groupsRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Group, bool>>>()))
                .TaskResult(true);

            Mock.Arrange(() => rolesRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Role, bool>>>()))
                .TaskResult(true);

            Mock.Arrange(() => rolesRepository.RoleNameExistsAsync(Arg.IsAny<Guid>(), Arg.IsAny<string>()))
                .TaskResult(false);

            Mock.Arrange(() => rolesRepository.IsRoleNameUniquePerGroupIdAsync(Arg.IsAny<Guid>(), Arg.IsAny<string>()))
                .TaskResult(true);

            Mock.Arrange(() => rolesRepository.GroupAndRoleCombinationExistsAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .TaskResult(true);

            var rolePermissionsRepository = Mock.Create<IRolePermissionsRepository<RolePermission>>();
            Mock.Arrange(() => rolePermissionsRepository.UpdateMultipleAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<RolePermission>>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var permissionsRepository = Mock.Create<IPermissionsRepository<Permission>>();
            Mock.Arrange(() => permissionsRepository.AllPermissionsBelongToGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<Guid>>()))
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

            var request = new UpdateRoleRequest
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
            await roleManager.UpdateRoleAsync(roleId, request);

            // Assert
            Mock.Assert(() => rolesRepository.UpdateRoleAsync(Arg.IsAny<Role>()), Occurs.Once());

            Mock.Assert(() => rolesRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<Role, bool>>>()), Occurs.Once());

            Mock.Assert(() => rolesRepository.RoleNameExistsAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<string>()), Occurs.Once());

            Mock.Assert(() => rolesRepository.IsRoleNameUniquePerGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<string>()), Occurs.Once());

            Mock.Assert(() => rolesRepository.GroupAndRoleCombinationExistsAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<Guid>()), Occurs.Once());

            Mock.Assert(() => rolePermissionsRepository.UpdateMultipleAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<RolePermission>>()),
                Occurs.Once());

            Mock.Assert(() => permissionsRepository.AllPermissionsBelongToGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<Guid>>()), Occurs.Once());

            Mock.Assert(() => groupsRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<Group, bool>>>()), Occurs.Once());
        }

        [Fact]
        public async Task UpdateRoleAsync_RoleNameNotUniquePerGroupId_throwsTQAuthenticationException()
        {
            // Arrange
            var roleId = Guid.NewGuid();

            var rolesRepository = Mock.Create<IRolesRepository<Role>>();
            Mock.Arrange(() => rolesRepository.UpdateRoleAsync(Arg.IsAny<Role>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var groupsRepository = Mock.Create<IGroupsRepository<Group>>();
            Mock.Arrange(() => groupsRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Group, bool>>>()))
                .TaskResult(true);

            Mock.Arrange(() => rolesRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Role, bool>>>()))
                .TaskResult(true);

            Mock.Arrange(() => rolesRepository.RoleNameExistsAsync(Arg.IsAny<Guid>(), Arg.IsAny<string>()))
                .TaskResult(false);

            Mock.Arrange(() => rolesRepository.IsRoleNameUniquePerGroupIdAsync(Arg.IsAny<Guid>(), Arg.IsAny<string>()))
                .TaskResult(false);

            Mock.Arrange(() => rolesRepository.GroupAndRoleCombinationExistsAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .TaskResult(true);

            var rolePermissionsRepository = Mock.Create<IRolePermissionsRepository<RolePermission>>();
            Mock.Arrange(() => rolePermissionsRepository.UpdateMultipleAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<RolePermission>>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var permissionsRepository = Mock.Create<IPermissionsRepository<Permission>>();
            Mock.Arrange(() => permissionsRepository.AllPermissionsBelongToGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<Guid>>()))
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

            var request = new UpdateRoleRequest
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
            await Assert.ThrowsAsync<TQAuthenticationException>(() => roleManager.UpdateRoleAsync(roleId, request));

            Mock.Assert(() => rolesRepository.UpdateRoleAsync(Arg.IsAny<Role>()), Occurs.Never());

            Mock.Assert(() => rolesRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<Role, bool>>>()), Occurs.Once());

            Mock.Assert(() => rolesRepository.RoleNameExistsAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<string>()), Occurs.Once());

            Mock.Assert(() => rolesRepository.IsRoleNameUniquePerGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<string>()), Occurs.Once());

            Mock.Assert(() => rolesRepository.GroupAndRoleCombinationExistsAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<Guid>()), Occurs.Once());

            Mock.Assert(() => rolePermissionsRepository.UpdateMultipleAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<RolePermission>>()),
                Occurs.Never());

            Mock.Assert(() => permissionsRepository.AllPermissionsBelongToGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<Guid>>()), Occurs.Never());

            Mock.Assert(() => groupsRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<Group, bool>>>()), Occurs.Once());
        }

        [Fact]
        public async Task UpdateRoleAsync_GroupAndRoleCombinationInvalid_ThrowsArgumentException()
        {
            // Arrange
            var roleId = Guid.NewGuid();

            var rolesRepository = Mock.Create<IRolesRepository<Role>>();
            Mock.Arrange(() => rolesRepository.UpdateRoleAsync(Arg.IsAny<Role>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var groupsRepository = Mock.Create<IGroupsRepository<Group>>();
            Mock.Arrange(() => groupsRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Group, bool>>>()))
                .TaskResult(true);

            Mock.Arrange(() => rolesRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Role, bool>>>()))
                .TaskResult(true);

            Mock.Arrange(() => rolesRepository.RoleNameExistsAsync(Arg.IsAny<Guid>(), Arg.IsAny<string>()))
                .TaskResult(true);

            Mock.Arrange(() => rolesRepository.IsRoleNameUniquePerGroupIdAsync(Arg.IsAny<Guid>(), Arg.IsAny<string>()))
                .TaskResult(true);

            Mock.Arrange(() => rolesRepository.GroupAndRoleCombinationExistsAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .TaskResult(false);

            var rolePermissionsRepository = Mock.Create<IRolePermissionsRepository<RolePermission>>();
            Mock.Arrange(() => rolePermissionsRepository.UpdateMultipleAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<RolePermission>>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var permissionsRepository = Mock.Create<IPermissionsRepository<Permission>>();
            Mock.Arrange(() => permissionsRepository.AllPermissionsBelongToGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<Guid>>()))
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

            var request = new UpdateRoleRequest
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
            await Assert.ThrowsAsync<ArgumentException>(() => roleManager.UpdateRoleAsync(roleId, request));

            Mock.Assert(() => rolesRepository.UpdateRoleAsync(Arg.IsAny<Role>()), Occurs.Never());

            Mock.Assert(() => rolesRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<Role, bool>>>()), Occurs.Once());

            Mock.Assert(() => rolePermissionsRepository.UpdateMultipleAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<RolePermission>>()),
                Occurs.Never());

            Mock.Assert(() => rolesRepository.RoleNameExistsAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<string>()), Occurs.Never());

            Mock.Assert(() => rolesRepository.IsRoleNameUniquePerGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<string>()), Occurs.Never());

            Mock.Assert(() => rolesRepository.GroupAndRoleCombinationExistsAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<Guid>()), Occurs.Once());

            Mock.Assert(() => permissionsRepository.AllPermissionsBelongToGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<IEnumerable<Guid>>()), Occurs.Never());

            Mock.Assert(() => groupsRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<Group, bool>>>()), Occurs.Once());
        }
    }
}