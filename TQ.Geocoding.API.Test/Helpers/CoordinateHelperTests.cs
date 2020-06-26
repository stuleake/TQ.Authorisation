using System.Collections.Generic;
using FluentAssertions;
using TQ.Geocoding.Models;
using TQ.Geocoding.Service;
using TQ.Geocoding.Service.Helpers;
using TQ.Geocoding.Service.Interface;
using TQ.Geocoding.Service.Interface.Helpers;
using Xunit;

namespace TQ.Geocoding.API.Test.Helpers
{
    public class CoordinateHelperTests
    {
        [Fact]
        public void GetMaxCoordinatesAllDigitsOfEastingNorthingPresentSuccess()
        {
            // Arrange
            ICoordinates coordinates = new Coordinates()
            {
                IsLongitudeLatitude = false,
                MinX = 123456.52M,
                MinY = 987654.54M
            };

            decimal ExpectedMinX = 123456.52M;
            decimal ExpectedMaxX = 123456.529M;
            decimal ExpectedMinY = 987654.54M;
            decimal ExpectedMaxY = 987654.549M;
            var IsLongitudeLatitude = false;

            var sut = CreateSut();

            // Act
            var actual = sut.GetMaxCoordinates(coordinates);

            // Assert
            this.AssertCoordinates(actual, ExpectedMinX, ExpectedMaxX, ExpectedMinY, ExpectedMaxY, IsLongitudeLatitude);
        }

        [Fact]
        public void GetMaxCoordinatesOneDigitOfEastingNorthingMissingSuccess()
        {
            // Arrange
            ICoordinates coordinates = new Coordinates()
            {
                IsLongitudeLatitude = false,
                MinX = 123456.5M,
                MinY = 987654.5M
            };

            decimal ExpectedMinX = 123456.5M;
            decimal ExpectedMaxX = 123456.59M;
            decimal ExpectedMinY = 987654.5M;
            decimal ExpectedMaxY = 987654.59M;
            var IsLongitudeLatitude = false;

            var sut = CreateSut();

            // Act
            var actual = sut.GetMaxCoordinates(coordinates);

            // Assert
            this.AssertCoordinates(actual, ExpectedMinX, ExpectedMaxX, ExpectedMinY, ExpectedMaxY, IsLongitudeLatitude);
        }

        [Fact]
        public void GetMaxCoordinatesAllDigitsOfEastingNorthingMissingSuccess()
        {
            // Arrange
            ICoordinates coordinates = new Coordinates()
            {
                IsLongitudeLatitude = false,
                MinX = 123456M,
                MinY = 987654M
            };

            decimal ExpectedMinX = 123456M;
            decimal ExpectedMaxX = 123456.99M;
            decimal ExpectedMinY = 987654M;
            decimal ExpectedMaxY = 987654.99M;
            var IsLongitudeLatitude = false;

            var sut = CreateSut();

            // Act
            var actual = sut.GetMaxCoordinates(coordinates);

            // Assert
            this.AssertCoordinates(actual, ExpectedMinX, ExpectedMaxX, ExpectedMinY, ExpectedMaxY, IsLongitudeLatitude);
        }

        [Fact]
        public void GetMaxCoordinatesTooManyDigitsOfEastingNorthingPresentSuccess()
        {
            // Arrange
            ICoordinates coordinates = new Coordinates()
            {
                IsLongitudeLatitude = false,
                MinX = 123456.527M,
                MinY = 987654.548M
            };

            decimal ExpectedMinX = 123456.527M;
            decimal ExpectedMaxX = 123456.529M;
            decimal ExpectedMinY = 987654.548M;
            decimal ExpectedMaxY = 987654.549M;
            var IsLongitudeLatitude = false;

            var sut = CreateSut();

            // Act
            var actual = sut.GetMaxCoordinates(coordinates);

            // Assert
            this.AssertCoordinates(actual, ExpectedMinX, ExpectedMaxX, ExpectedMinY, ExpectedMaxY, IsLongitudeLatitude);
        }

        [Fact]
        public void GetMaxCoordinatesAllDigitsOfLongitudeLatitudePresentSuccess()
        {
            // Arrange
            ICoordinates coordinates = new Coordinates()
            {
                IsLongitudeLatitude = true,
                MinX = -3.6893709M,
                MinY = 51.4872072M
            };

            decimal ExpectedMinX = -3.6893709M;
            decimal ExpectedMaxX = -3.68937099M;
            decimal ExpectedMinY = 51.4872072M;
            decimal ExpectedMaxY = 51.48720729M;
            var IsLongitudeLatitude = true;

            var sut = CreateSut();

            // Act
            var actual = sut.GetMaxCoordinates(coordinates);

            // Assert
            this.AssertCoordinates(actual, ExpectedMinX, ExpectedMaxX, ExpectedMinY, ExpectedMaxY, IsLongitudeLatitude);
        }

