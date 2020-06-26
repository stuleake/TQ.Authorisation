using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.Core.Requests.Users;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.Data.Repositories;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.ExternalServices.GraphAPI.Models;
using Xunit;

namespace TQ.Authentication.UnitTests.Services.UserManager
{
    public class GetUsersAsync
    {
        [Fact]
        public async Task GetUsersAsync_ReturnsUserCollection()
        {
            // Arrange
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();

            var userGraph = Mock.Create<IUserGraph>();
            Mock.Arrange(() => userGraph.GetPagedUsersAsync(Arg.IsAny<AzureUserFilter>()))
                .TaskResult(new AzurePagedUsers
                {
                    NextPageToken = null,
                    Users = new List<AzureUser>
                    {
                        new AzureUser
                        {
                            Id = userId1,
                            DisplayName = "FirstName LastName 1",
                            GivenName = "FirstName1",
                            Surname = "LastName1"
                        },
                        new AzureUser
                        {
                            Id = userId2,
                            DisplayName = "FirstName LastName 2",
                            GivenName = "FirstName2",
                            Surname = "LastName2"
                        }
                    }
                });

            var graphApiClient = Mock.Create<IGraphApiClient>();

            Mock.Arrange(() => graphApiClient.Users)
                .Returns(userGraph);

            var usersRepository = Mock.Create<IUsersRepository<User>>();
            Mock.Arrange(() => usersRepository.GetUsersByIdsAsync(Arg.IsAny<IEnumerable<Guid>>()))
                .TaskResult(new List<User>
                {
                    new User { Id = userId1 },
                    new User { Id = userId2 }
                });

            var userManager = new Authentication.Services.UserManager(graphApiClient, usersRepository);

            // Act
            var users = await userManager.GetUsersAsync(new GetFilteredUsersRequest { FirstName = "FirstName", LastName = "LastName", NextPageToken = "sometoken" });

            // Assert
            Mock.Assert(() => userGraph.GetPagedUsersAsync(Arg.IsAny<AzureUserFilter>()), Occurs.Once());
            Mock.Assert(() => graphApiClient.Users, Occurs.Once());
            Mock.Assert(() => usersRepository.GetUsersByIdsAsync(Arg.IsAny<IEnumerable<Guid>>()), Occurs.Once());

            Assert.Equal(2, users.Users.Count());
        }

        [Fact]
        public async Task GetUsersAsync_GraphApiReturnsNull_ThrowsException()
        {
            // Arrange
            var userGraph = Mock.Create<IUserGraph>();
            Mock.Arrange(() => userGraph.GetPagedUsersAsync(Arg.IsAny<AzureUserFilter>()))
                .TaskResult(Arg.IsNull<AzurePagedUsers>());

            var graphApiClient = Mock.Create<IGraphApiClient>();
            Mock.Arrange(() => graphApiClient.Users)
                .Returns(userGraph);

            var usersRepository = Mock.Create<IUsersRepository<User>>();
            Mock.Arrange(() => usersRepository.GetUsersByIdsAsync(Arg.IsAny<IEnumerable<Guid>>()))
                .TaskResult(Arg.IsAny<List<User>>());

            Mock.Arrange(() => graphApiClient.Users.GetPagedUsersAsync(Arg.IsAny<AzureUserFilter>())).Throws(new TQAuthenticationException(HttpStatusCode.NotFound, string.Empty));

            var userManager = new Authentication.Services.UserManager(graphApiClient, usersRepository);

            // Act & Assert
            await Assert.ThrowsAsync<TQAuthenticationException>(() => userManager.GetUsersAsync(new GetFilteredUsersRequest()));

            Mock.Assert(() => userGraph.GetPagedUsersAsync(Arg.IsAny<AzureUserFilter>()), Occurs.Once());
            Mock.Assert(() => graphApiClient.Users, Occurs.Once());
            Mock.Assert(() => usersRepository.GetUsersByIdsAsync(Arg.IsAny<IEnumerable<Guid>>()), Occurs.Never());
        }

        [Fact]
        public async Task GetUsersAsync_GraphApiReturnsEmptyCollection_ReturnsEmptyUserList()
        {
            // Arrange
            var userGraph = Mock.Create<IUserGraph>();
            Mock.Arrange(() => userGraph.GetPagedUsersAsync(Arg.IsAny<AzureUserFilter>()))
                .TaskResult(new AzurePagedUsers
                {
                    NextPageToken = null,
                    Users = Enumerable.Empty<AzureUser>()
                });

            var graphApiClient = Mock.Create<IGraphApiClient>();
            Mock.Arrange(() => graphApiClient.Users)
                .Returns(userGraph);

            var usersRepository = Mock.Create<IUsersRepository<User>>();
            Mock.Arrange(() => usersRepository.GetUsersByIdsAsync(Arg.IsAny<IEnumerable<Guid>>()))
                .TaskResult(Enumerable.Empty<User>());

            var userManager = new Authentication.Services.UserManager(graphApiClient, usersRepository);

            // Act & Assert
            var users = await userManager.GetUsersAsync(new GetFilteredUsersRequest());

            Mock.Assert(() => userGraph.GetPagedUsersAsync(Arg.IsAny<AzureUserFilter>()), Occurs.Once());
            Mock.Assert(() => graphApiClient.Users, Occurs.Once());
            Mock.Assert(() => usersRepository.GetUsersByIdsAsync(Arg.IsAny<IEnumerable<Guid>>()), Occurs.Once());

            Assert.Empty(users.Users);
        }
    }
}