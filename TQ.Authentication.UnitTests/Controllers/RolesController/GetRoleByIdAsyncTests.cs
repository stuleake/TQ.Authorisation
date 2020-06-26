using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Dto.Roles;
using TQ.Authentication.Services.Interfaces;
using Xunit;

namespace TQ.Authentication.UnitTests.Controllers.RolesController
{
    public class GetRoleByIdAsyncTests
    {
        private readonly IRoleManager roleManager = Mock.Create<IRoleManager>();

        [Fact]
        public async Task GetRoleAndPermissionsAsyncReturnsOkResult()
        {
            // Arrange
            Mock.Arrange(() => roleManager.GetRoleByIdAsync(Arg.IsAny<Guid>())).TaskResult(new GetRoleGroupPermissionDto());
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetRoleByIdAsync(Guid.NewGuid());

            // Assert
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
            Mock.Assert(() => roleManager.GetRoleByIdAsync(Arg.IsAny<Guid>()), Occurs.Once());
        }

        [Fact]
        public async Task GetRoleAndPermissionsAsyncThrowsRoleManagerExceptionWhenRoleDoesNotExist()
        {
            // Arrange
            Mock.Arrange(() => roleManager.GetRoleByIdAsync(Arg.IsAny<Guid>())).Throws<Exception>();
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => sut.GetRoleByIdAsync(Guid.NewGuid()));
            Mock.Assert(() => roleManager.GetRoleByIdAsync(Arg.IsAny<Guid>()), Occurs.Once());
        }

        [Fact]
        public async Task GetRoleAndPermissionsAsyncThrowsTQAuthenticationExceptionWhenGroupDoesNotExist()
        {
            // Arrange
            Mock.Arrange(() => roleManager.GetRoleByIdAsync(Arg.IsAny<Guid>())).Throws<ArgumentNullException>();
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetRoleByIdAsync(Guid.NewGuid()));
            Mock.Assert(() => roleManager.GetRoleByIdAsync(Arg.IsAny<Guid>()), Occurs.Once());
        }

        private API.Controllers.RolesController CreateSut()
        {
            return new API.Controllers.RolesController(roleManager);
        }
    }
}