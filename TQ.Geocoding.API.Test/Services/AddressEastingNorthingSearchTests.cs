using FluentAssertions;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using Telerik.JustMock;
using TQ.Geocoding.Api.Test;
using TQ.Geocoding.API.Test.Context;
using TQ.Geocoding.Dto.Address;
using TQ.Geocoding.Models;
using TQ.Geocoding.Repository;
using TQ.Geocoding.Repository.Address;
using TQ.Geocoding.Requests;
using TQ.Geocoding.Service;
using TQ.Geocoding.Service.Config;
using TQ.Geocoding.Service.Interface.Converters;
using TQ.Geocoding.Service.Interface.Helpers;
using TQ.Geocoding.Service.Interface.Search;
using TQ.Geocoding.Service.Search;
using Xunit;

namespace TQ.Geocoding.API.Test.Services
{
    public class AddressEastingNorthingSearchTests
    {
        private readonly ISearchableAddressConverter mockAddressConverter = Mock.Create<ISearchableAddressConverter>();
        private readonly ISearchableWelshAddressConverter mockWelshAddressConverter = Mock.Create<ISearchableWelshAddressConverter>();
        private readonly ICoordinateConverter mockCoordinateConverter = Mock.Create<ICoordinateConverter>();
        private readonly ICoordinateHelper mockCoordinateHelper = Mock.Create<ICoordinateHelper>();
        private readonly IAddressAuditLoggerHelper mockAuditLoggerHelper = Mock.Create<IAddressAuditLoggerHelper>();
        private readonly IOptions<ConfigurationSettings> mockConfigurationSettings = Options.Create(TestHelper.GetConfigurationSettings());

        [Fact]
        public async void GetAddressesByEastingNorthingAsyncReturnsNoData()
        {
            // Arrange
            var isLongitudeLatitude = true;
            var request = new GetAddressByEastingNorthingRequest { XCoordinateEasting = 100, YCoordinateNorthing = 200 };
            var expectedCoordinates = new Coordinates { IsLongitudeLatitude = isLongitudeLatitude };

            Mock.Arrange(() => mockCoordinateConverter.ApplyRadiusToCoordinates(request.XCoordinateEasting, request.YCoordinateNorthing, request.Radius)).Returns(expectedCoordinates).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetAddressesByEastingNorthingAsync(request);

            // Assert
            Mock.Assert(() => mockCoordinateConverter.ApplyRadiusToCoordinates(request.XCoordinateEasting, request.YCoordinateNorthing, request.Radius), Occurs.Once());
            result.Count().Should().Be(0);
        }

        [Fact]
        public async void GetWelshAddressesByEastingNorthingAsyncReturnsNoData()
        {
            // Arrange
            var isLongitudeLatitude = true;
            var request = new GetAddressByEastingNorthingRequest { XCoordinateEasting = 100, YCoordinateNorthing = 200 };
            var expectedCoordinates = new Coordinates { IsLongitudeLatitude = isLongitudeLatitude };

            Mock.Arrange(() => mockCoordinateConverter.ApplyRadiusToCoordinates(request.XCoordinateEasting, request.YCoordinateNorthing, request.Radius)).Returns(expectedCoordinates).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetWelshAddressesByEastingNorthingAsync(request);

            // Assert
            Mock.Assert(() => mockCoordinateConverter.ApplyRadiusToCoordinates(request.XCoordinateEasting, request.YCoordinateNorthing, request.Radius), Occurs.Once());
            result.Count().Should().Be(0);
        }

