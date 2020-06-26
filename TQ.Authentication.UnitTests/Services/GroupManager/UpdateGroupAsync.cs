using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.Core.Requests.Groups;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.Data.Repositories;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.ExternalServices.GraphAPI.Models;
using TQ.Authentication.Services.Interfaces;
using TQ.Authentication.Services.Interfaces.Converters;
using Xunit;

namespace TQ.Authentication.UnitTests.Services.GroupManager
{
    public class UpdateGroupAsync
    {
        [Fact]
        public async Task UpdateGroupAsync_UpdatesGroup()
        {
            // Arrange
            var groupId = Guid.NewGuid();

            var graphApiClient = Mock.Create<IGraphApiClient>();
            Mock.Arrange(() => graphApiClient.Groups.UpdateGroupAsync(Arg.IsAny<AzureGroup>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var groupsRepository = Mock.Create<IGroupsRepository<Group>>();
            Mock.Arrange(() => groupsRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Group, bool>>>()))
                .TaskResult(true);

            Mock.Arrange(() => groupsRepository.UpdateGroupAsync(Arg.IsAny<Group>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var userGroupsRepository = Mock.Create<IUserGroupsRepository<UserGroup>>();
            Mock.Arrange(() => userGroupsRepository.CreateAsync(Arg.IsAny<UserGroup>()))
                .TaskResult(groupId);

            var roleRepository = Mock.Create<IRolesRepository<Role>>();
            var roleConverter = Mock.Create<IRoleConverter>();
            var azureGroupManager = Mock.Create<IAzureGroupManager>();
            var permissionManager = Mock.Create<IPermissionManager>();
            var permissionConverter = Mock.Create<IPermissionConverter>();

            var groupManager = new Authentication.Services.GroupManager(graphApiClient, groupsRepository, userGroupsRepository, roleRepository, roleConverter, azureGroupManager, permissionManager, permissionConverter);

            var updateGroupRequest = new UpdateGroupRequest
            {
                Name = "Group Display name",
                Description = "Group Description",
                ServiceUrl = "https://localhost",
                IsActive = true
            };

            // Act
            await groupManager.UpdateGroupAsync(groupId, updateGroupRequest);

            Mock.Assert(() => graphApiClient.Groups.UpdateGroupAsync(Arg.IsAny<AzureGroup>()), Occurs.Once());
            Mock.Assert(() => groupsRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<Group, bool>>>()),
                Occurs.Once());
        }

        [Fact]
        public async Task UpdateGroupAsync_GroupIdNotExistInSql_ThrowsException()
        {
            var groupId = Guid.NewGuid();

            var graphApiClient = Mock.Create<IGraphApiClient>();
            Mock.Arrange(() => graphApiClient.Groups.UpdateGroupAsync(Arg.IsAny<AzureGroup>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var groupsRepository = Mock.Create<IGroupsRepository<Group>>();
            Mock.Arrange(() => groupsRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Group, bool>>>()))
                .TaskResult(false);

            var userGroupsRepository = Mock.Create<IUserGroupsRepository<UserGroup>>();
            Mock.Arrange(() => userGroupsRepository.CreateAsync(Arg.IsAny<UserGroup>()))
                .TaskResult(groupId);

            var roleRepository = Mock.Create<IRolesRepository<Role>>();
            var roleConverter = Mock.Create<IRoleConverter>();
            var azureGroupManager = Mock.Create<IAzureGroupManager>();
            var permissionManager = Mock.Create<IPermissionManager>();
            var permissionConverter = Mock.Create<IPermissionConverter>();

            var groupManager = new Authentication.Services.GroupManager(graphApiClient, groupsRepository, userGroupsRepository, roleRepository, roleConverter, azureGroupManager, permissionManager, permissionConverter);

            var updateGroupRequest = new UpdateGroupRequest
            {
                Name = "Group Display name",
                Description = "Group Description",
                ServiceUrl = "https://localhost",
                IsActive = true
            };

            // Should throw an exception
            await Assert.ThrowsAsync<TQAuthenticationException>(() => groupManager.UpdateGroupAsync(groupId, updateGroupRequest));

            Mock.Assert(() => graphApiClient.Groups.UpdateGroupAsync(Arg.IsAny<AzureGroup>()), Occurs.Once());
            Mock.Assert(() => groupsRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<Group, bool>>>()),
                Occurs.Once());
        }

        [Fact]
        public async Task UpdateGroupAsync_GraphApiFails_ThrowsException()
        {
            var groupId = Guid.NewGuid();

            var groupGraph = Mock.Create<IGroupGraph>();
            Mock.Arrange(() => groupGraph.UpdateGroupAsync(Arg.IsAny<AzureGroup>()))
                .Throws<Exception>();

            var graphApiClient = Mock.Create<IGraphApiClient>();
            Mock.Arrange(() => graphApiClient.Groups)
                .Returns(groupGraph);

            var groupsRepository = Mock.Create<IGroupsRepository<Group>>();
            var userGroupsRepository = Mock.Create<IUserGroupsRepository<UserGroup>>();

            var roleRepository = Mock.Create<IRolesRepository<Role>>();
            var roleConverter = Mock.Create<IRoleConverter>();
            var azureGroupManager = Mock.Create<IAzureGroupManager>();
            var permissionManager = Mock.Create<IPermissionManager>();
            var permissionConverter = Mock.Create<IPermissionConverter>();

            var groupManager = new Authentication.Services.GroupManager(graphApiClient, groupsRepository, userGroupsRepository, roleRepository, roleConverter, azureGroupManager, permissionManager, permissionConverter);

            var updateGroupRequest = new UpdateGroupRequest
            {
                Name = "Group Display name",
                Description = "Group Description",
                ServiceUrl = "https://localhost",
                IsActive = true
            };

            // Should throw an exception
            await Assert.ThrowsAsync<Exception>(() => groupManager.UpdateGroupAsync(groupId, updateGroupRequest));

            Mock.Assert(() => groupGraph.UpdateGroupAsync(Arg.IsAny<AzureGroup>()), Occurs.Once());
            Mock.Assert(() => graphApiClient.Groups, Occurs.Once());
            Mock.Assert(() => groupsRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<Group, bool>>>()),
                Occurs.Never());
        }
    }
}