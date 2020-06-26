using System;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Requests.Users;
using TQ.Authentication.Data.Repositories;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.ExternalServices.GraphAPI.Models;
using Xunit;

namespace TQ.Authentication.UnitTests.Services.UserManager
{
    public class UpdateUserAsync
    {
        [Fact]
        public async Task UpdateUserAsync_UpdatesUser()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var graphApiClient = Mock.Create<IGraphApiClient>();
            Mock.Arrange(() => graphApiClient.Users.UpdateUserAsync(new AzureUser()));

            var usersRepository = Mock.Create<IUsersRepository<Authentication.Data.Entities.User>>();

            var userManager = new Authentication.Services.UserManager(graphApiClient, usersRepository);

            var updateUserRequest = new UpdateUserRequest
            {
                GivenName = "FirstName",
                Surname = "LastName"
            };

            // Act
            await userManager.UpdateUserAsync(userId, updateUserRequest);

            // Assert
            Mock.Assert(() => graphApiClient.Users, Occurs.Once());
        }

        [Fact]
        public async Task UpdateUserAsync_GraphApiFails_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var graphApiClient = Mock.Create<IGraphApiClient>();
            Mock.Arrange(() => graphApiClient.Users)
                .Throws<Exception>();

            var usersRepository = Mock.Create<IUsersRepository<Authentication.Data.Entities.User>>();

            var userManager = new Authentication.Services.UserManager(graphApiClient, usersRepository);

            var updateUserRequest = new UpdateUserRequest
            {
                GivenName = "FirstName",
                Surname = "LastName"
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => userManager.UpdateUserAsync(userId, updateUserRequest));
            Mock.Assert(() => graphApiClient.Users, Occurs.Once());
        }
    }
}