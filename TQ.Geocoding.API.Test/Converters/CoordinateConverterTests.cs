using FluentAssertions;
using TQ.Geocoding.Service.Converters;
using TQ.Geocoding.Service.Interface;
using TQ.Geocoding.Service.Interface.Converters;
using Xunit;

namespace TQ.Geocoding.API.Test.Converters
{
    public class CoordinateConverterTests
    {
        [Fact]
        public void ApplyRadiusToCoordinatesForEastingNorthingZeroRadiusSuccess()
        {
            // Arrange
            decimal ExpectedMinX = 100000M;
            decimal ExpectedMaxX = 100000M;
            decimal ExpectedMinY = 200000M;
            decimal ExpectedMaxY = 200000M;
            var IsLongitudeLatitude = false;

            var sut = CreateSut();

            // Act
            var actual = sut.ApplyRadiusToCoordinates(100000M, 200000M, 0);

            // Assert
            this.AssertCoordinates(actual, ExpectedMinX, ExpectedMaxX, ExpectedMinY, ExpectedMaxY, IsLongitudeLatitude);
        }

        [Fact]
        public void ApplyRadiusToCoordinatesForEastingNorthingWithRadiusSuccess()
        {
            // Arrange
            decimal ExpectedMinX = 99900M;
            decimal ExpectedMaxX = 100100M;
            decimal ExpectedMinY = 199900M;
            decimal ExpectedMaxY = 200100M;
            var IsLongitudeLatitude = false;

            var sut = CreateSut();

            // Act
            var actual = sut.ApplyRadiusToCoordinates(100000M, 200000M, 100);

            // Assert
            this.AssertCoordinates(actual, ExpectedMinX, ExpectedMaxX, ExpectedMinY, ExpectedMaxY, IsLongitudeLatitude);
        }

        [Fact]
        public void ApplyRadiusToCoordinatesForLongitudeLatitudeZeroRadiusSuccess()
        {
            // Arrange
            decimal ExpectedMinX = 100M;
            decimal ExpectedMaxX = 100M;
            decimal ExpectedMinY = 200M;
            decimal ExpectedMaxY = 200M;
            var IsLongitudeLatitude = true;

            var sut = CreateSut();

            // Act
            var actual = sut.ApplyRadiusToCoordinates(100M, 200M, 0);

            // Assert
            this.AssertCoordinates(actual, ExpectedMinX, ExpectedMaxX, ExpectedMinY, ExpectedMaxY, IsLongitudeLatitude);
        }

        [Fact]
        public void ApplyRadiusToCoordinatesForLongitudeLatitudeWithRadiusSuccess()
        {
            // Arrange
            decimal ExpectedMinX = 100.000955967182M;
            decimal ExpectedMaxX = 99.999044032818M;
            decimal ExpectedMinY = 199.999101578682M;
            decimal ExpectedMaxY = 200.000898421318M;

            var IsLongitudeLatitude = true;

            var sut = CreateSut();

            // Act
            var actual = sut.ApplyRadiusToCoordinates(100M, 200M, 100);

            // Assert
            this.AssertCoordinates(actual, ExpectedMinX, ExpectedMaxX, ExpectedMinY, ExpectedMaxY, IsLongitudeLatitude);
        }

        private void AssertCoordinates(ICoordinates actual,
                                        decimal minX,
                                        decimal maxX,
                                        decimal minY,
                                        decimal maxY,
                                        bool isLatitudeLongitude)
        {
            actual.Should().NotBeNull();
            actual.IsLongitudeLatitude.Should().Be(isLatitudeLongitude);
            actual.MinX.Should().Be(minX);
            actual.MaxX.Should().Be(maxX);
            actual.MinY.Should().Be(minY);
            actual.MaxY.Should().Be(maxY);
        }

        private ICoordinateConverter CreateSut()
        {
            return new CoordinateConverter();
        }
    }
}
