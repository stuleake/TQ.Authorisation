using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Dto.Permissions;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.Data.Repositories;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.ExternalServices.GraphAPI.Models;
using TQ.Authentication.Services;
using Xunit;

namespace TQ.Authentication.UnitTests.Services.PermissionsManager
{
    public class GetUserPermissionsAsync
    {
        private readonly IGraphApiClient mockGraphApiClient = Mock.Create<IGraphApiClient>();
        private readonly IUsersRepository<User> mockUsersRepository = Mock.Create<IUsersRepository<User>>();
        private readonly IGroupsRepository<Group> mockGroupsRepository = Mock.Create<IGroupsRepository<Group>>();
        private readonly IPermissionsRepository<Permission> mockPermissionsRepository = Mock.Create<IPermissionsRepository<Permission>>();
        private readonly IRolesRepository<Role> mockRolesRepository = Mock.Create<IRolesRepository<Role>>();

        private const string permissionName = "PermissionName";
        private const string permissionDescription = "Permission Description";
        private const string groupName = "GroupName";
        private const string groupDescription = "Group Description";

        private Guid userId = Guid.NewGuid();
        private readonly Group groupForUser = new Group() { Id = Guid.NewGuid(), ServiceUrl = "GroupName" };

        [Fact]
        public async Task GetUserPermissionsAsyncReturnsGroupAndPermission()
        {
            // Arrange
            var groupsForUser = new List<Group> { groupForUser };
            var roles = new List<Role> { new Role() { Id = Guid.NewGuid(), GroupId = groupForUser.Id } };
            var permission = new Permission { Id = Guid.NewGuid(), Name = permissionName, Description = permissionDescription, GroupId = groupForUser.Id, ParentPermissionId = null };
            var rolePermissions = new List<RolePermission> { new RolePermission() { Id = Guid.NewGuid(), Permission = permission } };
            var rolesAndPermissions = new List<Role> { new Role() { Id = Guid.NewGuid(), RolePermissions = rolePermissions } };

            ArrangeAzure();

            Mock.Arrange(() => mockUsersRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<User, bool>>>())).TaskResult(true);
            Mock.Arrange(() => mockGroupsRepository.GetGroupsByUserId(userId)).Returns(groupsForUser);
            Mock.Arrange(() => mockRolesRepository.GetRolesByUserIdAndGroupsAsync(userId, groupsForUser)).TaskResult(roles);
            Mock.Arrange(() => mockRolesRepository.GetByIdsIncludeRolePermissionsAsync(roles.Select(x => x.Id).ToList())).TaskResult(rolesAndPermissions);

            var userPermissions = new List<GroupPermissionNamesDto>()
            {
                new GroupPermissionNamesDto()
                {
                    GroupName = groupName,
                    PermissionNames = new List<string>() { permissionName }
                }
            };

            var expectedResult = CreateExpectedResult(userPermissions);

            var sut = CreateSut();

            // Act
            var result = await sut.GetUserPermissionsAsync(userId);

            // Assert
            Mock.Assert(() => mockUsersRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<User, bool>>>()), Occurs.Once());
            Mock.Assert(() => mockGroupsRepository.GetGroupsByUserId(userId), Occurs.Once());
            Mock.Assert(() => mockRolesRepository.GetRolesByUserIdAndGroupsAsync(userId, groupsForUser), Occurs.Once());
            Mock.Assert(() => mockRolesRepository.GetByIdsIncludeRolePermissionsAsync(roles.Select(x => x.Id).ToList()), Occurs.Once());

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetUserPermissionsAsyncReturnsGroupWhenNoPermissions()
        {
            // Arrange
            var groupsForUser = new List<Group> { groupForUser };
            var noRoles = new List<Role>();

            ArrangeAzure();

            Mock.Arrange(() => mockUsersRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<User, bool>>>())).TaskResult(true);
            Mock.Arrange(() => mockGroupsRepository.GetGroupsByUserId(userId)).Returns(groupsForUser);
            Mock.Arrange(() => mockRolesRepository.GetRolesByUserIdAndGroupsAsync(userId, groupsForUser)).TaskResult(noRoles);

            var userPermissions = new List<GroupPermissionNamesDto>()
            {
                new GroupPermissionNamesDto()
                {
                    GroupName = groupName,
                    PermissionNames = new List<string>()
                }
            };

            var expectedResult = CreateExpectedResult(userPermissions);

            var sut = CreateSut();

            // Act
            var result = await sut.GetUserPermissionsAsync(userId);

