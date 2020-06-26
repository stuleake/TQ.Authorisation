using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Geocoding.API.Controllers;
using TQ.Geocoding.Dto.Location;
using TQ.Geocoding.Dtos.Address;
using TQ.Geocoding.Service.Interface.Audit;
using TQ.Geocoding.Service.Interface.Search;
using Xunit;

namespace TQ.Geocoding.API.Test.Controllers
{
    public class LocationControllerTests
    {
        private readonly ILogger<LocationController> locationControllerLogger = Mock.Create<ILogger<LocationController>>();
        private readonly ILocationCoordinateSearch mockLocationCoordinateSearch = Mock.Create<ILocationCoordinateSearch>();
        private readonly IReadOnlyLocationAuditLogger mockReadOnlyLocationAuditLogger = Mock.Create<IReadOnlyLocationAuditLogger>();

        [Fact]
        public async Task GetLocationCoordinatesByIdAsyncReturnsOkObjectResult()
        {
            // Arrange
            var id = "id";
            IEnumerable<LocationCoordinateDto> dtoList = new List<LocationCoordinateDto> { new LocationCoordinateDto { LocationId = "osgb4000000028045751" } };
            Mock.Arrange(() => mockLocationCoordinateSearch.GetByIdAsync(id)).Returns(Task.FromResult(dtoList)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var actual = await sut.GetLocationCoordinatesByIdAsync(id);

            // Assert
            Mock.Assert(() => mockLocationCoordinateSearch.GetByIdAsync(id), Occurs.Once());
            var okActionResult = Assert.IsType<OkObjectResult>(actual);
        }

        [Fact]
        public async Task GetAuditLogAsyncReturnsOkObjectResult()
        {
            // Arrange
            IEnumerable<LocationSearchAuditLogDto> dtoList = new List<LocationSearchAuditLogDto>
            {
                new LocationSearchAuditLogDto
                {
                    LocationSearch = "location search",
                    Id = Guid.NewGuid(),
                    LogDate = DateTime.Now,
                    ResultCount = 5
                }
            };

            Mock.Arrange(() => mockReadOnlyLocationAuditLogger.GetAllAsync()).Returns(Task.FromResult(dtoList)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetAuditLog();

            // Assert
            Mock.Assert(() => mockReadOnlyLocationAuditLogger.GetAllAsync(), Occurs.Once());
            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dtoList, okActionResult.Value);
        }

        [Fact]
        public async Task GetAuditLogByDatesAsyncReturnsOkObjectResult()
        {
            // Arrange
            IEnumerable<LocationSearchAuditLogDto> dtoList = new List<LocationSearchAuditLogDto>
            {
                new LocationSearchAuditLogDto
                {
                    LocationSearch = "location search",
                    Id = Guid.NewGuid(),
                    LogDate = DateTime.Now,
                    ResultCount = 5
                }
            };

            Mock.Arrange(() => mockReadOnlyLocationAuditLogger.GetAllByDateRangeAsync(Arg.IsAny<DateTime>(), Arg.IsAny<DateTime>())).Returns(Task.FromResult(dtoList)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetAuditLogByDates(DateTime.Now, DateTime.Now);

            // Assert
            Mock.Assert(() => mockReadOnlyLocationAuditLogger.GetAllByDateRangeAsync(Arg.IsAny<DateTime>(), Arg.IsAny<DateTime>()), Occurs.Once());
            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dtoList, okActionResult.Value);
        }

        [Fact]
        public async Task GetAuditLogByResultsAsyncReturnsOkObjectResult()
        {
            // Arrange
            IEnumerable<LocationSearchAuditLogDto> dtoList = new List<LocationSearchAuditLogDto>
            {
                new LocationSearchAuditLogDto
                {
                    LocationSearch = "location search",
                    Id = Guid.NewGuid(),
                    LogDate = DateTime.Now,
                    ResultCount = 5
                }
            };

            Mock.Arrange(() => mockReadOnlyLocationAuditLogger.GetAllByResultCountRangeAsync(Arg.IsAny<int>(), Arg.IsAny<int>())).Returns(Task.FromResult(dtoList)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetAuditLogByResults(1, 100);

            // Assert
            Mock.Assert(() => mockReadOnlyLocationAuditLogger.GetAllByResultCountRangeAsync(Arg.IsAny<int>(), Arg.IsAny<int>()), Occurs.Once());
            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dtoList, okActionResult.Value);
        }

        [Fact]
        public async Task GetAuditLogBySearchAsyncReturnsOkObjectResult()
        {
            // Arrange
            IEnumerable<LocationSearchAuditLogDto> dtoList = new List<LocationSearchAuditLogDto>
            {
                new LocationSearchAuditLogDto
                {
                    LocationSearch = "location search",
                    Id = Guid.NewGuid(),
                    LogDate = DateTime.Now,
                    ResultCount = 5
                }
            };

            Mock.Arrange(() => mockReadOnlyLocationAuditLogger.GetAllBySearchTextAsync(Arg.IsAny<string>())).Returns(Task.FromResult(dtoList)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetAuditLogBySearch("search text");

            // Assert
            Mock.Assert(() => mockReadOnlyLocationAuditLogger.GetAllBySearchTextAsync(Arg.IsAny<string>()), Occurs.Once());
            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dtoList, okActionResult.Value);
        }

        private LocationController CreateSut()
        {
            return new LocationController(locationControllerLogger, mockLocationCoordinateSearch, mockReadOnlyLocationAuditLogger);
        }
    }
}