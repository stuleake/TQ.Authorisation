using System;
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

namespace TQ.Authentication.UnitTests.Services.GroupManager
{
    public class AddUserAsync
    {
        [Fact]
        public async Task AddUserAsync_AddsUser()
        {
            // Arrange
            var userGroupId = Guid.NewGuid();

            // Graph
            var groupGraph = Mock.Create<IGroupGraph>();
            var userGraph = Mock.Create<IUserGraph>();

            Mock.Arrange(() => groupGraph.UserExistsInGroupAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .TaskResult(false);

            Mock.Arrange(() => groupGraph.GroupExistsAsync(Arg.IsAny<Guid>()))
                .TaskResult(true);

            Mock.Arrange(() => userGraph.UserExistsAsync(Arg.IsAny<Guid>()))
                .TaskResult(true);

            Mock.Arrange(() => groupGraph.AddUserAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var graphApiClient = Mock.Create<IGraphApiClient>();
            Mock.Arrange(() => graphApiClient.Groups)
                .Returns(groupGraph);
            Mock.Arrange(() => graphApiClient.Users)
                .Returns(userGraph);

            // Repositories
            var groupsRepository = Mock.Create<IGroupsRepository<Group>>();

            var userGroupsRepository = Mock.Create<IUserGroupsRepository<UserGroup>>();
            Mock.Arrange(() => userGroupsRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<UserGroup, bool>>>()))
                .TaskResult(false);

            Mock.Arrange(() => userGroupsRepository.CreateAsync(Arg.IsAny<UserGroup>()))
                .TaskResult(userGroupId);

            var roleRepository = Mock.Create<IRolesRepository<Role>>();
            var roleConverter = Mock.Create<IRoleConverter>();
            var azureGroupManager = Mock.Create<IAzureGroupManager>();
            var permissionManager = Mock.Create<IPermissionManager>();
            var permissionConverter = Mock.Create<IPermissionConverter>();

            // Initialise GroupManager
            var groupManager = new Authentication.Services.GroupManager(graphApiClient,
                                                                        groupsRepository,
                                                                        userGroupsRepository,
                                                                        roleRepository,
                                                                        roleConverter,
                                                                        azureGroupManager,
                                                                        permissionManager,
                                                                        permissionConverter);

            // Act
            await groupManager.AddUserAsync(Guid.NewGuid(), Guid.NewGuid());

            // Assert
            Mock.Assert(() => graphApiClient.Groups, Occurs.AtLeastOnce());
            Mock.Assert(() => graphApiClient.Users, Occurs.AtLeastOnce());
            Mock.Assert(() => groupGraph.AddUserAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()), Occurs.Once());

            Mock.Assert(() => userGroupsRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<UserGroup, bool>>>()), Occurs.Once());

            Mock.Assert(() => userGroupsRepository.CreateAsync(
                Arg.IsAny<UserGroup>()), Occurs.Once());
        }

        [Fact]
        public async Task AddUserAsync_UserNotExistInAD_ThrowsException()
        {
            // Arrange
            var userGroupId = Guid.NewGuid();

            // Graph
            var groupGraph = Mock.Create<IGroupGraph>();
            var userGraph = Mock.Create<IUserGraph>();

            Mock.Arrange(() => groupGraph.UserExistsInGroupAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .TaskResult(false);

            Mock.Arrange(() => groupGraph.GroupExistsAsync(Arg.IsAny<Guid>()))
                .TaskResult(true);

            Mock.Arrange(() => userGraph.UserExistsAsync(Arg.IsAny<Guid>()))
                .TaskResult(false);

            Mock.Arrange(() => groupGraph.AddUserAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var graphApiClient = Mock.Create<IGraphApiClient>();
            Mock.Arrange(() => graphApiClient.Groups)
                .Returns(groupGraph);
            Mock.Arrange(() => graphApiClient.Users)
                .Returns(userGraph);

            // Repositories
            var groupsRepository = Mock.Create<IGroupsRepository<Group>>();

            var userGroupsRepository = Mock.Create<IUserGroupsRepository<UserGroup>>();
            Mock.Arrange(() => userGroupsRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<UserGroup, bool>>>()))
                .TaskResult(false);

            Mock.Arrange(() => userGroupsRepository.CreateAsync(Arg.IsAny<UserGroup>()))
                .TaskResult(userGroupId);

            var roleRepository = Mock.Create<IRolesRepository<Role>>();
            var roleConverter = Mock.Create<IRoleConverter>();
            var azureGroupManager = Mock.Create<IAzureGroupManager>();
            var permissionManager = Mock.Create<IPermissionManager>();
            var permissionConverter = Mock.Create<IPermissionConverter>();

            // Initialise GroupManager
            var groupManager =
                new Authentication.Services.GroupManager(graphApiClient, groupsRepository, userGroupsRepository, roleRepository, roleConverter, azureGroupManager, permissionManager, permissionConverter);

            // Assert & Act
            await Assert.ThrowsAsync<TQAuthenticationException>(() => groupManager.AddUserAsync(Guid.NewGuid(), Guid.NewGuid()));

            Mock.Assert(() => graphApiClient.Groups, Occurs.AtLeastOnce());
            Mock.Assert(() => graphApiClient.Users, Occurs.AtLeastOnce());
            Mock.Assert(() => groupGraph.AddUserAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()), Occurs.Never());

            Mock.Assert(() => userGroupsRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<UserGroup, bool>>>()), Occurs.Never());