        [Fact]
        public void GetMaxCoordinatesOneDigitOfLongitudeLatitudeMissingSuccess()
        {
            // Arrange
            ICoordinates coordinates = new Coordinates()
            {
                IsLongitudeLatitude = true,
                MinX = -3.689370M,
                MinY = 51.487207M
            };

            decimal ExpectedMinX = -3.689370M;
            decimal ExpectedMaxX = -3.6893709M;
            decimal ExpectedMinY = 51.487207M;
            decimal ExpectedMaxY = 51.4872079M;
            var IsLongitudeLatitude = true;

            var sut = CreateSut();

            // Act
            var actual = sut.GetMaxCoordinates(coordinates);

            // Assert
            this.AssertCoordinates(actual, ExpectedMinX, ExpectedMaxX, ExpectedMinY, ExpectedMaxY, IsLongitudeLatitude);
        }

        [Fact]
        public void GetMaxCoordinatesTwoDigitsOfLongitudeLatitudeMissingSuccess()
        {
            // Arrange
            ICoordinates coordinates = new Coordinates()
            {
                IsLongitudeLatitude = true,
                MinX = -3.68937M,
                MinY = 51.48720M
            };

            decimal ExpectedMinX = -3.68937M;
            decimal ExpectedMaxX = -3.6893799M;
            decimal ExpectedMinY = 51.48720M;
            decimal ExpectedMaxY = 51.4872099M;
            var IsLongitudeLatitude = true;

            var sut = CreateSut();

            // Act
            var actual = sut.GetMaxCoordinates(coordinates);

            // Assert
            this.AssertCoordinates(actual, ExpectedMinX, ExpectedMaxX, ExpectedMinY, ExpectedMaxY, IsLongitudeLatitude);
        }

        [Fact]
        public void GetMaxCoordinatesThreeDigitsOfLongitudeLatitudeMissingSuccess()
        {
            // Arrange
            ICoordinates coordinates = new Coordinates()
            {
                IsLongitudeLatitude = true,
                MinX = -3.6893M,
                MinY = 51.4872M
            };

            decimal ExpectedMinX = -3.6893M;
            decimal ExpectedMaxX = -3.6893999M;
            decimal ExpectedMinY = 51.4872M;
            decimal ExpectedMaxY = 51.4872999M;
            var IsLongitudeLatitude = true;

            var sut = CreateSut();

            // Act
            var actual = sut.GetMaxCoordinates(coordinates);

            // Assert
            this.AssertCoordinates(actual, ExpectedMinX, ExpectedMaxX, ExpectedMinY, ExpectedMaxY, IsLongitudeLatitude);
        }

        [Fact]
        public void GetMaxCoordinatesFourDigitsOfLongitudeLatitudeMissingSuccess()
        {
            // Arrange
            ICoordinates coordinates = new Coordinates()
            {
                IsLongitudeLatitude = true,
                MinX = -3.689M,
                MinY = 51.487M
            };

            decimal ExpectedMinX = -3.689M;
            decimal ExpectedMaxX = -3.6899999M;
            decimal ExpectedMinY = 51.487M;
            decimal ExpectedMaxY = 51.4879999M;
            var IsLongitudeLatitude = true;

            var sut = CreateSut();

            // Act
            var actual = sut.GetMaxCoordinates(coordinates);

            // Assert
            this.AssertCoordinates(actual, ExpectedMinX, ExpectedMaxX, ExpectedMinY, ExpectedMaxY, IsLongitudeLatitude);
        }

        [Fact]
        public void GetMaxCoordinatesFiveDigitsOfLongitudeLatitudeMissingSuccess()
        {
            // Arrange
            ICoordinates coordinates = new Coordinates()
            {
                IsLongitudeLatitude = true,
                MinX = -3.68M,
                MinY = 51.48M
            };

            decimal ExpectedMinX = -3.68M;
            decimal ExpectedMaxX = -3.6899999M;
            decimal ExpectedMinY = 51.48M;
            decimal ExpectedMaxY = 51.4899999M;
            var IsLongitudeLatitude = true;

            var sut = CreateSut();

            // Act
            var actual = sut.GetMaxCoordinates(coordinates);

            // Assert
            this.AssertCoordinates(actual, ExpectedMinX, ExpectedMaxX, ExpectedMinY, ExpectedMaxY, IsLongitudeLatitude);
        }

