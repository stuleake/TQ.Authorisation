using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telerik.JustMock;
using TQ.Geocoding.API.Controllers;
using TQ.Geocoding.Dto.Address;
using TQ.Geocoding.Dtos.Address;
using TQ.Geocoding.Requests;
using TQ.Geocoding.Service.Interface.Audit;
using TQ.Geocoding.Service.Interface.Search;
using Xunit;

namespace TQ.Geocoding.API.Test.Controllers
{
    public class AddressControllerTests
    {
        private readonly ILogger<AddressController> addressControllerLogger = Mock.Create<ILogger<AddressController>>();
        private readonly IAddressUprnSearch mockAddressUrpnSearch = Mock.Create<IAddressUprnSearch>();
        private readonly IAddressEastingNorthingSearch mockAddressEastingNorthingSearch = Mock.Create<IAddressEastingNorthingSearch>();
        private readonly IAddressLongitudeLatitudeSearch mockAddressLongitudeLatitudeSearch = Mock.Create<IAddressLongitudeLatitudeSearch>();
        private readonly IAddressPostcodeSearch mockAddressPostcodeSearch = Mock.Create<IAddressPostcodeSearch>();
        private readonly IAddressTextSearch mockAddressTextSearch = Mock.Create<IAddressTextSearch>();
        private readonly IReadOnlyAddressAuditLogger mockReadOnlyAddressAuditLogger = Mock.Create<IReadOnlyAddressAuditLogger>();

        private readonly string postCode = "ts11 8hr";
        private readonly GetAddressByEastingNorthingRequest getAddressByEastingNorthingRequest = new GetAddressByEastingNorthingRequest
        {
            Radius = 100,
            XCoordinateEasting = 200,
            YCoordinateNorthing = 300
        };

        private readonly GetAddressByLongitudeLatitudeRequest getAddressByLongitudeLatitudeRequest = new GetAddressByLongitudeLatitudeRequest
        {
            Radius = 100,
            Longitude = 200,
            Latitude = 300
        };

        #region Text

        [Fact]
        public async Task GetSimpleAddressesByTextAsyncReturnsOkObjectResult()
        {
            // Arrange
            IEnumerable<SimpleAddressDto> dtoList = new List<SimpleAddressDto> { new SimpleAddressDto { Uprn = 500 } };
            var getAddressRequest = this.NewGetAddressByTextRequest();

            Mock.Arrange(() => mockAddressTextSearch.GetSimpleAddressByTextAsync(getAddressRequest)).Returns(Task.FromResult(dtoList)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetSimpleAddressesByTextAsync(getAddressRequest);

            // Assert
            Mock.Assert(() => mockAddressTextSearch.GetSimpleAddressByTextAsync(getAddressRequest), Occurs.Once());
            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dtoList, okActionResult.Value);
        }

        [Fact]
        public async Task GetSimpleWelshAddressesByTextAsyncReturnsOkObjectResult()
        {
            // Arrange
            IEnumerable<SimpleWelshAddressDto> dtoList = new List<SimpleWelshAddressDto> { new SimpleWelshAddressDto { Uprn = 500 } };
            var getAddressRequest = this.NewGetAddressByTextRequest();

            Mock.Arrange(() => mockAddressTextSearch.GetSimpleWelshAddressByTextAsync(getAddressRequest)).Returns(Task.FromResult(dtoList)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetSimpleWelshAddressesByTextAsync(getAddressRequest);

            // Assert
            Mock.Assert(() => mockAddressTextSearch.GetSimpleWelshAddressByTextAsync(getAddressRequest), Occurs.Once());
            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dtoList, okActionResult.Value);
        }

        [Fact]
        public async Task GetFullAddressesByTextAsyncReturnsOkObjectResult()
        {
            // Arrange
            IEnumerable<FullAddressDto> dtoList = new List<FullAddressDto> { new FullAddressDto { Uprn = 500 } };
            var getAddressRequest = this.NewGetAddressByTextRequest();

            Mock.Arrange(() => mockAddressTextSearch.GetFullAddressByTextAsync(getAddressRequest)).Returns(Task.FromResult(dtoList)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetFullAddressesByTextAsync(getAddressRequest);

            // Assert
            Mock.Assert(() => mockAddressTextSearch.GetFullAddressByTextAsync(getAddressRequest), Occurs.Once());
            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dtoList, okActionResult.Value);
        }

