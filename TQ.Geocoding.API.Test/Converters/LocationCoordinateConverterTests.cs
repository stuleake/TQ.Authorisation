using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using TQ.Geocoding.Dto.Location;
using TQ.Geocoding.Models;
using TQ.Geocoding.Service.Converters;
using TQ.Geocoding.Service.Interface.Converters;
using Xunit;

namespace TQ.Geocoding.API.Test.Converters
{
    public class LocationCoordinateConverterTests
    {
        [Fact]
        public void ToLocationCoordinateDtoReturnsNullWhenEntityIsNull()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act
            var dto = sut.ToLocationCoordinateDto(null);

            // Assert
            dto.Should().BeNull();
        }

        [Fact]
        public void ToLocationCoordinateDtoReturnsSuccess()
        {
            // Arrange
            var searchableLocationDetails = NewSearchableLocationDetails(1);

            var sut = this.CreateSut();

            // Act
            var dto = sut.ToLocationCoordinateDto(searchableLocationDetails);

            // Assert
            this.AssertSearchableLocationDetails(searchableLocationDetails, dto);
        }

        [Fact]
        public void ToLocationCoordinateDtoListReturnsNullWhenInputIsNull()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act
            var result = sut.ToLocationCoordinateDtoList(null);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ToLocationCoordinateDtoListReturnsEmptyWhenInputIsEmpty()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act
            var result = sut.ToLocationCoordinateDtoList(new List<vwSearchableLocationDetails>());

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void ToLocationCoordinateDtoListReturnsSuccess()
        {
            // Arrange
            var searchableLocationDetails = new List<vwSearchableLocationDetails>
            {
                NewSearchableLocationDetails(1),
                NewSearchableLocationDetails(2),
                NewSearchableLocationDetails(3),
                NewSearchableLocationDetails(4),
                NewSearchableLocationDetails(5),
            };

            var sut = this.CreateSut();

            // Act
            var dtos = sut.ToLocationCoordinateDtoList(searchableLocationDetails).ToList();

            // Assert
            dtos.Should().HaveCount(searchableLocationDetails.Count);
            for (var index = 0; index < dtos.Count; index++)
            {
                this.AssertSearchableLocationDetails(searchableLocationDetails[index], dtos[index]);
            }
        }

        private void AssertSearchableLocationDetails(vwSearchableLocationDetails entity, LocationCoordinateDto dto)
        {
            dto.Should().BeEquivalentTo(new LocationCoordinateDto
            {
                LocationId = entity.LocationId,
                GeometryX = entity.GeometryX,
                GeometryY = entity.GeometryY,
                SearchLocation = entity.SearchLocation,
                SearchLanguage = entity.SearchLanguage,
                Place = entity.Place,
                PlaceType = entity.PlaceType,
                PlaceSubType = entity.PlaceSubType,
                PostcodeDistrict = entity.PostcodeDistrict,
                PopulatedPlace = entity.PopulatedPlace,
                DistrictBorough = entity.DistrictBorough,
                CountyUnitary = entity.CountyUnitary,
                Region = entity.Region,
                Country = entity.Country
            });
        }

        private static vwSearchableLocationDetails NewSearchableLocationDetails(int index)
        {
            return new vwSearchableLocationDetails
            {
                Id = index,
                LocationId = $"{index.ToString().PadLeft(8, '0')}-caf3-441e-9ce7-a383b35b509b",
                GeometryX = 500000 + index,
                GeometryY = 200000 + index,
                SearchLocation = $"randomroadandtown {index}",
                SearchLanguage = $"eng {index}",
                Place = $"randomplace {index}",
                PlaceType = $"randomplacetype {index}",
                PlaceSubType = $"randomplacesubtype {index}",
                PostcodeDistrict = $"randompostcodedistrict {index}",
                PopulatedPlace = $"randompopulatedplace {index}",
                DistrictBorough = $"randomdistrictborough {index}",
                CountyUnitary = $"randomcountyunitary {index}",
                Region = $"randomregion {index}",
                Country = $"randomcountry {index}"
            };
        }

        private ILocationCoordinateConverter CreateSut()
        {
            return new LocationCoordinateConverter();
        }
    }
}