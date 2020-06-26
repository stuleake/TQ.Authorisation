using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Requests.Roles;
using TQ.Authentication.Services.Interfaces;
using Xunit;

namespace TQ.Authentication.UnitTests.Controllers.RolesController
{
    public class UpdateRoleAsync
    {
        private IRoleManager roleManager;

        [Fact]
        public async Task UpdateRoleAsync_ReturnsStatus204NoContent()
        {
            // Arrange
            var roleId = Guid.NewGuid();

            var request = new UpdateRoleRequest
            {
                Name = "Role1",
                Description = "Role1 Description",
                GroupId = Guid.NewGuid(),
                Permissions = new List<Guid>()
            };

            roleManager = Mock.Create<IRoleManager>();
            Mock.Arrange(() => roleManager.UpdateRoleAsync(Arg.IsAny<Guid>(), Arg.IsAny<UpdateRoleRequest>())).Returns(Task.Factory.StartNew(() => string.Empty));

            var controller = CreateSut();

            // Act
            var result = await controller.UpdateRoleAsync(roleId, request);
            var noContentResult = (NoContentResult)result;

            // Assert
            Assert.Equal(204, noContentResult.StatusCode);
            Mock.Assert(() => roleManager.UpdateRoleAsync(Arg.IsAny<Guid>(), Arg.IsAny<UpdateRoleRequest>()), Occurs.Once());
        }

        private API.Controllers.RolesController CreateSut()
        {
            return new API.Controllers.RolesController(roleManager);
        }
    }
}