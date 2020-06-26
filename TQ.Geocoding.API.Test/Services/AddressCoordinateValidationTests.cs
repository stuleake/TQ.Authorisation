using System;
using TQ.Geocoding.Service.Interface.Search;
using TQ.Geocoding.Service.Search;
using Xunit;

namespace TQ.Geocoding.API.Test.Services
{
    public class AddressCoordinateValidationTests
    {
        [Fact]
        public void ValidateCoordinatesThrowsArgumentExceptionWhenXCoordinateIsDefault()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            Exception ex = Assert.Throws<ArgumentException>(() =>  sut.ValidateCoordinates(0,1));
        }

        [Fact]
        public void ValidateCoordinatesThrowsArgumentExceptionWhenYCoordinateIsDefault()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            Exception ex = Assert.Throws<ArgumentException>(() => sut.ValidateCoordinates(1, 0));
        }

        [Fact]
        public void ValidateCoordinatesThrowsArgumentExceptionWhenRadiusIsDefault()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            Exception ex = Assert.Throws<ArgumentException>(() => sut.ValidateCoordinates(1, 1, 0));
        }

        [Fact]
        public void ValidateRequestThrowsArgumentNullExceptionWhenRequestIsNull()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            Exception ex = Assert.Throws<ArgumentNullException>(() => sut.ValidateRequest(null));
        }

        private IAddressCoordinateValidation CreateSut()
        {
            return new AddressCoordinateValidation();
        }       
    }
}