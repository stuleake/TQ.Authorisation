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
    public class SearchableAddressConverterTests
    {
        [Fact]
        public void ToFullAddressDtoReturnsNullWhenEntityIsNull()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act
            var result = sut.ToFullAddressDto(null);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ToFullAddressDtoSuccess()
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
                WelshAddressLine1 = "welsh address line 1",
                WelshAddressLine2 = "welsh address line 2",
                WelshAddressLine3 = "welsh address line 3",
                WelshDependentThoroughfare = "welsh dependent thoroughfare",
                WelshThoroughfare = "welsh thoroughfare",
                WelshDoubleDependentLocality = "welsh double dependent locality",
                WelshDependentLocality = "welsh dependent locality",
                WelshPostTown = "welsh post town",
                WelshSingleLineAddress = "welsh single line address",
                Country = "E"
            };

            // Act
            var result = CreateSut().ToFullAddressDto(entity);

            // Assert
            this.AssertFullAddress(entity, result);
        }

        [Fact]
        public void ToSimpleAddressDtoReturnsNullWhenEntityIsNull()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act
            var result = sut.ToSimpleAddressDto(null);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ToSimpleAddressDtoSuccess()
        {
            // Arrange
            var entity = new SearchableAddress
            {
                Uprn = 100,
                SingleLineAddress = "single line address",
                AddressLine1 = "address line 1",
                AddressLine2 = "address line 2",
                AddressLine3 = "address line 3",
                Postcode = "post code",
                Latitude = 100,
                Longitude = 200,
                PostTown = "post town",
                YCoordinateNorthing = 500,
                XCoordinateEasting = 600,
                Country = "E"
            };

            var sut = this.CreateSut();

            // Act
            var result = sut.ToSimpleAddressDto(entity);

            // Assert
            this.AssertSimpleAddress(entity, result);
        }

        [Fact]
        public void ToSimpleAddressDtoListReturnsNullWhenInputIsNull()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act
            var result = sut.ToSimpleAddressDtoList(null);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ToSimpleAddressDtoListReturnsEmptyWhenInputIsEmpty()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act
            var result = sut.ToSimpleAddressDtoList(new List<SearchableAddress>());

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void ToFullAddressDtoListSuccess()
        {
            // Arrange
            var helper = new SearchableAddressHelper();
            var searchableAddresses = new List<SearchableAddress>
            {
                (SearchableAddress)helper.NewFullAddress<SearchableAddress>(1),
                (SearchableAddress)helper.NewFullAddress<SearchableAddress>(2),
                (SearchableAddress)helper.NewFullAddress<SearchableAddress>(3),
                (SearchableAddress)helper.NewFullAddress<SearchableAddress>(4),
                (SearchableAddress)helper.NewFullAddress<SearchableAddress>(5),
            };

            var sut = this.CreateSut();

            // Act
            var dtos = sut.ToFullAddressDtoList(searchableAddresses).ToList();

            // Assert
            dtos.Count().Should().Be(searchableAddresses.Count());
            for (var index = 0; index < dtos.Count(); index++)
            {
                this.AssertFullAddress(searchableAddresses[index], dtos[index]);
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

        private void AssertFullAddress(SearchableAddress entity, FullAddressDto dto)
        {
            dto.Should().BeEquivalentTo(new FullAddressDto
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
                WelshAddressLine1 = entity.WelshAddressLine1,
                WelshAddressLine2 = entity.WelshAddressLine2,
                WelshAddressLine3 = entity.WelshAddressLine3,
                WelshDependentThoroughfare = entity.WelshDependentThoroughfare,
                WelshThoroughfare = entity.WelshThoroughfare,
                WelshDoubleDependentLocality = entity.WelshDoubleDependentLocality,
                WelshDependentLocality = entity.WelshDependentLocality,
                WelshPostTown = entity.WelshPostTown,
                WelshSingleLineAddress = entity.WelshSingleLineAddress,
                Country = entity.Country
            });
        }

        private void AssertSimpleAddress(SearchableAddress entity, SimpleAddressDto result)
        {
            result.Should().BeEquivalentTo(new SimpleAddressDto
            {
                Uprn = entity.Uprn,
                AddressLine1 = entity.AddressLine1,
                AddressLine2 = entity.AddressLine2,
                AddressLine3 = entity.AddressLine3,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                Postcode = entity.Postcode,
                Town = entity.PostTown,
                SingleLineAddress = entity.SingleLineAddress,
                XCoordinateEasting = entity.XCoordinateEasting,
                YCoordinateNorthing = entity.YCoordinateNorthing,
                Country = entity.Country
            });
        }

        public ISearchableAddressConverter CreateSut()
        {
            return new SearchableAddressConverter();
        }
    }
}