        [Fact]
        public async Task GetFullWelshAddressesByTextAsyncReturnsOkObjectResult()
        {
            // Arrange
            IEnumerable<FullWelshAddressDto> dtoList = new List<FullWelshAddressDto> { new FullWelshAddressDto { Uprn = 500 } };
            var getAddressRequest = this.NewGetAddressByTextRequest();

            Mock.Arrange(() => mockAddressTextSearch.GetFullWelshAddressByTextAsync(getAddressRequest)).Returns(Task.FromResult(dtoList)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetFullWelshAddressesByTextAsync(getAddressRequest);

            // Assert
            Mock.Assert(() => mockAddressTextSearch.GetFullWelshAddressByTextAsync(getAddressRequest), Occurs.Once());
            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dtoList, okActionResult.Value);
        }
        #endregion

        #region Uprn
        [Fact]
        public async Task GetFullAddressByUprnAsyncReturnsOkObjectResult()
        {
            // Arrange
            long uprn = 100071010823;
            var expected = new FullAddressDto
            {
                Uprn = uprn,
                AddressLine1 = "address line 1",
                AddressLine2 = "address line 2",
                AddressLine3 = "address line 3",
                BuildingName = "building name",
                BuildingNumber = 200,
                DepartmentName = "department name",
                DependentLocality = "dependent locality",
                DependentThoroughfare = "dependent thoroughfare",
                DoubleDependentLocality = "double dependent locality",
                Latitude = 100,
                Longitude = 200,
                OrganisationName = "organisation name",
                PoBoxNumber = "po box number",
                PostTown = "post town",
                Postcode = "post code",
                SingleLineAddress = "single line address",
                SubBuildingName = "sub building name",
                Thoroughfare = "thoroughFare",
                XCoordinateEasting = 300,
                YCoordinateNorthing = 500
            };

            Mock.Arrange(() => mockAddressUrpnSearch.GetFullAddressByUprnAsync(uprn)).Returns(Task.FromResult(expected)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetFullAddressByUprnAsync(uprn);

            // Assert
            Mock.Assert(() => mockAddressUrpnSearch.GetFullAddressByUprnAsync(uprn), Occurs.Once());
            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expected, okActionResult.Value);
        }

        [Fact]
        public async Task GetSimpleAddressByUprnAsyncReturnsOkObjectResult()
        {
            // Arrange
            var uprn = 100071010823;
            var expected = new SimpleAddressDto
            {
                Uprn = uprn,
                AddressLine1 = "address line 1",
                AddressLine2 = "address line 2",
                AddressLine3 = "address line 3",
                Latitude = 100,
                Longitude = 200,
                Postcode = "post code",
                SingleLineAddress = "single line address",
                XCoordinateEasting = 300,
                YCoordinateNorthing = 500
            };

            Mock.Arrange(() => mockAddressUrpnSearch.GetSimpleAddressByUprnAsync(uprn)).Returns(Task.FromResult(expected)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetSimpleAddressByUprnAsync(uprn);

            // Assert
            Mock.Assert(() => mockAddressUrpnSearch.GetSimpleAddressByUprnAsync(uprn), Occurs.Once());
            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expected, okActionResult.Value);
        }

        [Fact]
        public async Task GetFullWelshAddressByUprnAsyncReturnsOkObjectResult()
        {
            // Arrange
            var uprn = 100071010823;
            var expected = new FullWelshAddressDto
            {
                Uprn = uprn,
                AddressLine1 = "address line 1",
                AddressLine2 = "address line 2",
                AddressLine3 = "address line 3",
                BuildingName = "building name",
                BuildingNumber = 200,
                DepartmentName = "department name",
                DependentLocality = "dependent locality",
                DependentThoroughfare = "dependent thoroughfare",
                DoubleDependentLocality = "double dependent locality",
                Latitude = 100,
                Longitude = 200,
                OrganisationName = "organisation name",
                PoBoxNumber = "po box number",
                PostTown = "post town",
                Postcode = "post code",
                SingleLineAddress = "single line address",
                SubBuildingName = "sub building name",
                Thoroughfare = "thoroughFare",
                XCoordinateEasting = 300,
                YCoordinateNorthing = 500
            };

            Mock.Arrange(() => mockAddressUrpnSearch.GetFullWelshAddressByUprnAsync(uprn)).Returns(Task.FromResult(expected)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetFullWelshAddressByUprnAsync(uprn);

            // Assert
            Mock.Assert(() => mockAddressUrpnSearch.GetFullWelshAddressByUprnAsync(uprn), Occurs.Once());
            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expected, okActionResult.Value);
        }

