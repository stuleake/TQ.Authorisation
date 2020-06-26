using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Services.Interfaces;
using Xunit;

namespace TQ.Authentication.UnitTests.Controllers.GroupsController
{
    public class RemoveUserAsync
    {
        private IGroupManager groupManager;

        [Fact]
        public async Task RemoveUserAsync_RemovesUser()
        {
            // Arrange
            groupManager = Mock.Create<IGroupManager>();
            Mock.Arrange(() => groupManager.RemoveUserAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var groupsController = CreateSut();

            // Act
            var result = await groupsController.RemoveUserAsync(Guid.NewGuid(), Guid.NewGuid());

            var noContentResult = (NoContentResult)result;

            // Assert
            Assert.Equal(204, noContentResult.StatusCode);

            Mock.Assert(() => groupManager.RemoveUserAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<Guid>()), Occurs.Once());
        }

        private API.Controllers.GroupsController CreateSut()
        {
            return new API.Controllers.GroupsController(groupManager);
        }
    }
}