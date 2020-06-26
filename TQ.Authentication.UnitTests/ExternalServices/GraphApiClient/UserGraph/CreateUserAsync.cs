using Microsoft.Graph;
using System;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.ExternalServices.GraphAPI.Models;
using Xunit;

namespace TQ.Authentication.UnitTests.ExternalServices.GraphApiClient.UserGraph
{
    public class CreateUserAsync
    {
        private IGraphServiceClientFactory graphServiceClientFactory;

        [Fact]
        public async Task CreateUserAsync_CreatesUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var model = new AzureUser
            {
                DisplayName = "Display Name",
                Password = "P455w0rD123!"
            };

            var graphServiceClient = Mock.Create<IGraphServiceClient>();
            Mock.Arrange(
                () => graphServiceClient.Users.Request().AddAsync(Arg.IsAny<User>()))
                .TaskResult(new User
                {
                    Id = userId.ToString(),
                    AccountEnabled = true,
                    CreationType = "LocalAccount",
                    DisplayName = "Display Name",
                    MailNickname = "mail nickname",
                    PasswordProfile = new PasswordProfile
                    {
                        ForceChangePasswordNextSignIn = true,
                        Password = "P455w0rD123!"
                    }
                });

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(graphServiceClient);

            var graphApiClient = CreateSut();

            // Act
            var newUserId = await graphApiClient.CreateUserAsync(model);

            // Assert
            Assert.Equal(userId, newUserId);

            Mock.Assert(() => graphServiceClient.Users, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task CreateUserAsync_FactoryCreatesNull_ThrowsException()
        {
            // Arrange
            AzureUser model = new AzureUser();

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(Arg.IsNull<IGraphServiceClient>());

            var userGraph = CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(
                () => userGraph.CreateUserAsync(model));

            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task CreateUserAsync_GraphApiFails_ThrowsServiceException()
        {
            // Arrange
            var model = new AzureUser
            {
                DisplayName = "Display Name",
                Password = "P455w0rD123!"
            };

            var graphServiceClient = Mock.Create<IGraphServiceClient>();
            Mock.Arrange(
                () => graphServiceClient.Users)
                .Throws(new ServiceException(null, null));

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(graphServiceClient);

            var userGraph = CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<TQAuthenticationException>(
                () => userGraph.CreateUserAsync(model));

            Mock.Assert(() => graphServiceClient.Users, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        [Fact]
        public async Task CreateUserAsync_GraphApiFails_ThrowsSystemException()
        {
            // Arrange
            var model = new AzureUser
            {
                DisplayName = "Display Name",
                Password = "P455w0rD123!"
            };

            var graphServiceClient = Mock.Create<IGraphServiceClient>();
            Mock.Arrange(
                () => graphServiceClient.Users)
                .Throws(new Exception());

            graphServiceClientFactory = Mock.Create<IGraphServiceClientFactory>();
            Mock.Arrange(() => graphServiceClientFactory.Create())
                .Returns(graphServiceClient);

            var userGraph = CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                 () => userGraph.CreateUserAsync(model));

            Mock.Assert(() => graphServiceClient.Users, Occurs.Once());
            Mock.Assert(() => graphServiceClientFactory.Create(), Occurs.Once());
        }

        private Authentication.ExternalServices.GraphAPI.UserGraph CreateSut()
        {
            return new Authentication.ExternalServices.GraphAPI.UserGraph(
                new GraphApiClientConfiguration
                {
                    TenantName = "terraquest",
                    DefaultPageSize = 1
                },
                graphServiceClientFactory);
        }
    }
}