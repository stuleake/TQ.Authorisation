using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Dto.Roles;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.Data.Repositories;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.ExternalServices.GraphAPI.Models;
using TQ.Authentication.Services.Interfaces;
using TQ.Authentication.Services.Interfaces.Converters;
using Xunit;

namespace TQ.Authentication.UnitTests.Services.GroupManager
{
    public class GetGroupAsync
    {
        [Fact]
        public async Task GetGroup_ReturnsSingleGroup()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            var azureGroupManager = Mock.Create<IAzureGroupManager>();
            Mock.Arrange(() => azureGroupManager.GetByIdAsync(groupId)).TaskResult(new AzureGroup
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Description = "description",
                CreatedAt = DateTime.Now
            });

            var groupsRepository = Mock.Create<IGroupsRepository<Authentication.Data.Entities.Group>>();
            Mock.Arrange(() => groupsRepository.GetAsync(groupId)).TaskResult(new Group { Id = groupId, IsActive = false, ServiceUrl = "service url" });

            var roleRepository = Mock.Create<IRolesRepository<Role>>();
            Mock.Arrange(() => roleRepository.GetRolesForGroupAsync(Arg.AnyGuid)).TaskResult(new List<Role> { new Role { Id = Guid.NewGuid() } });

            var roleConverter = Mock.Create<IRoleConverter>();
            Mock.Arrange(() => roleConverter.ToRoleGetDtos(Arg.IsAny<IEnumerable<Role>>())).Returns(Enumerable.Empty<GetRoleDto>());

            var userGroupsRepository = Mock.Create<IUserGroupsRepository<UserGroup>>();
            var graphApiClient = Mock.Create<IGraphApiClient>();
            var permissionManager = Mock.Create<IPermissionManager>();
            var permissionConverter = Mock.Create<IPermissionConverter>();

            var groupManager = new Authentication.Services.GroupManager(graphApiClient,
                                                                        groupsRepository,
                                                                        userGroupsRepository,
                                                                        roleRepository,
                                                                        roleConverter,
                                                                        azureGroupManager,
                                                                        permissionManager,
                                                                        permissionConverter);

            // Act
            var group = await groupManager.GetGroupAsync(groupId);

            // Assert
            Assert.NotNull(group);
            Mock.Assert(() => azureGroupManager.GetByIdAsync(groupId), Occurs.Once());
            Mock.Assert(() => groupsRepository.GetAsync(groupId), Occurs.Once());
            Mock.Assert(() => roleConverter.ToRoleGetDtos(Arg.IsAny<IEnumerable<Role>>()), Occurs.Once());
        }

        [Fact]
        public async Task GetGroup_GroupDoesNotExistInSql_ReturnsException()
        {
            // Arrange
            var groupId = Guid.NewGuid();

            var azureGroupManager = Mock.Create<IAzureGroupManager>();
            Mock.Arrange(() => azureGroupManager.GetByIdAsync(Arg.AnyGuid)).TaskResult(new AzureGroup { Id = Guid.NewGuid() });

            var groupsRepository = Mock.Create<IGroupsRepository<Authentication.Data.Entities.Group>>();
            Mock.Arrange(() => groupsRepository.GetAsync(groupId))
                .TaskResult(Arg.IsNull<Authentication.Data.Entities.Group>());

            var userGroupsRepository = Mock.Create<IUserGroupsRepository<UserGroup>>();

            var roleRepository = Mock.Create<IRolesRepository<Role>>();
            var roleConverter = Mock.Create<IRoleConverter>();
            var graphApiClient = Mock.Create<IGraphApiClient>();
            var permissionManager = Mock.Create<IPermissionManager>();
            var permissionConverter = Mock.Create<IPermissionConverter>();

            var groupManager = new Authentication.Services.GroupManager(graphApiClient, groupsRepository, userGroupsRepository, roleRepository, roleConverter, azureGroupManager, permissionManager, permissionConverter);

            // Act & Assert
            await Assert.ThrowsAsync<TQAuthenticationException>(() => groupManager.GetGroupAsync(groupId));
            Mock.Assert(() => azureGroupManager.GetByIdAsync(Arg.AnyGuid), Occurs.Once());
            Mock.Assert(() => groupsRepository.GetAsync(groupId), Occurs.Once());
        }

        [Fact]
        public async Task GetGroup_GroupDoesNotExistInAzure_ReturnsException()
        {
            // Arrange
            var graphApiClient = Mock.Create<IGraphApiClient>();
            var groupsRepository = Mock.Create<IGroupsRepository<Authentication.Data.Entities.Group>>();
            var userGroupsRepository = Mock.Create<IUserGroupsRepository<UserGroup>>();
            var roleRepository = Mock.Create<IRolesRepository<Role>>();
            var roleConverter = Mock.Create<IRoleConverter>();

            var azureGroupManager = Mock.Create<IAzureGroupManager>();
            Mock.Arrange(() => azureGroupManager.GetByIdAsync(Arg.AnyGuid)).Throws<Exception>();
            var permissionManager = Mock.Create<IPermissionManager>();
            var permissionConverter = Mock.Create<IPermissionConverter>();

            // Act
            var groupManager = new Authentication.Services.GroupManager(graphApiClient, groupsRepository, userGroupsRepository, roleRepository, roleConverter, azureGroupManager, permissionManager, permissionConverter);

            // Assert
            await Assert.ThrowsAsync<Exception>(() => groupManager.GetGroupAsync(Guid.NewGuid()));
            Mock.Assert(() => azureGroupManager.GetByIdAsync(Arg.AnyGuid), Occurs.Once());
        }
    }
}