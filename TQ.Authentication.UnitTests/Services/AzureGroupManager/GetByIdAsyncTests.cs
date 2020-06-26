using FluentAssertions;
using System;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.ExternalServices.GraphAPI;
using TQ.Authentication.ExternalServices.GraphAPI.Models;
using TQ.Authentication.Services.Interfaces;
using Xunit;

namespace TQ.Authentication.UnitTests.Services.AzureGroupManager
{
    public class GetByIdAsyncTests
    {
        private readonly IGraphApiClient graphApiClient = Mock.Create<IGraphApiClient>();

        [Fact]
        public async Task GetGroupByIdAsyncReturnsGroupSuccess()
        {
            // Arrange
            Mock.Arrange(() => graphApiClient.Groups.GetGroupAsync(Arg.AnyGuid)).TaskResult(new AzureGroup { Id = Guid.NewGuid() });
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetByIdAsync(Guid.NewGuid());

            // Assert
            result.Should().NotBeNull();
            Mock.Assert(() => graphApiClient.Groups.GetGroupAsync(Arg.AnyGuid), Occurs.Once());
        }

        [Fact]
        public async Task GetByIdAsyncThrowsAzureGroupManagerExceptionWhenGroupNotFound()
        {
            // Arrange
            Mock.Arrange(() => graphApiClient.Groups.GetGroupAsync(Arg.AnyGuid)).Throws<Exception>();
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<TQAuthenticationException>(() => sut.GetByIdAsync(Guid.NewGuid()));
        }

        private IAzureGroupManager CreateSut()
        {
            return new Authentication.Services.AzureGroupManager(graphApiClient);
        }
    }
}