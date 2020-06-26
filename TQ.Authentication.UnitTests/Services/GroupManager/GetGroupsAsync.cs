using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.Data.Repositories;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.ExternalServices.GraphAPI.Models;
using TQ.Authentication.Services.Interfaces;
using TQ.Authentication.Services.Interfaces.Converters;
using Xunit;

namespace TQ.Authentication.UnitTests.Services.GroupManager
{
    public class GetGroupsAsync
    {
        [Fact]
        public async Task GetGroups_ReturnsGroupCollection()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            var createdAt = DateTime.Now;

            var groupGraph = Mock.Create<IGroupGraph>();
            Mock.Arrange(() => groupGraph.GetGroupsAsync())
                .TaskResult(new List<AzureGroup>
                {
                    new AzureGroup
                    {
                        Id = groupId,
                        Name = "Azure Group",
                        Description = "Azure Group Description",
                        CreatedAt = createdAt
                    }
                });

            var graphApiClient = Mock.Create<IGraphApiClient>();
            Mock.Arrange(() => graphApiClient.Groups)
                .Returns(groupGraph);

            var groupsRepository = Mock.Create<IGroupsRepository<Group>>();
            Mock.Arrange(() => groupsRepository.GetAllAsync())
                .TaskResult(new List<Group>
                {
                    new Group
                    {
                        Id = groupId
                    }
                });

            var userGroupsRepository = Mock.Create<IUserGroupsRepository<UserGroup>>();
            Mock.Arrange(() => userGroupsRepository.CreateAsync(Arg.IsAny<UserGroup>()))
                .TaskResult(groupId);

            var roleRepository = Mock.Create<IRolesRepository<Role>>();
            var roleConverter = Mock.Create<IRoleConverter>();
            var azureGroupManager = Mock.Create<IAzureGroupManager>();
            var permissionManager = Mock.Create<IPermissionManager>();
            var permissionConverter = Mock.Create<IPermissionConverter>();

            var groupManager = new Authentication.Services.GroupManager(graphApiClient, groupsRepository, userGroupsRepository, roleRepository, roleConverter, azureGroupManager, permissionManager, permissionConverter);

            // Act
            var groups = await groupManager.GetGroupsAsync();

            // Assert
            Assert.Single(groups);

            Mock.Assert(() => groupGraph.GetGroupsAsync(), Occurs.Once());
            Mock.Assert(() => graphApiClient.Groups, Occurs.Once());
            Mock.Assert(() => groupsRepository.GetAllAsync(), Occurs.Once());
        }

        [Fact]
        public async Task GetGroups_GraphApiFails_ThrowsException()
        {
            // Arrange
            var groupId = Guid.NewGuid();

            var groupGraph = Mock.Create<IGroupGraph>();
            Mock.Arrange(() => groupGraph.GetGroupsAsync())
                .Throws<ArgumentNullException>();

            var graphApiClient = Mock.Create<IGraphApiClient>();
            Mock.Arrange(() => graphApiClient.Groups)
                .Returns(groupGraph);

            var groupsRepository = Mock.Create<IGroupsRepository<Group>>();
            Mock.Arrange(() => groupsRepository.GetAllAsync())
                .TaskResult(new List<Group>
                {
                    new Group
                    {
                        Id = groupId
                    }
                });

            var userGroupsRepository = Mock.Create<IUserGroupsRepository<UserGroup>>();
            Mock.Arrange(() => userGroupsRepository.CreateAsync(Arg.IsAny<UserGroup>()))
                .TaskResult(groupId);

            var roleRepository = Mock.Create<IRolesRepository<Role>>();
            var roleConverter = Mock.Create<IRoleConverter>();
            var azureGroupManager = Mock.Create<IAzureGroupManager>();
            var permissionManager = Mock.Create<IPermissionManager>();
            var permissionConverter = Mock.Create<IPermissionConverter>();

            var groupManager = new Authentication.Services.GroupManager(graphApiClient, groupsRepository, userGroupsRepository, roleRepository, roleConverter, azureGroupManager, permissionManager, permissionConverter);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => groupManager.GetGroupsAsync());

            Mock.Assert(() => groupGraph.GetGroupsAsync(), Occurs.Once());
            Mock.Assert(() => graphApiClient.Groups, Occurs.Once());
            Mock.Assert(() => groupsRepository.GetAllAsync(), Occurs.Never());
        }
    }
}