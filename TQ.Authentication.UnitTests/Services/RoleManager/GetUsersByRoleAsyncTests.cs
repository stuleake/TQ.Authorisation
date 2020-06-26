using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.Data.Repositories;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.ExternalServices.GraphAPI.Models;
using TQ.Authentication.Services.Interfaces;
using TQ.Authentication.Services.Interfaces.Converters;
using Xunit;

namespace TQ.Authentication.UnitTests.Services.RoleManager
{
    public class GetUsersByRoleAsyncTests

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
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var roleId = Guid.NewGuid();

            Mock.Arrange(() => rolesRepository.CreateAsync(Arg.IsAny<Role>()))
                .TaskResult(roleId);

            var userGraph = Mock.Create<IUserGraph>();

            Mock.Arrange(() => userGraph.GetPagedUsersAsync(Arg.IsAny<AzureUserFilter>()))
                .TaskResult(new AzurePagedUsers
                {
                    NextPageToken = null,
                    Users = new List<AzureUser>
                    {
                        new AzureUser
                        {
                            Id = userId1,
                            DisplayName = "FirstName LastName 1",
                            GivenName = "FirstName1",
                            Surname = "LastName1"
                        },
                        new AzureUser
                        {
                            Id = userId2,
                            DisplayName = "FirstName LastName 2",
                            GivenName = "FirstName2",
                            Surname = "LastName2"
                        }
                    }
                });

            Mock.Arrange(() => userGraph.UserExistsAsync(Arg.IsAny<Guid>()))
                .TaskResult(true);

            var userManager = new Authentication.Services.UserManager(graphApiClient, usersRepository);

            Mock.Arrange(() => graphApiClient.Users)
                .Returns(userGraph);

            Mock.Arrange(() => usersRepository.GetUsersByRoleIdsAsync(roleId, Arg.IsAny<IEnumerable<Guid>>()))
                .TaskResult(new List<User>
                    {
                                    new User { Id = userId1 },
                                    new User { Id = userId2 }
                    });

