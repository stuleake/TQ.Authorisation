using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Telerik.JustMock;
using TQ.Authentication.Core.Dto.Permissions;
using TQ.Authentication.Services.Interfaces;
using Xunit;

namespace TQ.Authentication.UnitTests.Controllers.GroupsController
{
    public class GetPermissionsByGroupIdAsync
    {
        private readonly IGroupManager mockGroupManager = Mock.Create<IGroupManager>();

        private const string permissionName = "PermissionName";
        private const string permissionDescription = "Permission Description";
        private const string childPermissionName = "ChildPermissionName";
        private const string childPermissionDescription = "Child Permission Description";

        [Fact]
        public async Task GetPermissionsByUserIdAsyncSuccess()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            var permissionId = Guid.NewGuid();
            var childPermissionId = Guid.NewGuid();

            var expectedResult = new List<GetPermissionsDto>()
            {
                new GetPermissionsDto()
                {
                    Id = permissionId,
                    Name = permissionName,
                    Description = permissionDescription,
                    Children = new List<GetPermissionsDto>()
                    {
                        new GetPermissionsDto()
                        {
                            Id = childPermissionId,
                            Name = childPermissionName,
                            Description = childPermissionDescription,
                            Children = new List<GetPermissionsDto>()
                        }
                    }
                }
            }.AsEnumerable();

            Mock.Arrange(() => mockGroupManager.GetGroupPermissionsAsync(groupId)).TaskResult(expectedResult);

            var groupsController = CreateSut();

            // Act
            var result = await groupsController.GetPermissionsByGroupIdAsync(groupId);

            // Assert
            Mock.Assert(() => mockGroupManager.GetGroupPermissionsAsync(groupId), Occurs.Once());

            result.Should().NotBeNull();
            var okResult = (OkObjectResult)result.Result;
            okResult.Value.Should().BeOfType(typeof(List<GetPermissionsDto>));
            okResult.Value.Should().BeEquivalentTo(expectedResult);
        }

        private API.Controllers.GroupsController CreateSut()
        {
            return new API.Controllers.GroupsController(mockGroupManager);
        }
    }
}