        [Fact]
        public async void GetAddressesByEastingNorthingAsyncReturnsData()
        {
            // Arrange
            var isLongitudeLatitude = false;
            var request = new GetAddressByEastingNorthingRequest { XCoordinateEasting = 100, YCoordinateNorthing = 200 };
            var expectedCoordinates = new Coordinates { IsLongitudeLatitude = isLongitudeLatitude, MinX = 700, MaxY = 800 };
            var expectedDtos = new List<SimpleAddressDto>
            {
                new SimpleAddressDto
                {
                    AddressLine1 = "address line1",
                    AddressLine2 = "address line2",
                    AddressLine3 = "address line3"
                }
            };

            Mock.Arrange(() => mockCoordinateConverter.ApplyRadiusToCoordinates(request.XCoordinateEasting, request.YCoordinateNorthing, request.Radius)).Returns(expectedCoordinates).MustBeCalled();
            Mock.Arrange(() => mockAddressConverter.ToSimpleAddressDtoList(Arg.IsAny<IEnumerable<SearchableAddress>>())).Returns(expectedDtos).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetAddressesByEastingNorthingAsync(request);

            // Assert
            Mock.Assert(() => mockCoordinateConverter.ApplyRadiusToCoordinates(request.XCoordinateEasting, request.YCoordinateNorthing, request.Radius), Occurs.Once());
            Mock.Assert(() => mockAddressConverter.ToSimpleAddressDtoList(Arg.IsAny<IEnumerable<SearchableAddress>>()), Occurs.Once());
            result.Count().Should().Be(1);
        }

        [Fact]
        public async void GetWelshAddressesByEastingNorthingAsyncReturnsData()
        {
            // Arrange
            var isLongitudeLatitude = false;
            var request = new GetAddressByEastingNorthingRequest { XCoordinateEasting = 100, YCoordinateNorthing = 200 };
            var expectedCoordinates = new Coordinates { IsLongitudeLatitude = isLongitudeLatitude, MinX = 700, MaxY = 800 };
            var expectedDtos = new List<SimpleWelshAddressDto>
            {
                new SimpleWelshAddressDto
                {
                    AddressLine1 = "welsh address line1",
                    AddressLine2 = "welsh address line2",
                    AddressLine3 = "welsh address line3"
                }
            };

            Mock.Arrange(() => mockCoordinateConverter.ApplyRadiusToCoordinates(request.XCoordinateEasting, request.YCoordinateNorthing, request.Radius)).Returns(expectedCoordinates).MustBeCalled();
            Mock.Arrange(() => mockWelshAddressConverter.ToSimpleAddressDtoList(Arg.IsAny<IEnumerable<SearchableAddress>>())).Returns(expectedDtos).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetWelshAddressesByEastingNorthingAsync(request);

            // Assert
            Mock.Assert(() => mockCoordinateConverter.ApplyRadiusToCoordinates(request.XCoordinateEasting, request.YCoordinateNorthing, request.Radius), Occurs.Once());
            Mock.Assert(() => mockWelshAddressConverter.ToSimpleAddressDtoList(Arg.IsAny<IEnumerable<SearchableAddress>>()), Occurs.Once());
            result.Count().Should().Be(1);
        }

        [Fact]
        public async void GetAddressesByEastingNorthingAsyncReturnsNoDataWhenByClosestCoordinatesIsTrue()
        {
            // Arrange
            var isLongitudeLatitude = true;
            var request = new GetAddressByEastingNorthingRequest
            {
                XCoordinateEasting = 100,
                YCoordinateNorthing = 200,
                ByClosestCoordinates = true
            };
            var expectedCoordinates = new Coordinates
            {
                IsLongitudeLatitude = isLongitudeLatitude,
            };

            var closestCoordinates = new ClosestCoordinates
            {
                ClosestEasting = 0,
                ClosestNorthing = 0,
            };

            Mock.Arrange(() => mockCoordinateConverter.ApplyRadiusToCoordinates(request.XCoordinateEasting, request.YCoordinateNorthing, request.Radius))
                .Returns(expectedCoordinates)
                .MustBeCalled();
            Mock.Arrange(() => mockCoordinateHelper.GetClosestCoordinates(Arg.IsAny<IEnumerable<SearchableAddress>>(), Arg.IsAny<decimal>(), Arg.IsAny<decimal>(), Arg.IsAny<bool>()))
                .Returns(closestCoordinates)
                .MustBeCalled();

            var sut = this.CreateSut();

            // Act
            var result = await sut.GetAddressesByEastingNorthingAsync(request);

            // Assert
            Mock.Assert(() => mockCoordinateConverter.ApplyRadiusToCoordinates(request.XCoordinateEasting, request.YCoordinateNorthing, request.Radius), Occurs.Once());
            Mock.Assert(() => mockCoordinateHelper.GetClosestCoordinates(Arg.IsAny<IEnumerable<SearchableAddress>>(), Arg.IsAny<decimal>(), Arg.IsAny<decimal>(), Arg.IsAny<bool>()), Occurs.Once());
            result.Count().Should().Be(0);
        }

