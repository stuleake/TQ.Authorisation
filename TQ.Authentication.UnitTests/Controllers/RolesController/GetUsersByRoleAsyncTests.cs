using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Dto.Users;
using TQ.Authentication.Core.Requests.Users;
using TQ.Authentication.Services.Interfaces;
using Xunit;

namespace TQ.Authentication.UnitTests.Controllers.RolesController
{
    public class GetUsersByRoleAsyncTests
    {
        private readonly IRoleManager roleManager = Mock.Create<IRoleManager>();

        [Fact]
        public async Task GetUsersByRoleAsync_Returns200()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            var request = new GetFilteredUsersRequest();

            Mock.Arrange(() => roleManager.GetUsersByRoleAsync(Arg.IsAny<Guid>())).TaskResult(new GetPagedUsersDto());
            var controller = CreateSut();

            // Act
            var result = await controller.GetUsersByRoleAsync(roleId);
            var objectResult = (ObjectResult)result.Result;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
            Mock.Assert(() => roleManager.GetUsersByRoleAsync(
                Arg.IsAny<Guid>()), Occurs.Once());
        }

        private API.Controllers.RolesController CreateSut()
        {
            return new API.Controllers.RolesController(roleManager);
        }
    }
}