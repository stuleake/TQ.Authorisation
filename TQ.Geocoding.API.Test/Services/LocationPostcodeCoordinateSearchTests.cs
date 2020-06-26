using FluentAssertions;
using System;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Geocoding.Models;
using TQ.Geocoding.Repository.Location;
using TQ.Geocoding.Service.Interface.Converters;
using TQ.Geocoding.Service.Interface.Helpers;
using TQ.Geocoding.Service.Interface.Search;
using TQ.Geocoding.Service.Search;
using Xunit;

namespace TQ.Geocoding.API.Test.Services
{
    public class LocationPostcodeCoordinateSearchTests
    {
        private readonly ILocationReadOnlyRepository<PostcodeCoordinates> mockReadOnlyRepository = Mock.Create<ILocationReadOnlyRepository<PostcodeCoordinates>>();
        private readonly IPostcodeCoordinateConverter mockPostcodeCoordinateConverter = Mock.Create<IPostcodeCoordinateConverter>();
        private readonly ILocationAuditLoggerHelper mockAuditLoggerHelper = Mock.Create<ILocationAuditLoggerHelper>();

        [Fact]
        public async Task GetByIdAsyncThrowsArgumentNullExceptionWhenIdIsNull()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetByIdAsync(null));
        }

        [Fact]
        public async Task GetByIdAsyncThrowsArgumentNullExceptionWhenIdIsEmpty()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetByIdAsync(string.Empty));
        }

        [Fact]
        public async Task GetByIdAsyncThrowsArgumentNullExceptionWhenIdIsWhitespace()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetByIdAsync(" "));
        }

        [Fact]
        public void GetByIdAsyncSuccess()
        {
            // Arrange
            var postcodeId = "B912PJ";
            Mock.Arrange(() => mockReadOnlyRepository.FirstOrDefaultAsync(postcodeCoordinate => postcodeCoordinate.Id == postcodeId)).Returns(Task.FromResult(new PostcodeCoordinates())).MustBeCalled();
            Mock.Arrange(() => mockPostcodeCoordinateConverter.ToPostcodeCoordinateDto(Arg.IsAny<PostcodeCoordinates>())).Returns(new Dto.Dtos.PostcodeCoordinateDto()).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            var result = sut.GetByIdAsync(postcodeId);

            // Assert
            Mock.Assert(() => mockReadOnlyRepository.FirstOrDefaultAsync(postcodeCoordinate => postcodeCoordinate.Id == postcodeId), Occurs.Once());
            Mock.Assert(() => mockPostcodeCoordinateConverter.ToPostcodeCoordinateDto(Arg.IsAny<PostcodeCoordinates>()), Occurs.Once());
            result.Should().NotBeNull();
        }

        private ILocationPostcodeCoordinateSearch CreateSut()
        {
            return new LocationPostcodeCoordinateSearch(this.mockReadOnlyRepository, this.mockPostcodeCoordinateConverter, this.mockAuditLoggerHelper);
        }
    }
}