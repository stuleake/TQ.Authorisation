using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using TQ.Authentication.Core.Dto.Permissions;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.Services.Converters;
using TQ.Authentication.Services.Extensions;
using TQ.Authentication.Services.Interfaces;
using TQ.Authentication.Services.Interfaces.Converters;
using Xunit;

namespace TQ.Authentication.UnitTests.Services.Converters
{
    public class PermissionConverterTests
    {
        [Fact]
        public void GetNestedPermissionsDtoReturnsEmptyListWhenPermissionsAreNull()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act
            var result = sut.GetNestedPermissionsDto(null);

            // Asset
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetNestedPermissionsDtoReturnsEmptyListWhenPermissionsAreEmpty()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act
            var result = sut.GetNestedPermissionsDto(Enumerable.Empty<IPermissionTree>());

            // Asset
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetNestedPermissionsWithNoChildrenReturnsDto()
        {
            // Arrange
            var permission = new Permission()
            {
                Id = Guid.NewGuid(),
                Description = "Permission Description",
                Name = "Permission Name"
            };

            var permissionList = new List<Permission>() { permission };
            var permissionTree = permissionList.ToPermissionTree((parent, child) => child.ParentPermissionId == parent.Id).Children;

            var expectedResult = new List<GetPermissionsDto>()
            {
                new GetPermissionsDto()
                {
                    Id = permission.Id,
                    Description = permission.Description,
                    Name = permission.Name,
                    Children = new List<GetPermissionsDto>()
                }
            };

            var sut = this.CreateSut();

            // Act
            var result = sut.GetNestedPermissionsDto(permissionTree);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(List<GetPermissionsDto>));
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void GetNestedPermissionsWithChildrenReturnsDto()
        {
            // Arrange
            var parentPermission = new Permission()
            {
                Id = Guid.NewGuid(),
                Description = "Parent Description",
                Name = "Parent Name"                
            };

            var childPermission = new Permission()
            {
                Id = Guid.NewGuid(),
                Description = "Child Description",
                Name = "Child Name",
                ParentPermissionId = parentPermission.Id
            };

            var permissionList = new List<Permission>() { parentPermission, childPermission };
            var permissionTree = permissionList.ToPermissionTree((parent, child) => child.ParentPermissionId == parent.Id).Children;

            var expectedResult = new List<GetPermissionsDto>()
            {
                new GetPermissionsDto()
                {
                    Id = parentPermission.Id,
                    Description = parentPermission.Description,
                    Name = parentPermission.Name,
                    Children = new List<GetPermissionsDto>()
                    {
                        new GetPermissionsDto()
                        {
                            Id = childPermission.Id,
                            Description = childPermission.Description,
                            Name = childPermission.Name,
                            Children = new List<GetPermissionsDto>()
                        }
                    }
                }
            };

            var sut = this.CreateSut();

            // Act
            var result = sut.GetNestedPermissionsDto(permissionTree);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(List<GetPermissionsDto>));
            result.Should().BeEquivalentTo(expectedResult);
        }

        private IPermissionConverter CreateSut()
        {
            return new PermissionConverter();
        }
    }
}