using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Requests.Users;
using TQ.Authentication.Services.Interfaces;
using Xunit;

namespace TQ.Authentication.UnitTests.Controllers.UsersController
{
    public class UpdateUserAsync
    {
        private IUserManager userManager;
        private readonly IPermissionManager permissionManager = Mock.Create<IPermissionManager>();

        [Fact]
        public async Task UpdateUserAsync_ReturnsOkObjectResult()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var updateUserRequest = new UpdateUserRequest
            {
                DisplayName = "GivenName Surname",
                GivenName = "GivenName",
                Surname = "Surname"
            };

            userManager = Mock.Create<IUserManager>();
            Mock.Arrange(() => userManager.UpdateUserAsync(Arg.IsAny<Guid>(), Arg.IsAny<UpdateUserRequest>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var controller = CreateSut();

            // Act
            var response = await controller.UpdateUserAsync(userId, updateUserRequest);

            var statusCodeResult = (StatusCodeResult)response;

            // Assert
            Assert.NotNull(statusCodeResult);
            Assert.Equal(204, statusCodeResult.StatusCode);

            Mock.Assert(() => userManager.UpdateUserAsync(Arg.IsAny<Guid>(), Arg.IsAny<UpdateUserRequest>()), Occurs.Once());
        }

        private API.Controllers.UsersController CreateSut()
        {
            return new API.Controllers.UsersController(userManager, permissionManager);
        }
    }
}