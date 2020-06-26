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
    public class CreateRoleAsync
    {
        private IRoleManager roleManager;

        [Fact]
        public async Task CreateRoleAsync_ReturnsStatus201Created()
        {
            // Arrange
            var request = new CreateRoleRequest
            {
                Name = "Role1",
                Description = "Role1 Description",
                GroupId = Guid.NewGuid(),
                Permissions = new List<Guid>()
            };

            roleManager = Mock.Create<IRoleManager>();
            Mock.Arrange(() => roleManager.CreateRoleAsync(Arg.IsAny<CreateRoleRequest>()))
                .TaskResult(Arg.IsAny<Guid>());

            var controller = CreateSut();

            // Act
            var result = await controller.CreateRoleAsync(request);
            var objectResult = (ObjectResult)result.Result;

            // Assert
            Assert.Equal(201, objectResult.StatusCode);
            Mock.Assert(() => roleManager.CreateRoleAsync(Arg.IsAny<CreateRoleRequest>()), Occurs.Once());
        }

        private API.Controllers.RolesController CreateSut()
        {
            return new API.Controllers.RolesController(roleManager);
        }
    }
}