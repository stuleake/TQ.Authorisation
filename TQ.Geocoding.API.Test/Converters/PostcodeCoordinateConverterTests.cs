using FluentAssertions;
using TQ.Geocoding.Models;
using TQ.Geocoding.Service.Converters;
using TQ.Geocoding.Service.Interface.Converters;
using Xunit;

namespace TQ.Geocoding.API.Test.Converters
{
    public class PostcodeCoordinateConverterTests
    {
        [Fact]
        public void ToPostcodeCoordinateDtoReturnsNullWhenEntityIsNull()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act
            var dto = sut.ToPostcodeCoordinateDto(null);

            // Assert
            dto.Should().BeNull();
        }

        [Fact]
        public void ToPostcodeCoordinateDtoReturnsSuccess()
        {
            const string Country = "W";
            const string CountyUnitary = "County Unitary";
            const string DistrictBorough = "District Borough";
            const string PopulatedPlace = "Populated Place";
            const string PostCode = "postcode";
            const decimal GeometryX = 100;
            const decimal GeometryY = 200;

            // Arrange
            var entity = new PostcodeCoordinates
            {
                Country = Country,
                CountyUnitary = CountyUnitary,
                DistrictBorough = DistrictBorough,
                GeometryX = GeometryX,
                GeometryY = GeometryY,
                PopulatedPlace = PopulatedPlace,
                Postcode = PostCode
            };

            var sut = this.CreateSut();

            // Act
            var dto = sut.ToPostcodeCoordinateDto(entity);

            // Assert
            dto.Should().NotBeNull();
            dto.Country.Should().Be(Country);
            dto.CountyUnitary.Should().Be(CountyUnitary);
            dto.DistrictBorough.Should().Be(DistrictBorough);
            dto.GeometryX.Should().Be(GeometryX);
            dto.GeometryY.Should().Be(GeometryY);
            dto.PopulatedPlace.Should().Be(PopulatedPlace);
            dto.Postcode.Should().Be(PostCode);
        }

        private IPostcodeCoordinateConverter CreateSut()
        {
            return new PostcodeCoordinateConverter();
        }
    }
}