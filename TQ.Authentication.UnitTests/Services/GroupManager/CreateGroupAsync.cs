using System;
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
    public class CreateGroupAsync
    {
        [Fact]
        public async Task CreateGroupAsync_ReturnsGroupsDto()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            var createdAt = DateTime.Now;

            var groupGraph = Mock.Create<IGroupGraph>();
            Mock.Arrange(() => groupGraph.CreateGroupAsync(Arg.IsAny<AzureGroup>()))
                .TaskResult(new AzureGroup
                {
                    Id = groupId,
                    Name = "Group Display name",
                    Description = "Group Description",
                    CreatedAt = createdAt
                });

            Mock.Arrange(() => groupGraph.GetGroupByNameAsync(Arg.IsAny<string>()))
                .TaskResult(Arg.IsNull<AzureGroup>());

            var graphApiClient = Mock.Create<IGraphApiClient>();
            Mock.Arrange(() => graphApiClient.Groups)
                .Returns(groupGraph);

            var groupsRepository = Mock.Create<IGroupsRepository<Group>>();
            Mock.Arrange(() => groupsRepository.CreateAsync(Arg.IsAny<Group>()))
                .TaskResult(groupId);

            var userGroupsRepository = Mock.Create<IUserGroupsRepository<UserGroup>>();
            Mock.Arrange(() => userGroupsRepository.CreateAsync(Arg.IsAny<UserGroup>()))
                .TaskResult(groupId);

            var roleRepository = Mock.Create<IRolesRepository<Role>>();
            var roleConverter = Mock.Create<IRoleConverter>();
            var azureGroupManager = Mock.Create<IAzureGroupManager>();
            var permissionManager = Mock.Create<IPermissionManager>();
            var permissionConverter = Mock.Create<IPermissionConverter>();

            var groupManager =
                new Authentication.Services.GroupManager(graphApiClient, groupsRepository, userGroupsRepository, roleRepository, roleConverter, azureGroupManager, permissionManager, permissionConverter);

            var group = new CreateGroupRequest
            {
                Name = "Group Display name",
                Description = "Group Description",
                ServiceUrl = "https://localhost",
                IsActive = true
            };

            // Act
            var newGroup = await groupManager.CreateGroupAsync(group);

            // Assert
            Assert.Equal(groupId, newGroup.Id);
            Assert.Equal(newGroup.Name, newGroup.Name);
            Assert.Equal(newGroup.Description, newGroup.Description);
            Assert.Equal(newGroup.CreatedAt, newGroup.CreatedAt);

            Mock.Assert(() => graphApiClient.Groups, Occurs.AtLeastOnce());
            Mock.Assert(() => groupGraph.CreateGroupAsync(Arg.IsAny<AzureGroup>()), Occurs.Once());
            Mock.Assert(() => groupsRepository.CreateAsync(Arg.IsAny<Group>()), Occurs.Once());
            Mock.Assert(() => userGroupsRepository.CreateAsync(Arg.IsAny<UserGroup>()), Occurs.Never());
        }

        [Fact]
        public async Task CreateGroupAsync_GroupAlreadyExists_ThrowsException()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            var createdAt = DateTime.Now;

            var groupGraph = Mock.Create<IGroupGraph>();
            Mock.Arrange(() => groupGraph.CreateGroupAsync(Arg.IsAny<AzureGroup>()))
                .TaskResult(new AzureGroup
                {
                    Id = groupId,
                    Name = "Group Display name",
                    Description = "Group Description",
                    CreatedAt = createdAt
                });

            Mock.Arrange(() => groupGraph.GetGroupByNameAsync(Arg.IsAny<string>()))
                .TaskResult(new AzureGroup
                {
                    Name = "Group Display name",
                    Description = "Group Description",
                    CreatedAt = createdAt
                });

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

            var group = new CreateGroupRequest
            {
                Name = "Group Display name",
                Description = "Group Description",
                ServiceUrl = "https://localhost",
                IsActive = true
            };

            // Act & Assert
            await Assert.ThrowsAsync<TQAuthenticationException>(() => groupManager.CreateGroupAsync(group));

            Mock.Assert(() => graphApiClient.Groups, Occurs.Once());
            Mock.Assert(() => groupGraph.CreateGroupAsync(Arg.IsAny<AzureGroup>()), Occurs.Never());
            Mock.Assert(() => groupGraph.GetGroupByNameAsync(Arg.IsAny<string>()), Occurs.Once());
        }

        [Fact]
        public async Task CreateGroupAsync_GraphApiFails_ThrowsException()
        {
            // Arrange
            var groupGraph = Mock.Create<IGroupGraph>();
            Mock.Arrange(() => groupGraph.CreateGroupAsync(Arg.IsAny<AzureGroup>()))
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

            var group = new CreateGroupRequest
            {
                Name = "Group Name",
                Description = "Group Description",
                ServiceUrl = "https://localhost",
                IsActive = true
            };

            // Act & Assert

            // Should throw an exception
            await Assert.ThrowsAsync<TQAuthenticationException>(() => groupManager.CreateGroupAsync(group));

            Mock.Assert(() => graphApiClient.Groups, Occurs.Once());
            Mock.Assert(() => groupGraph.CreateGroupAsync(Arg.IsAny<AzureGroup>()), Occurs.Never());
            Mock.Assert(() => groupsRepository.CreateAsync(Arg.IsAny<Group>()), Occurs.Never());
        }

        [Fact]
        public async Task CreateGroupAsync_SqlFailsCreateGroup_AzureCleanupPerformed()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            var createdAt = DateTime.Now;

            var groupGraph = Mock.Create<IGroupGraph>();
            Mock.Arrange(() => groupGraph.CreateGroupAsync(Arg.IsAny<AzureGroup>()))
                .TaskResult(new AzureGroup
                {
                    Id = groupId,
                    Name = "Group Display name",
                    Description = "Group Description",
                    CreatedAt = createdAt
                });

            Mock.Arrange(() => groupGraph.DeleteGroupAsync(Arg.IsAny<Guid>()));

            var graphApiClient = Mock.Create<IGraphApiClient>();
            Mock.Arrange(() => graphApiClient.Groups)
                .Returns(groupGraph);

            var groupsRepository = Mock.Create<IGroupsRepository<Group>>();
            Mock.Arrange(() => groupsRepository.CreateAsync(Arg.IsAny<Group>()))
                .Throws<Exception>();

            var userGroupsRepository = Mock.Create<IUserGroupsRepository<UserGroup>>();

            var roleRepository = Mock.Create<IRolesRepository<Role>>();
            var roleConverter = Mock.Create<IRoleConverter>();
            var azureGroupManager = Mock.Create<IAzureGroupManager>();
            var permissionManager = Mock.Create<IPermissionManager>();
            var permissionConverter = Mock.Create<IPermissionConverter>();

            var groupManager = new Authentication.Services.GroupManager(graphApiClient, groupsRepository, userGroupsRepository, roleRepository, roleConverter, azureGroupManager, permissionManager, permissionConverter);

            var group = new CreateGroupRequest
            {
                Name = "Group Name",
                Description = "Group Description",
                ServiceUrl = "https://localhost",
                IsActive = true
            };

            // Act & Assert
            await Assert.ThrowsAsync<TQAuthenticationException>(() => groupManager.CreateGroupAsync(group));

            // Group deletion from Azure AD should be performed once
            Mock.Assert(() => graphApiClient.Groups, Occurs.Once());
            Mock.Assert(() => groupGraph.DeleteGroupAsync(Arg.IsAny<Guid>()), Occurs.Never());
        }
    }
}