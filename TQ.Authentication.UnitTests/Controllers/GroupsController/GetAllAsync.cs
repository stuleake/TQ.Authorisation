using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Dto.Groups;
using TQ.Authentication.Services.Interfaces;
using Xunit;

namespace TQ.Authentication.UnitTests.Controllers.GroupsController
{
    public class GetAllAsync
    {
        private IGroupManager groupManager;

        [Fact]
        public async Task GetAll_ReturnsOkObjectResult()
        {
            // Arrange
            groupManager = Mock.Create<IGroupManager>();
            Mock.Arrange(() => groupManager.GetGroupsAsync())
                .TaskResult(new List<GetGroupGetDto>
                {
                    new GetGroupGetDto
                    {
                        Id = Guid.NewGuid(),
                        Name = "SecurityPortal",
                        Description = "Security Portal Group",
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    },
                    new GetGroupGetDto
                    {
                        Id = Guid.NewGuid(),
                        Name = "PP2",
                        Description = "PP2 Group",
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    }
                });

            var groupsController = CreateSut();

            // Act
            var result = await groupsController.GetAllAsync();

            var okResult = (OkObjectResult)result.Result;

            var groups = (IEnumerable<GetGroupGetDto>)okResult.Value;

            var groupList = groups.ToList();

            // Assert
            Assert.Equal(2, groupList.Count);
            Assert.Contains(groupList, x => x.Name == "SecurityPortal");

            Mock.Assert(() => groupManager.GetGroupsAsync(), Occurs.Once());
        }

        [Fact]
        public async Task GetAll_GroupManagerThrowsException_ThrowsException()
        {
            // Arrange
            groupManager = Mock.Create<IGroupManager>();
            Mock.Arrange(() => groupManager.GetGroupsAsync())
                .Throws<Exception>();

            var groupsController = CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => groupsController.GetAllAsync());

            Mock.Assert(() => groupManager.GetGroupsAsync(), Occurs.Once());
        }

        private API.Controllers.GroupsController CreateSut()
        {
            return new API.Controllers.GroupsController(groupManager);
        }
    }
}