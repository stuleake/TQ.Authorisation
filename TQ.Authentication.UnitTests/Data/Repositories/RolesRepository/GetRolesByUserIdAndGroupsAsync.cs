using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TQ.Authentication.Data.Contexts;
using TQ.Authentication.Data.Entities;
using Xunit;

namespace TQ.Authentication.UnitTests.Data.Repositories.RolesRepository
{
    public class GetRolesByUserIdAndGroupsAsync
    {
        private readonly ApplicationDbContext context;
        private Guid userId = Guid.NewGuid();
        private Guid groupId = Guid.NewGuid();
        private Guid permission1 = Guid.NewGuid();
        private Guid roleId = Guid.NewGuid();
        private Guid rolePermissionId = Guid.NewGuid();
        private Guid userRoleId = Guid.NewGuid();

        public GetRolesByUserIdAndGroupsAsync()
        {
            context = new InMemoryDbContextFactory(false).GetApplicationDbContext();
        }

        [Fact]
        public async Task GetRolesByUserIdAndGroupsAsyncReturnsRoleSuccess()
        {
            // Arrange
            var expectedRoleCount = 1;
            var groups = new List<Group>() { new Group { Id = groupId, ServiceUrl = "http://test.co.uk/" } };
            var role = new Role { Id = roleId, Name = "RoleName1", Description = "Role Description 1", GroupId = groupId };
            this.context.Roles.Add(role);
            this.context.RolePermissions.Add(new RolePermission { Id = rolePermissionId, RoleId = roleId, PermissionId = permission1 });
            this.context.UserRoles.Add(new UserRole { Id = userRoleId, UserId = userId, RoleId = roleId });
            await this.context.SaveChangesAsync();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetRolesByUserIdAndGroupsAsync(userId, groups);

            // Assert
            result.Count().Should().Be(expectedRoleCount);
            result.Should().BeEquivalentTo(new List<Role>
            {
                new Role()
                {
                    Id = roleId,
                    Name = "RoleName1",
                    Description = "Role Description 1",
                    GroupId = groupId,
                    RolePermissions = new List<RolePermission>
                    {
                        new RolePermission()
                        {
                            Id = rolePermissionId,
                            RoleId = roleId,
                            PermissionId = permission1,
                            Role = role
                        }
                    },
                    UserRoles = new List<UserRole>
                    {
                        new UserRole()
                        {
                            Id = userRoleId,
                            UserId = userId,
                            RoleId = roleId,
                            Role = role
                        }
                    }
                }
            });
        }

        [Fact]
        public async Task GetRolesByUserIdAndGroupsAsyncReturnsNothingWhenNoUserRole()
        {
            // Arrange
            var expectedRoleCount = 0;
            var groups = new List<Group>() { new Group { Id = groupId, ServiceUrl = "http://test.co.uk/" } };
            var role = new Role { Id = roleId, Name = "RoleName1", Description = "Role Description 1", GroupId = groupId };
            this.context.Roles.Add(role);
            this.context.RolePermissions.Add(new RolePermission { Id = rolePermissionId, RoleId = roleId, PermissionId = permission1 });
            await this.context.SaveChangesAsync();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetRolesByUserIdAndGroupsAsync(userId, groups);

            // Assert
            result.Count().Should().Be(expectedRoleCount);
        }

        [Fact]
        public async Task GetRolesByUserIdAndGroupsAsyncReturnsNothingWhenNoRolePermission()
        {
            // Arrange
            var expectedRoleCount = 0;
            var groups = new List<Group>() { new Group { Id = groupId, ServiceUrl = "http://test.co.uk/" } };
            var role = new Role { Id = roleId, Name = "RoleName1", Description = "Role Description 1", GroupId = groupId };
            this.context.Roles.Add(role);
            this.context.UserRoles.Add(new UserRole { Id = userRoleId, UserId = userId, RoleId = roleId });
            await this.context.SaveChangesAsync();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetRolesByUserIdAndGroupsAsync(userId, groups);

            // Assert
            result.Count().Should().Be(expectedRoleCount);
        }

        [Fact]
        public async Task GetRolesByUserIdAndGroupsAsyncReturnsNothingWhenWrongUserIdPassed()
        {
            // Arrange
            var expectedRoleCount = 0;
            var anotherUserId = Guid.NewGuid();
            var groups = new List<Group>() { new Group { Id = groupId, ServiceUrl = "http://test.co.uk/" } };
            var role = new Role { Id = roleId, Name = "RoleName1", Description = "Role Description 1", GroupId = groupId };
            this.context.Roles.Add(role);
            this.context.RolePermissions.Add(new RolePermission { Id = rolePermissionId, RoleId = roleId, PermissionId = permission1 });
            this.context.UserRoles.Add(new UserRole { Id = userRoleId, UserId = userId, RoleId = roleId });
            await this.context.SaveChangesAsync();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetRolesByUserIdAndGroupsAsync(anotherUserId, groups);

            // Assert
            result.Count().Should().Be(expectedRoleCount);
        }

        [Fact]
        public async Task GetRolesByUserIdAndGroupsAsyncReturnsNothingWhenWrongGroupsPassed()
        {
            // Arrange
            var expectedRoleCount = 0;
            var role = new Role { Id = roleId, Name = "RoleName1", Description = "Role Description 1", GroupId = groupId };
            this.context.Roles.Add(role);
            this.context.RolePermissions.Add(new RolePermission { Id = rolePermissionId, RoleId = roleId, PermissionId = permission1 });
            this.context.UserRoles.Add(new UserRole { Id = userRoleId, UserId = userId, RoleId = roleId });
            await this.context.SaveChangesAsync();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetRolesByUserIdAndGroupsAsync(userId, new List<Group>() { new Group { Id = new Guid(), ServiceUrl = "http://test.co.uk/" } });

            // Assert
            result.Count().Should().Be(expectedRoleCount);
        }

        private Authentication.Data.Repositories.RolesRepository CreateSut()
        {
            return new Authentication.Data.Repositories.RolesRepository(context);
        }
    }
}