using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Dto.Users;
using TQ.Authentication.Core.Requests.Users;
using TQ.Authentication.Services.Interfaces;
using Xunit;

namespace TQ.Authentication.UnitTests.Controllers.UsersController
{
    public class GetFilteredAsync
    {
        private IUserManager userManager;
        private readonly IPermissionManager permissionManager = Mock.Create<IPermissionManager>();

        [Fact]
        public async Task GetFiltered_ReturnsResponse()
        {
            // Arrange
            userManager = Mock.Create<IUserManager>();
            Mock.Arrange(() => userManager.GetUsersAsync(Arg.IsAny<GetFilteredUsersRequest>()))
                .TaskResult(new GetPagedUsersDto
                {
                    NextPageToken = "sometoken",
                    Users = new List<UserDto>
                    {
                        new UserDto { Id = Guid.NewGuid() },
                        new UserDto { Id = Guid.NewGuid() },
                        new UserDto { Id = Guid.NewGuid() }
                    }
                });

            var controller = CreateSut();

            // Act
            var response = await controller.GetFilteredAsync(new GetFilteredUsersRequest());

            var okResponse = (OkObjectResult)response.Result;

            var pagedUsers = (GetPagedUsersDto)okResponse.Value;

            // Assert
            Assert.NotNull(okResponse);
            Assert.Equal(200, okResponse.StatusCode);
            Assert.Equal(3, pagedUsers.Users.Count());
            Assert.NotNull(pagedUsers.NextPageToken);

            Mock.Assert(() => userManager.GetUsersAsync(Arg.IsAny<GetFilteredUsersRequest>()), Occurs.Once());
        }

        [Fact]
        public async Task GetFiltered_AzureNoUsersFound_ReturnsResponse()
        {
            // Arrange
            userManager = Mock.Create<IUserManager>();
            Mock.Arrange(() => userManager.GetUsersAsync(Arg.IsAny<GetFilteredUsersRequest>()))
                .TaskResult(new GetPagedUsersDto
                {
                    NextPageToken = null,
                    Users = new List<UserDto>()
                });

            var controller = CreateSut();

            // Act
            var response = await controller.GetFilteredAsync(new GetFilteredUsersRequest());

            var okResponse = (OkObjectResult)response.Result;

            var pagedUsers = (GetPagedUsersDto)okResponse.Value;

            // Assert
            Assert.NotNull(okResponse);
            Assert.Equal(200, okResponse.StatusCode);
            Assert.Empty(pagedUsers.Users);
            Assert.Null(pagedUsers.NextPageToken);

            Mock.Assert(() => userManager.GetUsersAsync(Arg.IsAny<GetFilteredUsersRequest>()), Occurs.Once());
        }

        private API.Controllers.UsersController CreateSut()
        {
            return new API.Controllers.UsersController(userManager, permissionManager);
        }
    }
}