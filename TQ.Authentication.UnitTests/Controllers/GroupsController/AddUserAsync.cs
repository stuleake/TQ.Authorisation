using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Dto.Groups;
using TQ.Authentication.Services.Interfaces;
using Xunit;

namespace TQ.Authentication.UnitTests.Controllers.GroupsController
{
    public class AddUserAsync
    {
        private IGroupManager groupManager;

        [Fact]
        public async Task AddUserAsync_Returns201ObjectResult()
        {
            // Arrange
            var userGroupId = Guid.NewGuid();

            groupManager = Mock.Create<IGroupManager>();
            Mock.Arrange(() => groupManager.AddUserAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .TaskResult(new UserGroupDto
                {
                    UserAlreadyInGroup = false,
                    UserGroupId = userGroupId
                });

            var groupsController = CreateSut();

            // Act
            var result = await groupsController.AddUserAsync(Guid.NewGuid(), Guid.NewGuid());

            var objectResult = (ObjectResult)result;

            // Assert
            Assert.Equal((int)HttpStatusCode.Created, objectResult.StatusCode);

            Mock.Assert(() => groupManager.AddUserAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<Guid>()), Occurs.Once());
        }

        [Fact]
        public async Task AddUserAsync_AlreadyExists_Returns200ObjectResult()
        {
            // Arrange
            var userGroupId = Guid.NewGuid();

            groupManager = Mock.Create<IGroupManager>();
            Mock.Arrange(() => groupManager.AddUserAsync(Arg.IsAny<Guid>(), Arg.IsAny<Guid>()))
                .TaskResult(new UserGroupDto
                {
                    UserAlreadyInGroup = true,
                    UserGroupId = userGroupId
                });

            var groupsController = CreateSut();

            // Act
            var result = await groupsController.AddUserAsync(Guid.NewGuid(), Guid.NewGuid());

            var objectResult = (ObjectResult)result;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);

            Mock.Assert(() => groupManager.AddUserAsync(
                Arg.IsAny<Guid>(), Arg.IsAny<Guid>()), Occurs.Once());
        }

        private API.Controllers.GroupsController CreateSut()
        {
            return new API.Controllers.GroupsController(groupManager);
        }
    }
}