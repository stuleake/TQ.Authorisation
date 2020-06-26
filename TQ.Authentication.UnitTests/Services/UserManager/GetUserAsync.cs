using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.Data.Repositories;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.ExternalServices.GraphAPI.Models;
using Xunit;
using User = TQ.Authentication.Data.Entities.User;

namespace TQ.Authentication.UnitTests.Services.UserManager
{
    public class GetUserAsync
    {
        [Fact]
        public async Task GetUserAsync_ReturnsUser()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var userGraph = Mock.Create<IUserGraph>();
            Mock.Arrange(() => userGraph.GetUserAsync(Arg.IsAny<Guid>()))
                .TaskResult(new AzureUser
                {
                    Id = userId
                });

            var graphApiClient = Mock.Create<IGraphApiClient>();
            Mock.Arrange(() => graphApiClient.Users)
                .Returns(userGraph);

            var usersRepository = Mock.Create<IUsersRepository<User>>();
            Mock.Arrange(() => usersRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<User, bool>>>()))
                .TaskResult(true);

            var userManager = new Authentication.Services.UserManager(graphApiClient, usersRepository);

            // Act
            var user = await userManager.GetUserAsync(userId);

            // Assert
            Mock.Assert(() => userGraph.GetUserAsync(Arg.IsAny<Guid>()), Occurs.Once());
            Mock.Assert(() => graphApiClient.Users, Occurs.Once());
            Mock.Assert(() => usersRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<User, bool>>>()), Occurs.Once());

            Assert.NotNull(user);
        }

        [Fact]
        public async Task GetUserAsync_GraphApiNull_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var userGraph = Mock.Create<IUserGraph>();
            Mock.Arrange(() => userGraph.GetUserAsync(Arg.IsAny<Guid>()))
                .TaskResult(Arg.IsNull<AzureUser>());

            var graphApiClient = Mock.Create<IGraphApiClient>();
            Mock.Arrange(() => graphApiClient.Users)
                .Returns(userGraph);

            var usersRepository = Mock.Create<IUsersRepository<User>>();
            Mock.Arrange(() => usersRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<User, bool>>>()))
                .TaskResult(true);

            Mock.Arrange(() => graphApiClient.Users.GetUserAsync(Arg.IsAny<Guid>())).Throws(new TQAuthenticationException(System.Net.HttpStatusCode.NotFound, ""));

            var userManager = new Authentication.Services.UserManager(graphApiClient, usersRepository);

            // Assert
            await Assert.ThrowsAsync<TQAuthenticationException>(() => userManager.GetUserAsync(userId));

            Mock.Assert(() => userGraph.GetUserAsync(Arg.IsAny<Guid>()), Occurs.Once());
            Mock.Assert(() => graphApiClient.Users, Occurs.Once());
            Mock.Assert(() => usersRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<User, bool>>>()), Occurs.Never());
        }

        [Fact]
        public async Task GetUserAsync_SqlNotExist_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var userGraph = Mock.Create<IUserGraph>();
            Mock.Arrange(() => userGraph.GetUserAsync(Arg.IsAny<Guid>()))
                .TaskResult(new AzureUser
                {
                    Id = userId
                });

            var graphApiClient = Mock.Create<IGraphApiClient>();
            Mock.Arrange(() => graphApiClient.Users)
                .Returns(userGraph);

            var usersRepository = Mock.Create<IUsersRepository<User>>();
            Mock.Arrange(() => usersRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<User, bool>>>()))
                .TaskResult(false);

            var userManager = new Authentication.Services.UserManager(graphApiClient, usersRepository);

            // Assert
            await Assert.ThrowsAsync<TQAuthenticationException>(() => userManager.GetUserAsync(userId));

            Mock.Assert(() => userGraph.GetUserAsync(Arg.IsAny<Guid>()), Occurs.Once());
            Mock.Assert(() => graphApiClient.Users, Occurs.Once());
            Mock.Assert(() => usersRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<User, bool>>>()), Occurs.Once());
        }

        [Fact]
        public async Task GetUserAsync_GraphApiFails_ThrowsServiceException()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var userGraph = Mock.Create<IUserGraph>();
            Mock.Arrange(() => userGraph.GetUserAsync(Arg.IsAny<Guid>()))
                .Throws<Exception>();

            var graphApiClient = Mock.Create<IGraphApiClient>();
            Mock.Arrange(() => graphApiClient.Users)
                .Returns(userGraph);

            var usersRepository = Mock.Create<IUsersRepository<User>>();
            Mock.Arrange(() => usersRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<User, bool>>>()))
                .TaskResult(false);

            var userManager = new Authentication.Services.UserManager(graphApiClient, usersRepository);

            // Assert
            await Assert.ThrowsAsync<Exception>(() => userManager.GetUserAsync(userId));

            Mock.Assert(() => userGraph.GetUserAsync(Arg.IsAny<Guid>()), Occurs.Once());
            Mock.Assert(() => graphApiClient.Users, Occurs.Once());
            Mock.Assert(() => usersRepository.IsAnyMatchedAsync(Arg.IsAny<Expression<Func<User, bool>>>()), Occurs.Never());
        }
    }
}