        [Fact]
        public async Task GetSimpleWelshAddressByUprnAsyncReturnsOkObjectResult()
        {
            // Arrange
            var uprn = 100071010823;
            var expected = new SimpleWelshAddressDto
            {
                Uprn = uprn,
                AddressLine1 = "address line 1",
                AddressLine2 = "address line 2",
                AddressLine3 = "address line 3",
                Latitude = 100,
                Longitude = 200,
                Postcode = "post code",
                SingleLineAddress = "single line address",
                XCoordinateEasting = 300,
                YCoordinateNorthing = 500
            };

            Mock.Arrange(() => mockAddressUrpnSearch.GetSimpleWelshAddressByUprnAsync(uprn)).Returns(Task.FromResult(expected)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetSimpleWelshAddressByUprnAsync(uprn);

            // Assert
            Mock.Assert(() => mockAddressUrpnSearch.GetSimpleWelshAddressByUprnAsync(uprn), Occurs.Once());
            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expected, okActionResult.Value);
        }
        #endregion

        #region Coordinates - Easting \ Northing
        [Fact]
        public async Task GetAddressesByEastingNorthingAsyncReturnsOkObjectResult()
        {
            // Arrange
            IEnumerable<SimpleAddressDto> dtoList = new List<SimpleAddressDto> { new SimpleAddressDto { } };

            Mock.Arrange(() => mockAddressEastingNorthingSearch.GetAddressesByEastingNorthingAsync(this.getAddressByEastingNorthingRequest)).Returns(Task.FromResult(dtoList)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetAddressesByEastingNorthing(this.getAddressByEastingNorthingRequest);

            // Assert
            Mock.Assert(() => mockAddressEastingNorthingSearch.GetAddressesByEastingNorthingAsync(this.getAddressByEastingNorthingRequest), Occurs.Once());
            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dtoList, okActionResult.Value);
        }

        [Fact]
        public async Task GetWelshAddressesByEastingNorthingAsyncReturnsOkObjectResult()
        {
            // Arrange
            IEnumerable<SimpleWelshAddressDto> dtoList = new List<SimpleWelshAddressDto> { new SimpleWelshAddressDto { } };

            Mock.Arrange(() => mockAddressEastingNorthingSearch.GetWelshAddressesByEastingNorthingAsync(this.getAddressByEastingNorthingRequest)).Returns(Task.FromResult(dtoList)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetWelshAddressesByEastingNorthing(this.getAddressByEastingNorthingRequest);

            // Assert
            Mock.Assert(() => mockAddressEastingNorthingSearch.GetWelshAddressesByEastingNorthingAsync(this.getAddressByEastingNorthingRequest), Occurs.Once());
            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dtoList, okActionResult.Value);
        }
        #endregion

        #region Coordinates - Longitude \ Latitude
        [Fact]
        public async Task GetAddressesByLongitudeLatitudeAsyncReturnsOkObjectResult()
        {
            // Arrange
            IEnumerable<SimpleAddressDto> dtoList = new List<SimpleAddressDto> { new SimpleAddressDto { } };

            Mock.Arrange(() => mockAddressLongitudeLatitudeSearch.GetAddressesByLongitudeLatitudeAsync(this.getAddressByLongitudeLatitudeRequest)).Returns(Task.FromResult(dtoList)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetAddressesByLongitudeLatitude(this.getAddressByLongitudeLatitudeRequest);

            // Assert
            Mock.Assert(() => mockAddressLongitudeLatitudeSearch.GetAddressesByLongitudeLatitudeAsync(this.getAddressByLongitudeLatitudeRequest), Occurs.Once());
            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dtoList, okActionResult.Value);
        }