            Mock.Arrange(() => rolesRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Role, bool>>>()))
                .TaskResult(true);

            var sut = this.CreateSut();

            // Act
            var result = await sut.GetUsersByRoleAsync(roleId);

            // Assert
            result.Should().NotBeNull();
            Mock.Assert(() => usersRepository.GetUsersByRoleIdsAsync(roleId, Arg.IsAny<IEnumerable<Guid>>()), Occurs.Once());
            Mock.Assert(() => userGraph.GetPagedUsersAsync(Arg.IsAny<AzureUserFilter>()), Occurs.Once());
            Mock.Assert(() => rolesRepository.IsAnyMatchedAsync(
                Arg.IsAny<Expression<Func<Role, bool>>>()), Occurs.Once());
        }

        [Fact]
        public async Task GetUsersByRoleAsyncThrowsTQAuthenticationExceptionWhenRoleNotFound()
        {
            // Arrange
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var roleId = Guid.NewGuid();

            Mock.Arrange(() => rolesRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Role, bool>>>()))
                .TaskResult(false);

            var userGraph = Mock.Create<IUserGraph>();

            Mock.Arrange(() => userGraph.GetPagedUsersAsync(Arg.IsAny<AzureUserFilter>()))
                .TaskResult(new AzurePagedUsers
                {
                    NextPageToken = null,
                    Users = new List<AzureUser>
                    {
                        new AzureUser
                        {
                            Id = userId1,
                            DisplayName = "FirstName LastName 1",
                            GivenName = "FirstName1",
                            Surname = "LastName1"
                        },
                        new AzureUser
                        {
                            Id = userId2,
                            DisplayName = "FirstName LastName 2",
                            GivenName = "FirstName2",
                            Surname = "LastName2"
                        }
                    }
                });

            Mock.Arrange(() => userGraph.UserExistsAsync(Arg.IsAny<Guid>()))
                .TaskResult(true);

            var userManager = new Authentication.Services.UserManager(graphApiClient, usersRepository);

            Mock.Arrange(() => graphApiClient.Users)
                .Returns(userGraph);

            Mock.Arrange(() => usersRepository.GetUsersByIdsAsync(Arg.IsAny<IEnumerable<Guid>>()))
                .TaskResult(new List<User>
                    {
                        new User { Id = userId1 },
                        new User { Id = userId2 }
                    });

            Mock.Arrange(() => userRolesRepository.CreateAsync(new UserRole { Id = Guid.NewGuid(), UserId = userId1, RoleId = roleId }));
            Mock.Arrange(() => userRolesRepository.CreateAsync(new UserRole { Id = Guid.NewGuid(), UserId = userId2, RoleId = roleId }));
            var sut = this.CreateSut();

            // Act
            await Assert.ThrowsAsync<TQAuthenticationException>(() => sut.GetUsersByRoleAsync(roleId));

            // Assert
            Mock.Assert(() => rolesRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Role, bool>>>()), Occurs.Once());
            Mock.Assert(() => usersRepository.GetUsersByIdsAsync(Arg.IsAny<IEnumerable<Guid>>()), Occurs.Never());
            Mock.Assert(() => userGraph.GetPagedUsersAsync(Arg.IsAny<AzureUserFilter>()), Occurs.Never());
        }

        [Fact]
        public async Task GetUsersByRoleAsyncThrowsTQAuthenticationExceptionWhenAzureUsersNull()
        {
            // Arrange
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var roleId = Guid.NewGuid();

            Mock.Arrange(() => rolesRepository.CreateAsync(Arg.IsAny<Role>()))
                .TaskResult(roleId);

            var userGraph = Mock.Create<IUserGraph>();

            Mock.Arrange(() => userGraph.GetPagedUsersAsync(Arg.IsAny<AzureUserFilter>()))
                .TaskResult(null);

            Mock.Arrange(() => userGraph.UserExistsAsync(Arg.IsAny<Guid>()))
                .TaskResult(true);

            var userManager = new Authentication.Services.UserManager(graphApiClient, usersRepository);

            Mock.Arrange(() => graphApiClient.Users)
                .Returns(userGraph);

            Mock.Arrange(() => usersRepository.GetUsersByIdsAsync(Arg.IsAny<IEnumerable<Guid>>()))
                .TaskResult(new List<User>
                    {
                        new User { Id = userId1 },
                        new User { Id = userId2 }
                    });

            Mock.Arrange(() => rolesRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Role, bool>>>()))
                .TaskResult(true);

            Mock.Arrange(() => userRolesRepository.CreateAsync(new UserRole { Id = Guid.NewGuid(), UserId = userId1, RoleId = roleId }));
            Mock.Arrange(() => userRolesRepository.CreateAsync(new UserRole { Id = Guid.NewGuid(), UserId = userId2, RoleId = roleId }));
            var sut = this.CreateSut();

            // Act
            await Assert.ThrowsAsync<NullReferenceException>(() => sut.GetUsersByRoleAsync(roleId));

            // Assert
            Mock.Assert(() => rolesRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Role, bool>>>()), Occurs.Once());
            Mock.Assert(() => usersRepository.GetUsersByIdsAsync(Arg.IsAny<IEnumerable<Guid>>()), Occurs.Never());
            Mock.Assert(() => userGraph.GetPagedUsersAsync(Arg.IsAny<AzureUserFilter>()), Occurs.Once());
        }

        [Fact]
        public async Task GetUsersByRoleAsyncThrowsTQAuthenticationExceptionWhenDatabaseUsersNull()
        {
            // Arrange
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var roleId = Guid.NewGuid();

            Mock.Arrange(() => rolesRepository.CreateAsync(Arg.IsAny<Role>()))
                .TaskResult(roleId);

            var userGraph = Mock.Create<IUserGraph>();

            Mock.Arrange(() => userGraph.GetPagedUsersAsync(Arg.IsAny<AzureUserFilter>()))
                .TaskResult(new AzurePagedUsers
                {
                    NextPageToken = null,
                    Users = new List<AzureUser>
                    {
                        new AzureUser
                        {
                            Id = userId1,
                            DisplayName = "FirstName LastName 1",
                            GivenName = "FirstName1",
                            Surname = "LastName1"
                        },
                        new AzureUser
                        {
                            Id = userId2,
                            DisplayName = "FirstName LastName 2",
                            GivenName = "FirstName2",
                            Surname = "LastName2"
                        }
                    }
                });

            Mock.Arrange(() => userGraph.UserExistsAsync(Arg.IsAny<Guid>()))
                .TaskResult(true);

            var userManager = new Authentication.Services.UserManager(graphApiClient, usersRepository);

            Mock.Arrange(() => graphApiClient.Users)
                .Returns(userGraph);

            Mock.Arrange(() => usersRepository.GetUsersByRoleIdsAsync(roleId, Arg.IsAny<IEnumerable<Guid>>()))
                .TaskResult(null);

            Mock.Arrange(() => rolesRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Role, bool>>>()))
                .TaskResult(true);

            var sut = this.CreateSut();

            // Act
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetUsersByRoleAsync(roleId));

            // Assert
            Mock.Assert(() => rolesRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<Role, bool>>>()), Occurs.Once());
            Mock.Assert(() => usersRepository.GetUsersByIdsAsync(Arg.IsAny<IEnumerable<Guid>>()), Occurs.Never());
            Mock.Assert(() => userGraph.GetPagedUsersAsync(Arg.IsAny<AzureUserFilter>()), Occurs.Once());
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