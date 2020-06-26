using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Geocoding.API.Controllers;
using TQ.Geocoding.Service.Interface.Search;
using Xunit;

namespace TQ.Geocoding.API.Test.Controllers
{
    public class PostcodeControllerTests
    {
        private readonly ILogger<PostcodeController> postcodeControllerLogger = Mock.Create<ILogger<PostcodeController>>();
        private readonly ILocationPostcodeCoordinateSearch mockLocationPostcodeCoordinateSearch = Mock.Create<ILocationPostcodeCoordinateSearch>();

        [Fact]
        public async Task GetPostcodeCoordinatesByIdAsyncReturnsOkObjectResult()
        {
            // Arrange
            var id = "B912PJ";
            Mock.Arrange(() => mockLocationPostcodeCoordinateSearch.GetByIdAsync(id)).Returns(Task.FromResult(new Dto.Dtos.PostcodeCoordinateDto())).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var actual = await sut.GetPostcodeCoordinatesByIdAsync(id);

            // Assert
            Mock.Assert(() => mockLocationPostcodeCoordinateSearch.GetByIdAsync(id), Occurs.Once());
            var okActionResult = Assert.IsType<OkObjectResult>(actual);
        }

        private PostcodeController CreateSut()
        {
            return new PostcodeController(postcodeControllerLogger, mockLocationPostcodeCoordinateSearch);
        }
    }
}