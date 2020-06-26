using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Dto.Groups;
using TQ.Authentication.Services.Interfaces;
using Xunit;

namespace TQ.Authentication.UnitTests.Controllers.GroupsController
{
    public class GetByIdAsync
    {
        private IGroupManager groupManager;

        [Fact]
        public async Task GetById_ReturnsOkObjectResult()
        {
            // Arrange
            var createdAt = DateTime.Now;

            groupManager = Mock.Create<IGroupManager>();
            Mock.Arrange(() => groupManager.GetGroupAsync(Arg.IsAny<Guid>()))
                .TaskResult(new GetGroupGetDto
                {
                    Id = Guid.NewGuid(),
                    Name = "SecurityPortal",
                    Description = "Security Portal Group",
                    IsActive = true,
                    CreatedAt = createdAt
                });

            var groupsController = CreateSut();

            // Act
            var result = await groupsController.GetByIdAsync(Guid.NewGuid());

            var okResult = (OkObjectResult)result.Result;

            var group = (GetGroupGetDto)okResult.Value;

            // Assert
            Assert.Equal("SecurityPortal", group.Name);
            Assert.Equal("Security Portal Group", group.Description);
            Assert.Equal(createdAt, group.CreatedAt);
            Assert.True(group.IsActive);

            Mock.Assert(() => groupManager.GetGroupAsync(Arg.IsAny<Guid>()), Occurs.Once());
        }

        [Fact]
        public async Task GetById_GroupManagerthrowsException_ThrowsException()
        {
            // Arrange
            groupManager = Mock.Create<IGroupManager>();
            Mock.Arrange(() => groupManager.GetGroupAsync(Arg.IsAny<Guid>()))
                .Throws<Exception>();

            var groupsController = CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => groupsController.GetByIdAsync(Guid.NewGuid()));

            Mock.Assert(() => groupManager.GetGroupAsync(Arg.IsAny<Guid>()), Occurs.Once());
        }

        [Fact]
        public async Task GetById_InvalidGroupId_ThrowsException()
        {
            // Arrange
            groupManager = Mock.Create<IGroupManager>();
            Mock.Arrange(() => groupManager.GetGroupAsync(Arg.IsAny<Guid>()))
                .Throws<Exception>();

            var groupsController = CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => groupsController.GetByIdAsync(Guid.Empty));

            Mock.Assert(() => groupManager.GetGroupAsync(Arg.IsAny<Guid>()), Occurs.Once());
        }

        private API.Controllers.GroupsController CreateSut()
        {
            return new API.Controllers.GroupsController(groupManager);
        }
    }
}