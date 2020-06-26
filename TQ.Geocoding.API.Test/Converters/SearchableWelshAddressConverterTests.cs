using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using TQ.Geocoding.API.Test.Helpers.Model;
using TQ.Geocoding.Dto.Address;
using TQ.Geocoding.Models;
using TQ.Geocoding.Service.Converters;
using TQ.Geocoding.Service.Interface.Converters;
using Xunit;

namespace TQ.Geocoding.API.Test.Converters
{
    public class SearchableWelshAddressConverterTests
    {
        [Fact]
        public void ToSimpleWelshAddressDtoReturnsNullWhenEntityIsNull()
        {
            // Arrange
            SearchableAddress searchableAddress = null;
            var sut = this.CreateSut();

            // Act
            var result = sut.ToSimpleAddressDto(searchableAddress);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ToSimpleWelshAddressDtoSuccess()
        {
            // Arrange
            var addressHelper = new SearchableAddressHelper();
            var entity = (SearchableAddress)addressHelper.NewFullWelshAddress<SearchableAddress>(0);

            var sut = this.CreateSut();

            // Act
            var result = sut.ToSimpleAddressDto(entity);

            // Assert
            this.AssertSimpleWelshAddress(entity, result);
        }

        [Fact]
        public void ToSimpleWelshAddressDtoSuccessWhenNoWelshAddressFound()
        {
            // Arrange
            var entity = new SearchableAddress
            {
                Uprn = 100,
                WelshSingleLineAddress = "WelshSingleLineAddress",
                WelshAddressLine1 = "WelshAddressLine1",
                WelshAddressLine2 = "WelshAddressLine2",
                WelshAddressLine3 = "WelshAddressLine3",
                Postcode = "Postcode",
                Latitude = 100,
                Longitude = 200,
                WelshPostTown = "WelshPostTown",
                YCoordinateNorthing = 500,
                XCoordinateEasting = 600,
                Country = "W"
            };

            var sut = this.CreateSut();

            // Act
            var result = sut.ToSimpleAddressDto(entity);

            // Assert
            this.AssertSimpleWelshAddress(entity, result);
        }

        [Fact]
        public void ToFullWelshAddressDtoReturnsNullWhenEntityIsNull()
        {
            // Arrange
            var sut = this.CreateSut();
            vwSearchableAddress vwSearchableAddress = null;

            // Act
            var result = sut.ToFullAddressDto(vwSearchableAddress);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ToFullWelshAddressDtoSuccess()
        {
            // Arrange
            var entity = new SearchableAddress
            {
                Uprn = 100,
                WelshAddressLine1 = "address line 1",
                WelshAddressLine2 = "address line 2",
                WelshAddressLine3 = "address line 3",
                BuildingName = "building name",
                BuildingNumber = 200,
                DepartmentName = "department name",
                WelshDependentLocality = "dependent locality",
                WelshDependentThoroughfare = "dependent thoroughfare",
                WelshDoubleDependentLocality = "double dependent locality",
                Latitude = 100,
                Longitude = 200,
                OrganisationName = "organisation name",
                PoBoxNumber = "po box number",
                WelshPostTown = "post town",
                Postcode = "post code",
                WelshSingleLineAddress = "single line address",
                SubBuildingName = "sub building name",
                WelshThoroughfare = "thoroughFare",
                XCoordinateEasting = 300,
                YCoordinateNorthing = 500,
                Country = "W"
            };

            var sut = this.CreateSut();

            // Act
            var result = sut.ToFullAddressDto(entity);

            // Assert
            this.AssertFullWelshAddress(entity, result);
        }

        [Fact]
        public void ToFullWelshAddressDtoSuccessWhenNoWelshAddressFound()
        {
            // Arrange
            var entity = new SearchableAddress
            {
                Uprn = 100,
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
                ThoroughFare = "thoroughFare",
                XCoordinateEasting = 300,
                YCoordinateNorthing = 500,
                Country = "W"
            };

            var sut = this.CreateSut();

            // Act
            var result = sut.ToFullAddressDto(entity);

            // Assert
            this.AssertFullAddress(entity, result);
        }

        [Fact]
        public void ToFullAddressDtoListSuccess()
        {
            // Arrange
            var helper = new SearchableAddressHelper();
            var entities = new List<ISearchableAddress>
            {
                helper.NewFullWelshAddress<SearchableAddress>(1),
                helper.NewFullWelshAddress<SearchableAddress>(2),
                helper.NewFullWelshAddress<SearchableAddress>(3),
                helper.NewFullWelshAddress<SearchableAddress>(4),
                helper.NewFullWelshAddress<SearchableAddress>(5),
            };

            var sut = this.CreateSut();

            // Act
            var dtos = sut.ToFullAddressDtoList(entities).ToList();

            // Assert
            dtos.Count().Should().Be(entities.Count());
            for (var index = 0; index < dtos.Count(); index++)
            {
                this.AssertFullWelshAddress((SearchableAddress)entities[index], dtos[index]);
            }
        }

        [Fact]
        public void ToFullAddressDtoListReturnsNullWhenInputIsNull()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act
            var dtos = sut.ToFullAddressDtoList(null);

            // Assert
            dtos.Should().BeNull();
        }

        [Fact]
        public void ToFullAddressDtoListReturnsEmptyWhenInputIsEmpty()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act
            var dtos = sut.ToFullAddressDtoList(new List<SearchableAddress>());

            // Assert
            dtos.Should().BeEmpty();
        }