        [Fact]
        public void GetMaxCoordinatesSixDigitsOfLongitudeLatitudeMissingSuccess()
        {
            // Arrange
            ICoordinates coordinates = new Coordinates()
            {
                IsLongitudeLatitude = true,
                MinX = -3.6M,
                MinY = 51.4M
            };

            decimal ExpectedMinX = -3.6M;
            decimal ExpectedMaxX = -3.6999999M;
            decimal ExpectedMinY = 51.4M;
            decimal ExpectedMaxY = 51.4999999M;
            var IsLongitudeLatitude = true;

            var sut = CreateSut();

            // Act
            var actual = sut.GetMaxCoordinates(coordinates);

            // Assert
            this.AssertCoordinates(actual, ExpectedMinX, ExpectedMaxX, ExpectedMinY, ExpectedMaxY, IsLongitudeLatitude);
        }

        [Fact]
        public void GetMaxCoordinatesAllDigitsOfLongitudeLatitudeMissingSuccess()
        {
            // Arrange
            ICoordinates coordinates = new Coordinates()
            {
                IsLongitudeLatitude = true,
                MinX = -3M,
                MinY = 51M
            };

            decimal ExpectedMinX = -3M;
            decimal ExpectedMaxX = -3.9999999M;
            decimal ExpectedMinY = 51M;
            decimal ExpectedMaxY = 51.9999999M;
            var IsLongitudeLatitude = true;

            var sut = CreateSut();

            // Act
            var actual = sut.GetMaxCoordinates(coordinates);

            // Assert
            this.AssertCoordinates(actual, ExpectedMinX, ExpectedMaxX, ExpectedMinY, ExpectedMaxY, IsLongitudeLatitude);
        }

        [Fact]
        public void GetMaxCoordinatesTooManyDigitsOfLongitudeLatitudePresentSuccess()
        {
            // Arrange
            ICoordinates coordinates = new Coordinates()
            {
                IsLongitudeLatitude = true,
                MinX = -3.68937093M,
                MinY = 51.48720723M
            };

            decimal ExpectedMinX = -3.68937093M;
            decimal ExpectedMaxX = -3.68937099M;
            decimal ExpectedMinY = 51.48720723M;
            decimal ExpectedMaxY = 51.48720729M;
            var IsLongitudeLatitude = true;

            var sut = CreateSut();

            // Act
            var actual = sut.GetMaxCoordinates(coordinates);

            // Assert
            this.AssertCoordinates(actual, ExpectedMinX, ExpectedMaxX, ExpectedMinY, ExpectedMaxY, IsLongitudeLatitude);
        }

        [Fact]
        public void GetClosestCoordinatesSuccessWhenSearchableAddressesIsNull()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act
            var result = sut.GetClosestCoordinates(new List<SearchableAddress>(), 0, 0, false);

            // Assert
            result.ClosestEasting.Should().Be(0);
            result.ClosestNorthing.Should().Be(0);
        }

        [Fact]
        public void GetClosestCoordinatesSuccessWhenLongitudeLatitudeIsTrue()
        {
            // Arrange
            const int ExpectedCoordinate = 100;
            var searchableAddresses = new List<SearchableAddress>
            {
                new SearchableAddress
                {
                    XCoordinateEasting = ExpectedCoordinate,
                    YCoordinateNorthing = ExpectedCoordinate
                }
            };
            var sut = this.CreateSut();

            // Act
            var result = sut.GetClosestCoordinates(searchableAddresses, 500, 500, true);

            // Assert
            result.ClosestEasting.Should().Be(ExpectedCoordinate);
            result.ClosestNorthing.Should().Be(ExpectedCoordinate);
        }

        [Fact]
        public void GetClosestCoordinatesSuccessWhenLongitudeLatitudeIsFalse()
        {
            // Arrange
            const int ExpectedCoordinate = 100;
            var searchableAddresses = new List<SearchableAddress>
            {
                new SearchableAddress
                {
                    XCoordinateEasting = ExpectedCoordinate,
                    YCoordinateNorthing = ExpectedCoordinate
                }
            };
            var sut = this.CreateSut();

            // Act
            var result = sut.GetClosestCoordinates(searchableAddresses, 500, 500, false);

            // Assert
            result.ClosestEasting.Should().Be(ExpectedCoordinate);
            result.ClosestNorthing.Should().Be(ExpectedCoordinate);
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

        private ICoordinateHelper CreateSut()
        {
            return new CoordinateHelper();
        }
    }
}