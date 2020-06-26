using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Telerik.JustMock;
using TQ.Authentication.Core.Dto.Permissions;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.Data.Repositories;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.Services.Interfaces;
using TQ.Authentication.Services.Interfaces.Converters;
using Xunit;

namespace TQ.Authentication.UnitTests.Services.GroupManager
{
    public class GetGroupPermissionsAsync
    {
        private readonly IGraphApiClient mockGraphApiClient = Mock.Create<IGraphApiClient>();
        private readonly IGroupsRepository<Group> mockGroupsRepository = Mock.Create<IGroupsRepository<Group>>();
        private readonly IUserGroupsRepository<UserGroup> mockUserGroupsRepository = Mock.Create<IUserGroupsRepository<UserGroup>>();
        private readonly IRolesRepository<Role> mockRolesRepository = Mock.Create<IRolesRepository<Role>>();
        private readonly IRoleConverter mockRoleConverter = Mock.Create<IRoleConverter>();
        private readonly IAzureGroupManager mockAzureGroupManager = Mock.Create<IAzureGroupManager>();
        private readonly IPermissionManager mockPermissionManager = Mock.Create<IPermissionManager>();
        private readonly IPermissionConverter mockPermissionConverter = Mock.Create<IPermissionConverter>();

        private const string permissionName = "PermissionName";
        private const string permissionDescription = "Permission Description";
        private const string childPermissionName = "ChildPermissionName";
        private const string childPermissionDescription = "Child Permission Description";

        [Fact]
        public async Task GetGroupPermissionsAsyncSuccess()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            var permissionId = Guid.NewGuid();
            var childPermissionId = Guid.NewGuid();
            var roleId = Guid.NewGuid();
            var permission = new Permission { Id = permissionId, Name = permissionName, Description = permissionDescription, GroupId = groupId, ParentPermissionId = null };
            var childPermission = new Permission { Id = childPermissionId, Name = permissionName, Description = permissionDescription, GroupId = groupId, ParentPermissionId = permissionId };
            var roles = new List<Role> { new Role() { Id = roleId } };
            var rolePermissions = new List<RolePermission> { new RolePermission() { Id = Guid.NewGuid(), Permission = permission } };
            var rolesAndPermissions = new List<Role> { new Role() { Id = roleId, RolePermissions = rolePermissions } };

            var expectedResult = new List<GetPermissionsDto>()
            {
                new GetPermissionsDto()
                {
                    Id = permissionId,
                    Name = permissionName,
                    Description = permissionDescription,
                    Children = new List<GetPermissionsDto>()
                    {
                        new GetPermissionsDto()
                        {
                            Id = childPermissionId,
                            Name = childPermissionName,
                            Description = childPermissionDescription,
                            Children = new List<GetPermissionsDto>()
                        }
                    }
                }
            };

            Mock.Arrange(() => mockRolesRepository.GetRolesForGroupAsync(groupId)).TaskResult(roles);
            Mock.Arrange(() => mockRolesRepository.GetByIdsIncludeRolePermissionsAsync(roles.Select(x => x.Id).ToList())).TaskResult(rolesAndPermissions);
            Mock.Arrange(() => mockPermissionManager.GetDistinctPermissionsForRoles(rolesAndPermissions)).TaskResult(new List<Permission>() { permission, childPermission });
            Mock.Arrange(() => mockPermissionConverter.GetNestedPermissionsDto(Arg.IsAny<IEnumerable<IPermissionTree>>())).Returns(expectedResult);

            var sut = CreateSut();

            // Act
            var result = await sut.GetGroupPermissionsAsync(groupId);

            // Assert
            Mock.Assert(() => mockRolesRepository.GetRolesForGroupAsync(groupId), Occurs.Once());
            Mock.Assert(() => mockRolesRepository.GetByIdsIncludeRolePermissionsAsync(roles.Select(x => x.Id).ToList()), Occurs.Once());
            Mock.Assert(() => mockPermissionManager.GetDistinctPermissionsForRoles(rolesAndPermissions), Occurs.Once());
            Mock.Assert(() => mockPermissionConverter.GetNestedPermissionsDto(Arg.IsAny<IEnumerable<IPermissionTree>>()), Occurs.Once());

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(List<GetPermissionsDto>));
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetGroupPermissionsAsyncReturnsEmptyWhenNoRoles()
        {
            // Arrange
            var groupId = Guid.NewGuid();

            var expectedResult = new List<GetPermissionsDto>();

            Mock.Arrange(() => mockRolesRepository.GetRolesForGroupAsync(groupId)).TaskResult(new List<Role>().AsEnumerable());

            var sut = CreateSut();

            // Act
            var result = await sut.GetGroupPermissionsAsync(groupId);

            // Assert
            Mock.Assert(() => mockRolesRepository.GetRolesForGroupAsync(groupId), Occurs.Once());

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(List<GetPermissionsDto>));
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetGroupPermissionsAsyncReturnsEmptyWhenNoRolePermissions()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            var roleId = Guid.NewGuid();
            var roles = new List<Role> { new Role() { Id = roleId } };

            var expectedResult = new List<GetPermissionsDto>();

            Mock.Arrange(() => mockRolesRepository.GetRolesForGroupAsync(groupId)).TaskResult(roles);
            Mock.Arrange(() => mockRolesRepository.GetByIdsIncludeRolePermissionsAsync(roles.Select(x => x.Id).ToList())).TaskResult(new List<Role>().AsEnumerable());

            var sut = CreateSut();

            // Act
            var result = await sut.GetGroupPermissionsAsync(groupId);

            // Assert
            Mock.Assert(() => mockRolesRepository.GetRolesForGroupAsync(groupId), Occurs.Once());
            Mock.Assert(() => mockRolesRepository.GetByIdsIncludeRolePermissionsAsync(roles.Select(x => x.Id).ToList()), Occurs.Once());

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(List<GetPermissionsDto>));
            result.Should().BeEquivalentTo(expectedResult);
        }

        private Authentication.Services.GroupManager CreateSut()
        {
            return new Authentication.Services.GroupManager(mockGraphApiClient,
                                                            mockGroupsRepository,
                                                            mockUserGroupsRepository,
                                                            mockRolesRepository,
                                                            mockRoleConverter,
                                                            mockAzureGroupManager,
                                                            mockPermissionManager,
                                                            mockPermissionConverter);
        }
    }
}