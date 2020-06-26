using System;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.Data.Repositories;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.Services.Interfaces;
using TQ.Authentication.Services.Interfaces.Converters;
using Xunit;

namespace TQ.Authentication.UnitTests.Services.GroupManager
{
    public class RemoveUserAsync
    {
        [Fact]
        public async Task RemoveUserAsync_RemovesUser()
        {
            // Arrange

            // Graph
            var groupGraph = Mock.Create<IGroupGraph>();
            var userGraph = Mock.Create<IUserGraph>();

            Mock.Arrange(() => groupGraph.GroupExistsAsync(Arg.IsAny<Guid>()))
                .TaskResult(true);

            Mock.Arrange(() => userGraph.UserExistsAsync(Arg.IsAny<Guid>()))
                .TaskResult(true);

            Mock.Arrange(() => groupGraph.UserExistsInGroupAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .TaskResult(true);

            Mock.Arrange(() => groupGraph.RemoveUserAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var graphApiClient = Mock.Create<IGraphApiClient>();
            Mock.Arrange(() => graphApiClient.Groups)
                .Returns(groupGraph);
            Mock.Arrange(() => graphApiClient.Users)
                .Returns(userGraph);

            // Repositories
            var groupsRepository = Mock.Create<IGroupsRepository<Group>>();

            var userGroupsRepository = Mock.Create<IUserGroupsRepository<UserGroup>>();
            Mock.Arrange(() => userGroupsRepository.RemoveUserGroupAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var roleRepository = Mock.Create<IRolesRepository<Role>>();
            var roleConverter = Mock.Create<IRoleConverter>();
            var azureGroupManager = Mock.Create<IAzureGroupManager>();
            var permissionManager = Mock.Create<IPermissionManager>();
            var permissionConverter = Mock.Create<IPermissionConverter>();


            // Initialise GroupManager
            var groupManager =
                new Authentication.Services.GroupManager(graphApiClient, groupsRepository, userGroupsRepository, roleRepository, roleConverter, azureGroupManager, permissionManager, permissionConverter);

            // Act
            await groupManager.RemoveUserAsync(Guid.NewGuid(), Guid.NewGuid());

            // Assert
            Mock.Assert(() => graphApiClient.Groups, Occurs.AtLeastOnce());
            Mock.Assert(() => graphApiClient.Users, Occurs.AtLeastOnce());

            Mock.Assert(() => groupGraph.RemoveUserAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<Guid>()), Occurs.Once());

            Mock.Assert(() => groupGraph.RemoveUserAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<Guid>()), Occurs.Once());
        }

        [Fact]
        public async Task RemoveUserAsync_GraphFails_ThrowsException()
        {
            // Arrange

            // Graph
            var groupGraph = Mock.Create<IGroupGraph>();
            var userGraph = Mock.Create<IUserGraph>();

            Mock.Arrange(() => groupGraph.GroupExistsAsync(Arg.IsAny<Guid>()))
                .TaskResult(true);

            Mock.Arrange(() => userGraph.UserExistsAsync(Arg.IsAny<Guid>()))
                .TaskResult(true);

            Mock.Arrange(() => groupGraph.UserExistsInGroupAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .Throws<Exception>();

            Mock.Arrange(() => groupGraph.RemoveUserAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var graphApiClient = Mock.Create<IGraphApiClient>();
            Mock.Arrange(() => graphApiClient.Groups)
                .Returns(groupGraph);
            Mock.Arrange(() => graphApiClient.Users)
                .Returns(userGraph);

            // Repositories
            var groupsRepository = Mock.Create<IGroupsRepository<Group>>();

            var userGroupsRepository = Mock.Create<IUserGroupsRepository<UserGroup>>();
            Mock.Arrange(() => userGroupsRepository.RemoveUserGroupAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var roleRepository = Mock.Create<IRolesRepository<Role>>();
            var roleConverter = Mock.Create<IRoleConverter>();
            var azureGroupManager = Mock.Create<IAzureGroupManager>();
            var permissionManager = Mock.Create<IPermissionManager>();
            var permissionConverter = Mock.Create<IPermissionConverter>();

            // Initialise GroupManager
            var groupManager = new Authentication.Services.GroupManager(graphApiClient, groupsRepository, userGroupsRepository, roleRepository, roleConverter, azureGroupManager, permissionManager, permissionConverter);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => groupManager.RemoveUserAsync(Guid.NewGuid(), Guid.NewGuid()));

            Mock.Assert(() => graphApiClient.Groups, Occurs.AtLeastOnce());
            Mock.Assert(() => graphApiClient.Users, Occurs.AtLeastOnce());

            Mock.Assert(() => groupGraph.RemoveUserAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<Guid>()), Occurs.Never());

            Mock.Assert(() => groupGraph.RemoveUserAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<Guid>()), Occurs.Never());
        }
    }
}