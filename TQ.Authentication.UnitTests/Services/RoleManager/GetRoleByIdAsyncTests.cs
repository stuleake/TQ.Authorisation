using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Dto.Permissions;
using TQ.Authentication.Core.Dto.Roles;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.Data.Repositories;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.ExternalServices.GraphAPI.Models;
using TQ.Authentication.Services.Interfaces;
using TQ.Authentication.Services.Interfaces.Converters;
using Xunit;
using Group = TQ.Authentication.Data.Entities.Group;
using Permission = TQ.Authentication.Data.Entities.Permission;
using RolePermission = TQ.Authentication.Data.Entities.RolePermission;

namespace TQ.Authentication.UnitTests.Services.RoleManager
{
    public class GetRoleByIdAsyncTests
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
        public async Task GetRoleByIdAsyncReturnsSuccess()
        {
            // Arrange
            Mock.Arrange(() => groupManager.GetGroupByIdAsync(Arg.AnyGuid)).TaskResult(new Group { Id = Guid.NewGuid() });
            Mock.Arrange(() => azureGroupManager.GetByIdAsync(Arg.AnyGuid)).TaskResult(new AzureGroup { Id = Guid.NewGuid() });
            Mock.Arrange(() => roleConverter.ToRoleGroupPermissionGetDto(Arg.IsAny<Role>(), Arg.IsAny<Group>(), Arg.IsAny<AzureGroup>(), Arg.IsAny<List<GetPermissionsDto>>())).Returns(new GetRoleGroupPermissionDto());

            var sut = this.CreateSut();

            // Act
            var result = await sut.GetRoleByIdAsync(Guid.NewGuid());

            // Asset
            result.Should().NotBeNull();
            Mock.Assert(() => groupManager.GetGroupByIdAsync(Arg.AnyGuid), Occurs.Once());
            Mock.Assert(() => azureGroupManager.GetByIdAsync(Arg.AnyGuid), Occurs.Once());
            Mock.Assert(() => roleConverter.ToRoleGroupPermissionGetDto(Arg.IsAny<Role>(), Arg.IsAny<Group>(), Arg.IsAny<AzureGroup>(), Arg.IsAny<List<GetPermissionsDto>>()), Occurs.Once());
        }

        [Fact]
        public async Task GetRoleByIdAsyncThrowsTQAuthenticationExceptionWhenRoleNotFound()
        {
            // Arrange
            Mock.Arrange(() => rolesRepository.GetByIdIncludeRolePermissionsAync(Arg.AnyGuid)).TaskResult(null);
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<TQAuthenticationException>(() => sut.GetRoleByIdAsync(Guid.NewGuid()));
            Mock.Assert(() => rolesRepository.GetByIdIncludeRolePermissionsAync(Arg.AnyGuid), Occurs.Once());
        }

        [Fact]
        public async Task GetRoleByIdAsyncThrowsExceptionWhenGroupNotFound()
        {
            // Arrange
            Mock.Arrange(() => rolesRepository.GetByIdIncludeRolePermissionsAync(Arg.AnyGuid)).TaskResult(new Role { Id = Guid.NewGuid(), GroupId = Guid.NewGuid() });
            Mock.Arrange(() => groupManager.GetGroupByIdAsync(Arg.AnyGuid)).Throws<ArgumentException>();

            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.GetRoleByIdAsync(Guid.NewGuid()));
            Mock.Assert(() => rolesRepository.GetByIdIncludeRolePermissionsAync(Arg.AnyGuid), Occurs.Once());
            Mock.Assert(() => groupManager.GetGroupByIdAsync(Arg.AnyGuid), Occurs.Once());
        }

        [Fact]
        public async Task GetRoleByIdAsyncThrowsGroupManagerExceptionWhenAzureGroupNotFound()
        {
            // Arrange
            Mock.Arrange(() => rolesRepository.GetByIdIncludeRolePermissionsAync(Arg.AnyGuid)).TaskResult(new Role { Id = Guid.NewGuid(), GroupId = Guid.NewGuid() });
            Mock.Arrange(() => groupManager.GetGroupByIdAsync(Arg.AnyGuid)).TaskResult(new Group { Id = Guid.NewGuid() });
            Mock.Arrange(() => azureGroupManager.GetByIdAsync(Arg.AnyGuid)).Throws<ArgumentException>();

            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.GetRoleByIdAsync(Guid.NewGuid()));
            Mock.Assert(() => rolesRepository.GetByIdIncludeRolePermissionsAync(Arg.AnyGuid), Occurs.Once());
            Mock.Assert(() => azureGroupManager.GetByIdAsync(Arg.AnyGuid), Occurs.Once());
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