        [Fact]
        public async void GetAddressesByEastingNorthingAsyncReturnsDataWhenByClosestCoordinatesIsTrue()
        {
            // Arrange
            var isLongitudeLatitude = true;
            var request = new GetAddressByEastingNorthingRequest
            {
                XCoordinateEasting = 100,
                YCoordinateNorthing = 200,
                ByClosestCoordinates = true
            };
            var expectedCoordinates = new Coordinates
            {
                IsLongitudeLatitude = isLongitudeLatitude,
            };

            var closestCoordinates = new ClosestCoordinates
            {
                ClosestEasting = 100,
                ClosestNorthing = 200,
            };

            var expectedDtos = new List<SimpleAddressDto>
            {
                new SimpleAddressDto
                {
                    AddressLine1 = "address line1",
                    AddressLine2 = "address line2",
                    AddressLine3 = "address line3"
                }
            };

            Mock.Arrange(() => mockCoordinateConverter.ApplyRadiusToCoordinates(request.XCoordinateEasting, request.YCoordinateNorthing, request.Radius))
                .Returns(expectedCoordinates)
                .MustBeCalled();
            Mock.Arrange(() => mockCoordinateHelper.GetClosestCoordinates(Arg.IsAny<IEnumerable<SearchableAddress>>(), Arg.IsAny<decimal>(), Arg.IsAny<decimal>(), Arg.IsAny<bool>()))
                .Returns(closestCoordinates)
                .MustBeCalled();
            Mock.Arrange(() => mockAddressConverter.ToSimpleAddressDtoList(Arg.IsAny<IEnumerable<SearchableAddress>>()))
                .Returns(expectedDtos)
                .MustBeCalled();

            var sut = this.CreateSut();

            // Act
            var result = await sut.GetAddressesByEastingNorthingAsync(request);

            // Assert
            Mock.Assert(() => mockCoordinateConverter.ApplyRadiusToCoordinates(request.XCoordinateEasting, request.YCoordinateNorthing, request.Radius), Occurs.Once());
            Mock.Assert(() => mockCoordinateHelper.GetClosestCoordinates(Arg.IsAny<IEnumerable<SearchableAddress>>(), Arg.IsAny<decimal>(), Arg.IsAny<decimal>(), Arg.IsAny<bool>()), Occurs.Once());
            Mock.Assert(() => mockAddressConverter.ToSimpleAddressDtoList(Arg.IsAny<IEnumerable<SearchableAddress>>()), Occurs.Once());
            result.Count().Should().Be(1);
        }

        private IReadOnlyRepository<SearchableAddress> CreateRepository()
        {
            var dbContext = new AddressTestInMemoryDbContextFactory().GetGeocodingDbContext();
            return new AddressReadOnlyRepository<SearchableAddress>(dbContext);
        }

        private IAddressEastingNorthingSearch CreateSut()
        {
            return new AddressEastingNorthingSearch(this.CreateRepository(),
                                                mockAddressConverter,
                                                mockCoordinateConverter,
                                                mockWelshAddressConverter,
                                                mockCoordinateHelper,
                                                mockAuditLoggerHelper,
                                                mockConfigurationSettings);
        }
    }
}