        [Fact]
        public void ToSimpleWelshAddressDtoListSuccess()
        {
            // Arrange
            var addressHelper = new SearchableAddressHelper();
            var entities = new List<SearchableAddress>
            {
               (SearchableAddress)addressHelper.NewFullWelshAddress<SearchableAddress>(0),
               (SearchableAddress)addressHelper.NewFullWelshAddress<SearchableAddress>(1)
            };

            var sut = this.CreateSut();

            // Act
            var result = sut.ToSimpleAddressDtoList(entities);

            // Assert
            result.Count().Should().Be(entities.Count);
        }

        private void AssertFullWelshAddress(SearchableAddress entity, FullWelshAddressDto dto)
        {
            dto.Should().BeEquivalentTo(new FullWelshAddressDto
            {
                Uprn = entity.Uprn,
                AddressLine1 = entity.WelshAddressLine1,
                AddressLine2 = entity.WelshAddressLine2,
                AddressLine3 = entity.WelshAddressLine3,
                BuildingName = entity.BuildingName,
                BuildingNumber = entity.BuildingNumber,
                DepartmentName = entity.DepartmentName,
                DependentLocality = entity.WelshDependentLocality,
                DependentThoroughfare = entity.WelshDependentThoroughfare,
                DoubleDependentLocality = entity.WelshDoubleDependentLocality,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                OrganisationName = entity.OrganisationName,
                PoBoxNumber = entity.PoBoxNumber,
                PostTown = entity.WelshPostTown,
                Postcode = entity.Postcode,
                SingleLineAddress = entity.WelshSingleLineAddress,
                SubBuildingName = entity.SubBuildingName,
                Thoroughfare = entity.WelshThoroughfare,
                XCoordinateEasting = entity.XCoordinateEasting,
                YCoordinateNorthing = entity.YCoordinateNorthing,
                Country = entity.Country
            });
        }

        private void AssertSimpleWelshAddress(SearchableAddress entity, SimpleWelshAddressDto dto)
        {
            dto.Should().BeEquivalentTo(new SimpleWelshAddressDto
            {
                Uprn = entity.Uprn,
                AddressLine1 = entity.WelshAddressLine1,
                AddressLine2 = entity.WelshAddressLine2,
                AddressLine3 = entity.WelshAddressLine3,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                Postcode = entity.Postcode,
                PostTown = entity.WelshPostTown,
                SingleLineAddress = entity.WelshSingleLineAddress,
                XCoordinateEasting = entity.XCoordinateEasting,
                YCoordinateNorthing = entity.YCoordinateNorthing,
                Country = entity.Country
            });
        }

        private void AssertFullAddress(SearchableAddress entity, FullWelshAddressDto dto)
        {
            dto.Should().BeEquivalentTo(new FullWelshAddressDto
            {
                Uprn = entity.Uprn,
                AddressLine1 = entity.AddressLine1,
                AddressLine2 = entity.AddressLine2,
                AddressLine3 = entity.AddressLine3,
                BuildingName = entity.BuildingName,
                BuildingNumber = entity.BuildingNumber,
                DepartmentName = entity.DepartmentName,
                DependentLocality = entity.DependentLocality,
                DependentThoroughfare = entity.DependentThoroughfare,
                DoubleDependentLocality = entity.DoubleDependentLocality,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                OrganisationName = entity.OrganisationName,
                PoBoxNumber = entity.PoBoxNumber,
                PostTown = entity.PostTown,
                Postcode = entity.Postcode,
                SingleLineAddress = entity.SingleLineAddress,
                SubBuildingName = entity.SubBuildingName,
                Thoroughfare = entity.ThoroughFare,
                XCoordinateEasting = entity.XCoordinateEasting,
                YCoordinateNorthing = entity.YCoordinateNorthing,
                Country = entity.Country
            });
        }

        private ISearchableWelshAddressConverter CreateSut()
        {
            return new SearchableWelshAddressConverter();
        }
    }
}