        [Fact]
        public async Task GetWelshAddressesByLongitudeLatitudeAsyncReturnsOkObjectResult()
        {
            // Arrange
            IEnumerable<SimpleWelshAddressDto> dtoList = new List<SimpleWelshAddressDto> { new SimpleWelshAddressDto { } };

            Mock.Arrange(() => mockAddressLongitudeLatitudeSearch.GetWelshAddressesByLongitudeLatitudeAsync(this.getAddressByLongitudeLatitudeRequest)).Returns(Task.FromResult(dtoList)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetWelshAddressesByLongitudeLatitude(this.getAddressByLongitudeLatitudeRequest);

            // Assert
            Mock.Assert(() => mockAddressLongitudeLatitudeSearch.GetWelshAddressesByLongitudeLatitudeAsync(this.getAddressByLongitudeLatitudeRequest), Occurs.Once());
            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dtoList, okActionResult.Value);
        }

        #endregion

        #region Postcode
        [Fact]
        public async Task GetSimpleAddressByPostCodeAsyncReturnsOkObjectResult()
        {
            // Arrange
            IEnumerable<SimpleAddressDto> dtoList = new List<SimpleAddressDto> { new SimpleAddressDto { Uprn = 500 } };

            Mock.Arrange(() => mockAddressPostcodeSearch.GetSimpleAddressesByPostcodeAsync(this.postCode)).Returns(Task.FromResult(dtoList)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetSimpleAddressByPostCodeAsync(this.postCode);

            // Assert
            Mock.Assert(() => mockAddressPostcodeSearch.GetSimpleAddressesByPostcodeAsync(this.postCode), Occurs.Once());
            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dtoList, okActionResult.Value);
        }

        [Fact]
        public async Task GetSimpleWelshAddressByPostCodeAsyncReturnsOkObjectResult()
        {
            // Arrange
            IEnumerable<SimpleWelshAddressDto> dtoList = new List<SimpleWelshAddressDto> { new SimpleWelshAddressDto { Uprn = 500 } };

            Mock.Arrange(() => mockAddressPostcodeSearch.GetSimpleWelshAddressesByPostcodeAsync(this.postCode)).Returns(Task.FromResult(dtoList)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetSimpleWelshAddressByPostCodeAsync(this.postCode);

            // Assert
            Mock.Assert(() => mockAddressPostcodeSearch.GetSimpleWelshAddressesByPostcodeAsync(this.postCode), Occurs.Once());
            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dtoList, okActionResult.Value);
        }

        [Fact]
        public async Task GetFullAddressByPostCodeAsyncReturnsOkObjectResult()
        {
            // Arrange
            IEnumerable<FullAddressDto> dtoList = new List<FullAddressDto> { new FullAddressDto { Uprn = 500 } };

            Mock.Arrange(() => mockAddressPostcodeSearch.GetFullAddressesByPostcodeAsync(this.postCode)).Returns(Task.FromResult(dtoList)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetFullAddressByPostCodeAsync(this.postCode);

            // Assert
            Mock.Assert(() => mockAddressPostcodeSearch.GetFullAddressesByPostcodeAsync(this.postCode), Occurs.Once());
            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dtoList, okActionResult.Value);
        }

        [Fact]
        public async Task GetFullWelshAddressByPostCodeAsyncReturnsOkObjectResult()
        {
            // Arrange
            IEnumerable<FullWelshAddressDto> dtoList = new List<FullWelshAddressDto> { new FullWelshAddressDto { Uprn = 500 } };

            Mock.Arrange(() => mockAddressPostcodeSearch.GetFullWelshAddressesByPostcodeAsync(this.postCode)).Returns(Task.FromResult(dtoList)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetFullWelshAddressByPostCodeAsync(this.postCode);

            // Assert
            Mock.Assert(() => mockAddressPostcodeSearch.GetFullWelshAddressesByPostcodeAsync(this.postCode), Occurs.Once());
            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dtoList, okActionResult.Value);
        }
        #endregion

