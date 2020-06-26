using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Dto.Groups;
using TQ.Authentication.Core.Requests.Groups;
using TQ.Authentication.Services.Interfaces;
using Xunit;

namespace TQ.Authentication.UnitTests.Controllers.GroupsController
{
    public class CreateGroupAsync
    {
        private IGroupManager groupManager;

        [Fact]
        public async Task CreateGroup_ReturnsOkObjectResult()
        {
            // Arrange
            var createdAt = DateTime.Now;
            var groupId = Guid.NewGuid();

            groupManager = Mock.Create<IGroupManager>();
            Mock.Arrange(() => groupManager.CreateGroupAsync(Arg.IsAny<CreateGroupRequest>()))
                .TaskResult(new GetGroupGetDto
                {
                    Id = groupId,
                    Name = "SecurityPortal",
                    Description = "Security Portal Group",
                    IsActive = true,
                    CreatedAt = createdAt
                });

            var groupsController = CreateSut();

            var groupModel = new CreateGroupRequest
            {
                Name = "SecurityPortal",
                Description = "Security Portal Group",
                ServiceUrl = "https://localhost",
                IsActive = true
            };

            // Act
            var result = await groupsController.CreateGroupAsync(groupModel);

            var objectResult = (ObjectResult)result.Result;
            var resultGroupId = objectResult.Value.ToString();

            // Assert
            Assert.Equal(201, objectResult.StatusCode);
            Assert.Contains(groupId.ToString(), resultGroupId);

            Mock.Assert(() => groupManager.CreateGroupAsync(Arg.IsAny<CreateGroupRequest>()), Occurs.Once());
        }

        [Fact]
        public async Task CreateGroup_GroupManagerFails_ThrowsException()
        {
            // Arrange
            groupManager = Mock.Create<IGroupManager>();
            Mock.Arrange(() => groupManager.CreateGroupAsync(Arg.IsAny<CreateGroupRequest>()))
                .Throws<Exception>();

            var groupsController = CreateSut();

            var groupModel = new CreateGroupRequest
            {
                Name = "Security Portal",
                Description = "Security Portal Group",
                ServiceUrl = "https://localhost",
                IsActive = true
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => groupsController.CreateGroupAsync(groupModel));

            Mock.Assert(() => groupManager.CreateGroupAsync(Arg.IsAny<CreateGroupRequest>()), Occurs.Once());
        }

        private API.Controllers.GroupsController CreateSut()
        {
            return new API.Controllers.GroupsController(groupManager);
        }
    }
}