using FluentAssertions;
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
    public class GetPermissionsByUserIdAsync
    {
        private readonly IRoleManager mockRoleManager = Mock.Create<IRoleManager>();

        [Fact]
        public async Task CompareRoleAsyncSuccess()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            var model = new List<Guid>();
            model.Add(Guid.NewGuid());
            model.Add(Guid.NewGuid());

            var expectedResult = new StatusCodeResult(200).StatusCode;
            var rolesController = CreateSut();

            var request = new AddUsersRequest { UserIds = model };

            // Act
            var result = await rolesController.AddUsersToRoleAsync(roleId, request);

            // Assert
            Mock.Assert(() => mockRoleManager.AddUsersToRoleAsync(roleId, model), Occurs.Once());
            result.Should().NotBeNull();
            var okResultValue = 200;
            okResultValue.Should().Equals(expectedResult);
        }

        private API.Controllers.RolesController CreateSut()
        {
            return new API.Controllers.RolesController(mockRoleManager);
        }
    }
}