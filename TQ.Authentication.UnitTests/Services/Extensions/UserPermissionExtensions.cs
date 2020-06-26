using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TQ.Authentication.Core.Dto.Permissions;
using TQ.Authentication.Services.Extensions;
using Xunit;

namespace TQ.Authentication.UnitTests.Services.PermissionsManager
{
    public class UserPermissionExtensions
    {
        private const string permissionName = "PermissionName";
        private const string groupName = "GroupName";

        [Fact]
        public void ToBearerTokenFormatForGroupAndPermissionSuccess()
        {
            // Arrange
            var userPermissions = new List<GroupPermissionNamesDto>()
            {
                new GroupPermissionNamesDto()
                {
                    GroupName = groupName,
                    PermissionNames = new List<string>() { permissionName }
                }
            };

            var expectedResult = CreateExpectedResult(userPermissions);

            // Act
            var result = userPermissions.ToBearerTokenFormat();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void ToBearerTokenFormatForGroupsAndPermissionsSuccess()
        {
            // Arrange
            var userPermissions = new List<GroupPermissionNamesDto>()
            {
                new GroupPermissionNamesDto()
                {
                    GroupName = $"{groupName}1",
                    PermissionNames = new List<string>() { $"{permissionName}1", $"{permissionName}2" }
                },
                new GroupPermissionNamesDto()
                {
                    GroupName = $"{groupName}2",
                    PermissionNames = new List<string>() { $"{permissionName}3", $"{permissionName}4", $"{permissionName}5" }
                }
            };

            var expectedResult = CreateExpectedResult(userPermissions);

            // Act
            var result = userPermissions.ToBearerTokenFormat();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void ToBearerTokenFormatForGroupWithNoPermissionsSuccess()
        {
            // Arrange
            var userPermissions = new List<GroupPermissionNamesDto>()
            {
                new GroupPermissionNamesDto()
                {
                    GroupName = $"{groupName}",
                    PermissionNames = new List<string>()
                }
            };

            var expectedResult = CreateExpectedResult(userPermissions);

            // Act
            var result = userPermissions.ToBearerTokenFormat();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void ToBearerTokenFormatForNoGroupsSuccess()
        {
            // Arrange
            var userPermissions = new List<GroupPermissionNamesDto>();
            var expectedResult = CreateExpectedResult(userPermissions);

            // Act
            var result = userPermissions.ToBearerTokenFormat();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void ToBearerTokenFormatThrowsArgumentExceptionWhenUserPermissionsNull()
        {
            // Arrange
            List<GroupPermissionNamesDto> userPermissions = null;

            // Act & Assert
            Exception ex = Assert.Throws<ArgumentNullException>(() => userPermissions.ToBearerTokenFormat());
            ex.Message.Should().Be("Value cannot be null. (Parameter 'userPermissions')");
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
    }
}