            // Assert
            Mock.Assert(() => mockUsersRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<User, bool>>>()), Occurs.Once());
            Mock.Assert(() => mockGroupsRepository.GetGroupsByUserId(userId), Occurs.Once());
            Mock.Assert(() => mockRolesRepository.GetRolesByUserIdAndGroupsAsync(userId, groupsForUser), Occurs.Once());

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetUserPermissionsAsyncReturnsEmptyWhenNoGroupsInAzure()
        {
            // Arrange
            var noGroupsForUser = new List<Group>();
            var noRoles = new List<Role>();

            Mock.Arrange(() => mockGraphApiClient.Users.UserExistsAsync(Arg.AnyGuid)).TaskResult(true);
            Mock.Arrange(() => mockUsersRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<User, bool>>>())).TaskResult(true);

            var userPermissions = new List<GroupPermissionNamesDto>();
            var expectedResult = CreateExpectedResult(userPermissions);

            var sut = CreateSut();

            // Act
            var result = await sut.GetUserPermissionsAsync(userId);

            // Assert
            Mock.Assert(() => mockUsersRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<User, bool>>>()), Occurs.Once());

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetUserPermissionsAsyncThrowsPermissionManagerExceptionWhenUserIdIsNotInDatabase()
        {
            // Arrange
            Mock.Arrange(() => mockUsersRepository.IsAnyMatchedAsync(x => x.Id == userId)).TaskResult(false);
            var sut = this.CreateSut();

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<TQAuthenticationException>(() => sut.GetUserPermissionsAsync(userId));

            ex.Message.Should().Be($"{nameof(User)} with {nameof(userId)} : {userId} does not exist in the database");
        }

        [Fact]
        public async Task GetUserPermissionsAsyncThrowsPermissionManagerExceptionWhenUserIdIsNotInAzure()
        {
            // Arrange
            Mock.Arrange(() => mockUsersRepository.IsAnyMatchedAsync(x => x.Id == userId)).TaskResult(true);
            var sut = this.CreateSut();

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<TQAuthenticationException>(() => sut.GetUserPermissionsAsync(userId));

            ex.Message.Should().Be($"{nameof(User)} with {nameof(userId)} : {userId} does not exist in Azure AD");
        }

        [Fact]
        public async Task GetUserPermissionsAsyncReturnsEmptyWhenNoGroupsInDatabase()
        {
            // Arrange
            ArrangeAzure();
            Mock.Arrange(() => mockUsersRepository.IsAnyMatchedAsync(x => x.Id == userId)).TaskResult(true);

            var userPermissions = new List<GroupPermissionNamesDto>();
            var expectedResult = CreateExpectedResult(userPermissions);

            var sut = this.CreateSut();

            // Act
            var result = await sut.GetUserPermissionsAsync(userId);

            // Assert
            Mock.Assert(() => mockUsersRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<User, bool>>>()), Occurs.Once());

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetUserPermissionsAsyncReturnsEmptyWhenNoGroupsAreInBothAzureAndDatabase()
        {
            // Arrange
            var otherGroupForUser = new Group() { Id = Guid.NewGuid(), ServiceUrl = "GroupName" };
            var otherGroupsForUser = new List<Group> { otherGroupForUser };

            ArrangeAzure();
            Mock.Arrange(() => mockUsersRepository.IsAnyMatchedAsync(x => x.Id == userId)).TaskResult(true);
            Mock.Arrange(() => mockGroupsRepository.GetGroupsByUserId(userId)).Returns(otherGroupsForUser);

            var userPermissions = new List<GroupPermissionNamesDto>();
            var expectedResult = CreateExpectedResult(userPermissions);

            var sut = this.CreateSut();

            // Act
            var result = await sut.GetUserPermissionsAsync(userId);

            // Assert
            Mock.Assert(() => mockUsersRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<User, bool>>>()), Occurs.Once());

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }

        private void ArrangeAzure()
        {
            Mock.Arrange(() => mockGraphApiClient.Users.UserExistsAsync(Arg.AnyGuid))
                .TaskResult(true);

            Mock.Arrange(() => mockGraphApiClient.Groups.GetAzureGroupsByUserIdAsync(Arg.IsAny<Guid>()))
                .TaskResult(new List<AzureGroup>() { 
                    new AzureGroup()
                    {
                        Id = groupForUser.Id,
                        Name = groupName,
                        Description = groupDescription
                    }
                });
        }

        private object CreateExpectedResult(List<GroupPermissionNamesDto> userPermissions)
        {
            var userPermissionsCustom = new Dictionary<string, ICollection<string>>();

            foreach (var group in userPermissions)
            {
                userPermissionsCustom.Add(group.GroupName, group.PermissionNames.ToList());
            }

            return new
            {
                ApplicationGroup = JsonConvert.SerializeObject(userPermissionsCustom)
            };
        }

        private PermissionManager CreateSut()
        {
            return new PermissionManager(mockGraphApiClient, mockUsersRepository, mockGroupsRepository, mockPermissionsRepository, mockRolesRepository);
        }
    }
}