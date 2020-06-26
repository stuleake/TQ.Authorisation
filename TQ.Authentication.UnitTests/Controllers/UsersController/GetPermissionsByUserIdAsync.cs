using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Services.Interfaces;
using Xunit;

namespace TQ.Authentication.UnitTests.Controllers.UsersController
{
    public class GetPermissionsByUserIdAsync
    {
        private readonly IUserManager mockUserManager = Mock.Create<IUserManager>();
        private readonly IPermissionManager mockPermissionManager = Mock.Create<IPermissionManager>();

        [Fact]
        public async Task GetPermissionsByUserIdAsyncSuccess()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var expectedResult = "\"applicationGroup\":\"{ \"GroupName\": [ \"PermissionName\" ] }";
            Mock.Arrange(() => mockPermissionManager.GetUserPermissionsAsync(userId)).TaskResult(expectedResult);
            var usersController = CreateSut();

            // Act
            var result = await usersController.GetPermissionsByUserIdAsync(userId);

            // Assert
            Mock.Assert(() => mockPermissionManager.GetUserPermissionsAsync(userId), Occurs.Once());
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
            var okResult = (OkObjectResult)result;
            okResult.Value.Should().BeEquivalentTo(expectedResult);
        }

        private API.Controllers.UsersController CreateSut()
        {
            return new API.Controllers.UsersController(mockUserManager, mockPermissionManager);
        }
    }
}