            Mock.Assert(() => userGroupsRepository.CreateAsync(
                Arg.IsAny<UserGroup>()), Occurs.Never());
        }

        [Fact]
        public async Task AddUserAsync_GroupNotExistInAD_ThrowsException()
        {
            // Arrange
            var userGroupId = Guid.NewGuid();

            // Graph
            var groupGraph = Mock.Create<IGroupGraph>();
            var userGraph = Mock.Create<IUserGraph>();

            Mock.Arrange(() => groupGraph.UserExistsInGroupAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .TaskResult(false);

            Mock.Arrange(() => groupGraph.GroupExistsAsync(Arg.IsAny<Guid>()))
                .TaskResult(false);

            Mock.Arrange(() => userGraph.UserExistsAsync(Arg.IsAny<Guid>()))
                .TaskResult(true);

            Mock.Arrange(() => groupGraph.AddUserAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var graphApiClient = Mock.Create<IGraphApiClient>();
            Mock.Arrange(() => graphApiClient.Groups)
                .Returns(groupGraph);
            Mock.Arrange(() => graphApiClient.Users)
                .Returns(userGraph);

            // Repositories
            var groupsRepository = Mock.Create<IGroupsRepository<Group>>();

            var userGroupsRepository = Mock.Create<IUserGroupsRepository<UserGroup>>();
            Mock.Arrange(() => userGroupsRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<UserGroup, bool>>>()))
                .TaskResult(false);

            Mock.Arrange(() => userGroupsRepository.CreateAsync(Arg.IsAny<UserGroup>()))
                .TaskResult(userGroupId);

            var roleRepository = Mock.Create<IRolesRepository<Role>>();
            var roleConverter = Mock.Create<IRoleConverter>();
            var azureGroupManager = Mock.Create<IAzureGroupManager>();
            var permissionManager = Mock.Create<IPermissionManager>();
            var permissionConverter = Mock.Create<IPermissionConverter>();

            // Initialise GroupManager
            var groupManager =
                new Authentication.Services.GroupManager(graphApiClient, groupsRepository, userGroupsRepository, roleRepository, roleConverter, azureGroupManager, permissionManager, permissionConverter);

            // Assert & Act
            await Assert.ThrowsAsync<TQAuthenticationException>(() => groupManager.AddUserAsync(Guid.NewGuid(), Guid.NewGuid()));

            Mock.Assert(() => graphApiClient.Groups, Occurs.AtLeastOnce());
            Mock.Assert(() => graphApiClient.Users, Occurs.Never());
            Mock.Assert(() => groupGraph.AddUserAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()), Occurs.Never());

            Mock.Assert(() => userGroupsRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<UserGroup, bool>>>()), Occurs.Never());

            Mock.Assert(() => userGroupsRepository.CreateAsync(
                Arg.IsAny<UserGroup>()), Occurs.Never());
        }

        [Fact]
        public async Task AddUserAsync_AlreadyExistsInB2C_AddsUser()
        {
            // Arrange
            var userGroupId = Guid.NewGuid();

            // Graph
            var groupGraph = Mock.Create<IGroupGraph>();
            var userGraph = Mock.Create<IUserGraph>();

            Mock.Arrange(() => groupGraph.UserExistsInGroupAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .TaskResult(true);

            Mock.Arrange(() => groupGraph.GroupExistsAsync(Arg.IsAny<Guid>()))
                .TaskResult(true);

            Mock.Arrange(() => userGraph.UserExistsAsync(Arg.IsAny<Guid>()))
                .TaskResult(true);

            Mock.Arrange(() => groupGraph.AddUserAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var graphApiClient = Mock.Create<IGraphApiClient>();
            Mock.Arrange(() => graphApiClient.Groups)
                .Returns(groupGraph);
            Mock.Arrange(() => graphApiClient.Users)
                .Returns(userGraph);

            // Repositories
            var groupsRepository = Mock.Create<IGroupsRepository<Group>>();

            var userGroupsRepository = Mock.Create<IUserGroupsRepository<UserGroup>>();
            Mock.Arrange(() => userGroupsRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<UserGroup, bool>>>()))
                .TaskResult(false);

            Mock.Arrange(() => userGroupsRepository.GetUserGroupIdAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .TaskResult(Arg.IsAny<Guid>());

            Mock.Arrange(() => userGroupsRepository.CreateAsync(Arg.IsAny<UserGroup>()))
                .TaskResult(userGroupId);

            var roleRepository = Mock.Create<IRolesRepository<Role>>();
            var roleConverter = Mock.Create<IRoleConverter>();
            var azureGroupManager = Mock.Create<IAzureGroupManager>();
            var permissionManager = Mock.Create<IPermissionManager>();
            var permissionConverter = Mock.Create<IPermissionConverter>();

            // Initialise GroupManager
            var groupManager =
                new Authentication.Services.GroupManager(graphApiClient, groupsRepository, userGroupsRepository, roleRepository, roleConverter, azureGroupManager, permissionManager, permissionConverter);

            // Act
            await groupManager.AddUserAsync(Guid.NewGuid(), Guid.NewGuid());

            // Assert
            Mock.Assert(() => graphApiClient.Groups, Occurs.AtLeastOnce());
            Mock.Assert(() => graphApiClient.Users, Occurs.AtLeastOnce());
            Mock.Assert(() => groupGraph.AddUserAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()), Occurs.Never());

            Mock.Assert(() => userGroupsRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<UserGroup, bool>>>()), Occurs.Never());

            Mock.Assert(() => userGroupsRepository.GetUserGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<Guid>()), Occurs.Once());

            Mock.Assert(() => userGroupsRepository.CreateAsync(
                Arg.IsAny<UserGroup>()), Occurs.Never());
        }

        [Fact]
        public async Task AddUserAsync_NotExistInB2C_ExistInSql_AddsUser()
        {
            // Arrange
            var userGroupId = Guid.NewGuid();

            // Graph
            var groupGraph = Mock.Create<IGroupGraph>();
            var userGraph = Mock.Create<IUserGraph>();

            Mock.Arrange(() => groupGraph.UserExistsInGroupAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .TaskResult(false);

            Mock.Arrange(() => groupGraph.GroupExistsAsync(Arg.IsAny<Guid>()))
                .TaskResult(true);

            Mock.Arrange(() => userGraph.UserExistsAsync(Arg.IsAny<Guid>()))
                .TaskResult(true);

            Mock.Arrange(() => groupGraph.AddUserAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var graphApiClient = Mock.Create<IGraphApiClient>();
            Mock.Arrange(() => graphApiClient.Groups)
                .Returns(groupGraph);
            Mock.Arrange(() => graphApiClient.Users)
                .Returns(userGraph);

            // Repositories
            var groupsRepository = Mock.Create<IGroupsRepository<Group>>();

            var userGroupsRepository = Mock.Create<IUserGroupsRepository<UserGroup>>();
            Mock.Arrange(() => userGroupsRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<UserGroup, bool>>>()))
                .TaskResult(true);

            Mock.Arrange(() => userGroupsRepository.GetUserGroupIdAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .TaskResult(Arg.IsAny<Guid>());

            Mock.Arrange(() => userGroupsRepository.CreateAsync(Arg.IsAny<UserGroup>()))
                .TaskResult(userGroupId);

            var roleRepository = Mock.Create<IRolesRepository<Role>>();
            var roleConverter = Mock.Create<IRoleConverter>();
            var azureGroupManager = Mock.Create<IAzureGroupManager>();
            var permissionManager = Mock.Create<IPermissionManager>();
            var permissionConverter = Mock.Create<IPermissionConverter>();

            // Initialise GroupManager
            var groupManager =
                new Authentication.Services.GroupManager(graphApiClient, groupsRepository, userGroupsRepository, roleRepository, roleConverter, azureGroupManager, permissionManager, permissionConverter);

            // Act
            await groupManager.AddUserAsync(Guid.NewGuid(), Guid.NewGuid());

            // Assert
            Mock.Assert(() => graphApiClient.Groups, Occurs.AtLeastOnce());
            Mock.Assert(() => graphApiClient.Users, Occurs.AtLeastOnce());

            Mock.Assert(() => groupGraph.AddUserAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<Guid>()), Occurs.Once());

            Mock.Assert(() => userGroupsRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<UserGroup, bool>>>()), Occurs.Once());

            Mock.Assert(() => userGroupsRepository.GetUserGroupIdAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<Guid>()), Occurs.Once());

            Mock.Assert(() => userGroupsRepository.CreateAsync(
                Arg.IsAny<UserGroup>()), Occurs.Never());
        }

        [Fact]
        public async Task AddUserAsync_GroupGraphFails_ThrowsException()
        {
            // Arrange
            var userGroupId = Guid.NewGuid();

            // Graph
            var groupGraph = Mock.Create<IGroupGraph>();
            var userGraph = Mock.Create<IUserGraph>();

            Mock.Arrange(() => groupGraph.UserExistsInGroupAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .Throws<Exception>();

            Mock.Arrange(() => groupGraph.GroupExistsAsync(Arg.IsAny<Guid>()))
                .TaskResult(true);

            Mock.Arrange(() => userGraph.UserExistsAsync(Arg.IsAny<Guid>()))
                .TaskResult(true);

            Mock.Arrange(() => groupGraph.AddUserAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var graphApiClient = Mock.Create<IGraphApiClient>();
            Mock.Arrange(() => graphApiClient.Groups)
                .Returns(groupGraph);
            Mock.Arrange(() => graphApiClient.Users)
                .Returns(userGraph);

            // Repositories
            var groupsRepository = Mock.Create<IGroupsRepository<Group>>();

            var userGroupsRepository = Mock.Create<IUserGroupsRepository<UserGroup>>();
            Mock.Arrange(() => userGroupsRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<UserGroup, bool>>>()))
                .TaskResult(false);

            Mock.Arrange(() => userGroupsRepository.CreateAsync(Arg.IsAny<UserGroup>()))
                .TaskResult(userGroupId);

            var roleRepository = Mock.Create<IRolesRepository<Role>>();
            var roleConverter = Mock.Create<IRoleConverter>();
            var azureGroupManager = Mock.Create<IAzureGroupManager>();
            var permissionManager = Mock.Create<IPermissionManager>();
            var permissionConverter = Mock.Create<IPermissionConverter>();


            // Initialise GroupManager
            var groupManager =
                new Authentication.Services.GroupManager(graphApiClient, groupsRepository, userGroupsRepository, roleRepository, roleConverter, azureGroupManager, permissionManager, permissionConverter);

            // Assert & Act
            await Assert.ThrowsAsync<Exception>(() => groupManager.AddUserAsync(Guid.NewGuid(), Guid.NewGuid()));

            Mock.Assert(() => graphApiClient.Groups, Occurs.AtLeastOnce());
            Mock.Assert(() => graphApiClient.Users, Occurs.Once());
            Mock.Assert(() => groupGraph.AddUserAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()), Occurs.Never());

            Mock.Assert(() => userGroupsRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<UserGroup, bool>>>()), Occurs.Never());

            Mock.Assert(() => userGroupsRepository.CreateAsync(
                Arg.IsAny<UserGroup>()), Occurs.Never());
        }
    }
}