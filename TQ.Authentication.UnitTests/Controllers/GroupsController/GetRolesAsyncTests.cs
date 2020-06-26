using System;
using System.Net;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Authentication.Core.Exceptions;
using TQ.Authentication.Core.Requests.Paging;
using TQ.Authentication.Services.Interfaces;
using Xunit;

namespace TQ.Authentication.UnitTests.Controllers.GroupsController
{
    public class GetRolesAsyncTests
    {
        private readonly IGroupManager mockGroupManager = Mock.Create<IGroupManager>();

        [Fact]
        public async Task GetRolesAsyncThrowsTQAuthenticationExceptionWhenGroupNotFound()
        {
            // Arrange
            Mock.Arrange(() => mockGroupManager.GetRolesAsync(Arg.AnyGuid, Arg.IsAny<GetPagedRequest>()))
                .Throws(new TQAuthenticationException(HttpStatusCode.NotFound, string.Empty));

            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<TQAuthenticationException>(() => sut.GetRolesAsync(Guid.NewGuid(), new GetPagedRequest()));
            Mock.Assert(() => mockGroupManager.GetRolesAsync(Arg.AnyGuid, Arg.IsAny<GetPagedRequest>()), Occurs.Once());
        }

        private API.Controllers.GroupsController CreateSut()
        {
            return new API.Controllers.GroupsController(mockGroupManager);
        }
    }
}