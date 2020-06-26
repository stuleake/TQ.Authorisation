using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using TQ.Authentication.Core.Dto.Permissions;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.ExternalServices.GraphAPI.Models;
using TQ.Authentication.Services.Converters;
using TQ.Authentication.Services.Interfaces.Converters;
using Xunit;

namespace TQ.Authentication.UnitTests.Services.Converters
{
    public class RoleConverterTests
    {
        private static Guid role1Id = Guid.NewGuid();
        private static Guid role2Id = Guid.NewGuid();
        private static readonly string role1Name = "role1 name";
        private static readonly string role2Name = "role2 name";
        private static readonly string role1Description = "role1 description";
        private static readonly string role2Description = "role2 description";

        private readonly List<Role> roles = new List<Role>
        {
            new Role { Id = role1Id, Name = role1Name, Description = role1Description },
            new Role { Id = role2Id, Name = role2Name, Description = role2Description }
        };

        [Fact]
        public void ToRoleGetDtosReturnsEmptyWhenRolesAreEmpty()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act
            var result = sut.ToRoleGetDtos(null);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void ToRoleGetDtosReturnsSuccess()
        {
            // Arrange
            var expectedCount = 2;
            
            var sut = this.CreateSut();

            // Act
            var result = sut.ToRoleGetDtos(roles);

            // Assert
            result.Count().Should().Be(expectedCount);
            result.First().Name.Should().Be(role1Name);
            result.First().Description.Should().Be(role1Description);
            result.Last().Name.Should().Be(role2Name);
            result.Last().Description.Should().Be(role2Description);
        }

        [Fact]
        public void ToRoleGroupPermissionGetDtoReturnsSuccess()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            var roleName = "role";
            var roleDescription = "description";
            var role = new Role
            {
                Id = roleId,
                Name = roleName,
                Description = roleDescription
            };

            var azureGroupId = Guid.NewGuid();
            var azureGroupCreatedAt = DateTime.Now;
            var azureGroupDescription = "azure description";
            var azureGroupName = "azure name";

            var azureGroup = new AzureGroup
            {
                Id = azureGroupId,
                CreatedAt = azureGroupCreatedAt,
                Description = azureGroupDescription,
                Name = azureGroupName
            };

            var groupServiceUrl = "dservice url";
            var groupIsActive = true;
            var group = new Group
            {
                ServiceUrl = groupServiceUrl,
                IsActive = groupIsActive
            };

            var permissionsDtos = new List<GetPermissionsDto>
            {
                new GetPermissionsDto
                {
                    Description = "description",
                    Id = Guid.NewGuid(),
                    Name = "name"
                }
            };

            var sut = this.CreateSut();

            // Act
            var result = sut.ToRoleGroupPermissionGetDto(role, group, azureGroup, permissionsDtos);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(role.Name);
            result.Description.Should().Be(role.Description);
            result.Group.Should().NotBeNull();
            result.Group.Id.Should().Be(azureGroupId);
            result.Group.CreatedAt.Should().Be(azureGroupCreatedAt);
            result.Group.ServiceUrl.Should().Be(groupServiceUrl);
            result.Group.IsActive.Should().Be(groupIsActive);
            result.Group.Name.Should().Be(azureGroupName);
            result.Group.Description.Should().Be(azureGroupDescription);
            result.Permissions.Should().NotBeEmpty();
            result.Permissions.Should().HaveCount(1);
            result.Permissions.Should().BeEquivalentTo(permissionsDtos);
        }

        private IRoleConverter CreateSut()
        {
            return new RoleConverter();
        }
    }
}