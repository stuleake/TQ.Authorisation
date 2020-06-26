using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Dto.Users;
using TQ.Authentication.Services.Interfaces;
using Xunit;

namespace TQ.Authentication.UnitTests.Controllers.UsersController
{
    public class GetByIdAsync
    {
        private IUserManager userManager;
        private readonly IPermissionManager permissionManager = Mock.Create<IPermissionManager>();

        [Fact]
        public async Task GetById_ReturnsOkObjectResult()
        {
            // Arrange
            userManager = Mock.Create<IUserManager>();
            Mock.Arrange(() => userManager.GetUserAsync(Arg.IsAny<Guid>()))
                .TaskResult(new UserDto
                {
                    Id = Guid.NewGuid(),
                    DisplayName = "Full Name",
                    GivenName = "GivenName",
                    Surname = "Surname",
                    EmailAddress = "my@email.com",
                    AccountEnabled = true,
                    UserPrincipalName = "not@used.com",
                    BusinessPhones = new List<string> { "111", "222" },
                    JobTitle = "Software Developer",
                    MobilePhone = "+44 123456789",
                    OfficeLocation = "Birmingham, United Kingdom",
                    PreferredLanguage = "en-EN",
                    Mail = "not@used.com"
                });

            var controller = CreateSut();

            // Act
            var response = await controller.GetByIdAsync(Guid.NewGuid());

            var okResponse = (OkObjectResult)response.Result;

            var user = (UserDto)okResponse.Value;

            // Assert
            Assert.NotNull(okResponse);
            Assert.Equal(200, okResponse.StatusCode);

            Assert.NotEqual(Guid.Empty, user.Id);
            Assert.Equal("Full Name", user.DisplayName);
            Assert.Equal("GivenName", user.GivenName);
            Assert.Equal("Surname", user.Surname);
            Assert.Equal("my@email.com", user.EmailAddress);
            Assert.True(user.AccountEnabled);
            Assert.Equal("not@used.com", user.UserPrincipalName);
            Assert.Equal(2, user.BusinessPhones.Count());
            Assert.Equal("Software Developer", user.JobTitle);
            Assert.Equal("+44 123456789", user.MobilePhone);
            Assert.Equal("Birmingham, United Kingdom", user.OfficeLocation);
            Assert.Equal("en-EN", user.PreferredLanguage);
            Assert.Equal("not@used.com", user.Mail);

            Mock.Assert(() => userManager.GetUserAsync(Arg.IsAny<Guid>()), Occurs.Once());
        }

        private API.Controllers.UsersController CreateSut()
        {
            return new API.Controllers.UsersController(userManager, permissionManager);
        }
    }
}