using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Requests.Groups;
using TQ.Authentication.Services.Interfaces;
using Xunit;

namespace TQ.Authentication.UnitTests.Controllers.GroupsController
{
    public class UpdateGroupAsync
    {
        private IGroupManager groupManager;

        [Fact]
        public async Task UpdateGroup_ReturnsOk204()
        {
            // Arrange
            var groupId = Guid.NewGuid();

            groupManager = Mock.Create<IGroupManager>();
            Mock.Arrange(() => groupManager.UpdateGroupAsync(Arg.IsAny<Guid>(), Arg.IsAny<UpdateGroupRequest>()))
                .Returns(Task.Factory.StartNew(() => string.Empty));

            var groupsController = CreateSut();

            var groupRequest = new UpdateGroupRequest
            {
                Name = "SecurityPortal",
                Description = "Security Portal Group",
                ServiceUrl = "https://localhost",
                IsActive = true
            };

            // Act
            var result = await groupsController.UpdateGroupAsync(groupId, groupRequest);

            var statusCodeResult = (StatusCodeResult)result;

            // Assert
            Assert.Equal(204, statusCodeResult.StatusCode);

            Mock.Assert(() => groupManager.UpdateGroupAsync(Arg.IsAny<Guid>(), Arg.IsAny<UpdateGroupRequest>()), Occurs.Once());
        }

        [Fact]
        public async Task UpdateGroup_GroupManagerFails_ThrowsException()
        {
            var groupId = Guid.NewGuid();

            groupManager = Mock.Create<IGroupManager>();
            Mock.Arrange(() => groupManager.UpdateGroupAsync(Arg.IsAny<Guid>(), Arg.IsAny<UpdateGroupRequest>()))
                .Throws<Exception>();

            var groupsController = CreateSut();

            var groupRequest = new UpdateGroupRequest
            {
                Name = "SecurityPortal",
                Description = "Security Portal Group",
                ServiceUrl = "https://localhost",
                IsActive = true
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => groupsController.UpdateGroupAsync(groupId, groupRequest));

            Mock.Assert(() => groupManager.UpdateGroupAsync(Arg.IsAny<Guid>(), Arg.IsAny<UpdateGroupRequest>()), Occurs.Once());
        }

        private API.Controllers.GroupsController CreateSut()
        {
            return new API.Controllers.GroupsController(groupManager);
        }
    }
}