        #region AuditLog
        [Fact]
        public async Task GetAuditLogAsyncReturnsOkObjectResult()
        {
            // Arrange
            IEnumerable<AddressSearchAuditLogDto> dtoList = new List<AddressSearchAuditLogDto>
            {
                new AddressSearchAuditLogDto
                {
                    AddressSearch = "address search",
                    Id = Guid.NewGuid(),
                    LogDate = DateTime.Now,
                    ResultCount = 5
                }
            };

            Mock.Arrange(() => mockReadOnlyAddressAuditLogger.GetAllAsync()).Returns(Task.FromResult(dtoList)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetAuditLog();

            // Assert
            Mock.Assert(() => mockReadOnlyAddressAuditLogger.GetAllAsync(), Occurs.Once());
            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dtoList, okActionResult.Value);
        }

        [Fact]
        public async Task GetAuditLogByDatesAsyncReturnsOkObjectResult()
        {
            // Arrange
            IEnumerable<AddressSearchAuditLogDto> dtoList = new List<AddressSearchAuditLogDto>
            {
                new AddressSearchAuditLogDto
                {
                    AddressSearch = "address search",
                    Id = Guid.NewGuid(),
                    LogDate = DateTime.Now,
                    ResultCount = 5
                }
            };

            Mock.Arrange(() => mockReadOnlyAddressAuditLogger.GetAllByDateRangeAsync(Arg.IsAny<DateTime>(), Arg.IsAny<DateTime>())).Returns(Task.FromResult(dtoList)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetAuditLogByDates(DateTime.Now, DateTime.Now);

            // Assert
            Mock.Assert(() => mockReadOnlyAddressAuditLogger.GetAllByDateRangeAsync(Arg.IsAny<DateTime>(), Arg.IsAny<DateTime>()), Occurs.Once());
            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dtoList, okActionResult.Value);
        }

        [Fact]
        public async Task GetAuditLogByResultsAsyncReturnsOkObjectResult()
        {
            // Arrange
            IEnumerable<AddressSearchAuditLogDto> dtoList = new List<AddressSearchAuditLogDto>
            {
                new AddressSearchAuditLogDto
                {
                    AddressSearch = "address search",
                    Id = Guid.NewGuid(),
                    LogDate = DateTime.Now,
                    ResultCount = 5
                }
            };

            Mock.Arrange(() => mockReadOnlyAddressAuditLogger.GetAllByResultCountRangeAsync(Arg.IsAny<int>(), Arg.IsAny<int>())).Returns(Task.FromResult(dtoList)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetAuditLogByResults(1, 100);

            // Assert
            Mock.Assert(() => mockReadOnlyAddressAuditLogger.GetAllByResultCountRangeAsync(Arg.IsAny<int>(), Arg.IsAny<int>()), Occurs.Once());
            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dtoList, okActionResult.Value);
        }

        [Fact]
        public async Task GetAuditLogBySearchAsyncReturnsOkObjectResult()
        {
            // Arrange
            IEnumerable<AddressSearchAuditLogDto> dtoList = new List<AddressSearchAuditLogDto>
            {
                new AddressSearchAuditLogDto
                {
                    AddressSearch = "address search",
                    Id = Guid.NewGuid(),
                    LogDate = DateTime.Now,
                    ResultCount = 5
                }
            };

            Mock.Arrange(() => mockReadOnlyAddressAuditLogger.GetAllBySearchTextAsync(Arg.IsAny<string>())).Returns(Task.FromResult(dtoList)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetAuditLogBySearch("search text");

            // Assert
            Mock.Assert(() => mockReadOnlyAddressAuditLogger.GetAllBySearchTextAsync(Arg.IsAny<string>()), Occurs.Once());
            var okActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dtoList, okActionResult.Value);
        }

        #endregion

        private AddressController CreateSut()
        {
            return new AddressController(this.addressControllerLogger,
                                        mockAddressUrpnSearch,
                                        mockAddressEastingNorthingSearch,
                                        mockAddressLongitudeLatitudeSearch,
                                        mockAddressPostcodeSearch,
                                        mockAddressTextSearch,
                                        mockReadOnlyAddressAuditLogger);
        }

        private GetAddressByTextRequest NewGetAddressByTextRequest()
        {
            return new GetAddressByTextRequest
            {
                SearchString = "ANNEXE",
                PageSize = 10,
                SkipPages = 1
            };
        }
    }
}