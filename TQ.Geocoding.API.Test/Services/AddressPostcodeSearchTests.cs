using FluentAssertions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Geocoding.Api.Test;
using TQ.Geocoding.API.Test.Helpers.Dto;
using TQ.Geocoding.API.Test.Helpers.Model;
using TQ.Geocoding.Dto.Address;
using TQ.Geocoding.Models;
using TQ.Geocoding.Repository;
using TQ.Geocoding.Service.Config;
using TQ.Geocoding.Service.Interface.Converters;
using TQ.Geocoding.Service.Interface.Helpers;
using TQ.Geocoding.Service.Interface.Search;
using TQ.Geocoding.Service.Search;
using Xunit;

namespace TQ.Geocoding.API.Test.Services
{
    public class AddressPostcodeSearchTests
    {
        private readonly IReadOnlyRepository<SearchableAddress> mockReadOnlyRepository = Mock.Create<IReadOnlyRepository<SearchableAddress>>();
        private readonly ISearchableAddressConverter mockAddressConverter = Mock.Create<ISearchableAddressConverter>();
        private readonly ISearchableWelshAddressConverter mockWelshAddressConverter = Mock.Create<ISearchableWelshAddressConverter>();
        private readonly IAddressAuditLoggerHelper mockAuditLoggerHelper = Mock.Create<IAddressAuditLoggerHelper>();
        private readonly IOptions<ConfigurationSettings> mockConfigurationSettings = Options.Create(TestHelper.GetConfigurationSettings());

        private const string ValidPostcode = "TS11 8HR";
        private const string ValidPostcodeNoSpace = "TS118HR";
        private const int ListCount = 5;

        [Fact]
        public async Task GetSimpleAddressesByPostcodeAsyncSuccess()
        {
            // Arrange
            var maxRowCount = mockConfigurationSettings.Value.MaxRowCount;
            var expectedModels = this.GetSimpleSearchableAddressList(ListCount);
            var expectedDtos = this.GetSimpleAddressDtos(ListCount);

            Mock.Arrange(() => mockReadOnlyRepository.ListAsync(p => p.Postcode == ValidPostcode, maxRowCount)).Returns(Task.FromResult(expectedModels)).MustBeCalled();
            Mock.Arrange(() => mockAddressConverter.ToSimpleAddressDtoList(expectedModels)).Returns(expectedDtos).MustBeCalled();
            
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetSimpleAddressesByPostcodeAsync(ValidPostcode);

            // Assert
            Mock.Assert(() => mockReadOnlyRepository.ListAsync(p => p.Postcode == ValidPostcode, maxRowCount), Occurs.Once());
            Mock.Assert(() => mockAddressConverter.ToSimpleAddressDtoList(expectedModels), Occurs.Once());
            
            result.Count().Should().Be(ListCount);
            this.AssertSimpleAddressesAreEqual(expectedModels.ToList(), result.ToList());
        }

        [Fact]
        public async Task GetFullAddressesByPostcodeAsyncSuccess()
        {
            // Arrange
            var maxRowCount = mockConfigurationSettings.Value.MaxRowCount;
            var expectedModels = this.GetFullSearchableAddressList(ListCount);
            var expectedDtos = this.GetFullAddressDtos(ListCount);

            Mock.Arrange(() => mockReadOnlyRepository.ListAsync(p => p.Postcode == ValidPostcode, maxRowCount)).Returns(Task.FromResult(expectedModels)).MustBeCalled();
            Mock.Arrange(() => mockAddressConverter.ToFullAddressDtoList(expectedModels)).Returns(expectedDtos).MustBeCalled();
           
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetFullAddressesByPostcodeAsync(ValidPostcode);

            // Assert
            Mock.Assert(() => mockReadOnlyRepository.ListAsync(p => p.Postcode == ValidPostcode, maxRowCount), Occurs.Once());
            Mock.Assert(() => mockAddressConverter.ToFullAddressDtoList(expectedModels), Occurs.Once());
            
            result.Count().Should().Be(ListCount);
            this.AssertFullAddressAreEqual(expectedModels.ToList(), expectedDtos.ToList());
        }

        [Fact]
        public async Task GetSimpleWelshAddressesByPostcodeAsyncSuccess()
        {
            // Arrange
            var maxRowCount = mockConfigurationSettings.Value.MaxRowCount;
            var expectedModels = this.GetSimpleWelshSearchableAddressList(ListCount);
            var expectedDtos = this.GetSimpleWelshAddressDtos(ListCount);

            Mock.Arrange(() => mockReadOnlyRepository.ListAsync(p => p.Postcode == ValidPostcode, maxRowCount)).Returns(Task.FromResult(expectedModels)).MustBeCalled();
            Mock.Arrange(() => mockWelshAddressConverter.ToSimpleAddressDtoList(expectedModels)).Returns(expectedDtos).MustBeCalled();
            
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetSimpleWelshAddressesByPostcodeAsync(ValidPostcode);

            // Assert
            Mock.Assert(() => mockReadOnlyRepository.ListAsync(p => p.Postcode == ValidPostcode, maxRowCount), Occurs.Once());
            Mock.Assert(() => mockWelshAddressConverter.ToSimpleAddressDtoList(expectedModels), Occurs.Once());
            
            result.Count().Should().Be(ListCount);
            this.AssertSimpleWelshAddressesAreEqual(expectedModels.ToList(), result.ToList());
        }

        [Fact]
        public async Task GetFullWelshAddressesByPostcodeAsyncSuccess()
        {
            // Arrange
            var maxRowCount = mockConfigurationSettings.Value.MaxRowCount;
            var expectedModels = this.GetFullWelshSearchableAddressList(ListCount);
            var expectedDtos = this.GetFullWelshAddressDtos(ListCount);

            Mock.Arrange(() => mockReadOnlyRepository.ListAsync(p => p.Postcode == ValidPostcode, maxRowCount)).Returns(Task.FromResult(expectedModels)).MustBeCalled();
            Mock.Arrange(() => mockWelshAddressConverter.ToFullAddressDtoList(expectedModels)).Returns(expectedDtos).MustBeCalled();
            
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetFullWelshAddressesByPostcodeAsync(ValidPostcode);

            // Assert
            Mock.Assert(() => mockReadOnlyRepository.ListAsync(p => p.Postcode == ValidPostcode, maxRowCount), Occurs.Once());
            Mock.Assert(() => mockWelshAddressConverter.ToFullAddressDtoList(expectedModels), Occurs.Once());
            
            result.Count().Should().Be(ListCount);
            this.AssertFullWelshAddressAreEqual(expectedModels.ToList(), expectedDtos.ToList());
        }

        [Fact]
        public async Task GetSimpleAddressesByPostcodeNoSpaceAsyncSuccess()
        {
            // Arrange
            var maxRowCount = mockConfigurationSettings.Value.MaxRowCount;
            var expectedModels = this.GetSimpleSearchableAddressList(ListCount);
            var expectedDtos = this.GetSimpleAddressDtos(ListCount);

            Mock.Arrange(() => mockReadOnlyRepository.ListAsync(p => p.Postcode == ValidPostcode, maxRowCount)).Returns(Task.FromResult(expectedModels)).MustBeCalled();
            Mock.Arrange(() => mockAddressConverter.ToSimpleAddressDtoList(expectedModels)).Returns(expectedDtos).MustBeCalled();
            
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetSimpleAddressesByPostcodeAsync(ValidPostcodeNoSpace);

            // Assert
            Mock.Assert(() => mockReadOnlyRepository.ListAsync(p => p.Postcode == ValidPostcode, maxRowCount), Occurs.Once());
            Mock.Assert(() => mockAddressConverter.ToSimpleAddressDtoList(expectedModels), Occurs.Once());
            
            result.Count().Should().Be(ListCount);
            this.AssertSimpleAddressesAreEqual(expectedModels.ToList(), result.ToList());
        }

        [Fact]
        public async Task GetSimpleAddressesByPostcodeAsyncThrowsArgumentNullException()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.GetSimpleAddressesByPostcodeAsync(string.Empty));
        }

        [Fact]
        public async Task GetSimpleAddressesByPostcodeAsyncThrowsArgumentException()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.GetSimpleAddressesByPostcodeAsync("this is not a valid postcode"));
        }

        [Fact]
        public async Task GetFullAddressesByPostcodeAsyncThrowsArgumentNullException()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.GetFullAddressesByPostcodeAsync(string.Empty));
        }

        [Fact]
        public async Task GetFullAddressesByPostcodeAsyncThrowsArgumentException()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.GetFullAddressesByPostcodeAsync("this is not a valid postcode"));
        }

        [Fact]
        public async Task GetSimpleWelshAddressesByPostcodeAsyncThrowsArgumentNullException()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.GetSimpleWelshAddressesByPostcodeAsync(string.Empty));
        }

        [Fact]
        public async Task GetSimpleWelshAddressesByPostcodeAsyncThrowsArgumentException()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.GetSimpleWelshAddressesByPostcodeAsync("this is not a valid postcode"));
        }

        [Fact]
        public async Task GetFullWelshAddressesByPostcodeAsyncThrowsArgumentNullException()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.GetFullWelshAddressesByPostcodeAsync(string.Empty));
        }

        [Fact]
        public async Task GetFullWelshAddressesByPostcodeAsyncThrowsArgumentException()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.GetFullWelshAddressesByPostcodeAsync("this is not a valid postcode"));
        }

        private IEnumerable<SearchableAddress> GetSimpleSearchableAddressList(int count)
        {
            var searchableAddresses = new List<SearchableAddress>();
            var addresshelper = new SearchableAddressHelper();
            for (var index = 0; index < count; index++)
            {
                searchableAddresses.Add((SearchableAddress)addresshelper.NewSimpleAddress<SearchableAddress>(index));
            }

            return searchableAddresses;
        }

        private IEnumerable<SearchableAddress> GetSimpleWelshSearchableAddressList(int count)
        {
            var searchableAddresses = new List<SearchableAddress>();
            var addressHelper = new SearchableAddressHelper();
            for (var index = 0; index < count; index++)
            {
                searchableAddresses.Add((SearchableAddress)addressHelper.NewSimpleWelshAddress<SearchableAddress>(index));
            }

            return searchableAddresses;
        }

        private IEnumerable<SimpleAddressDto> GetSimpleAddressDtos(int count)
        {
            var dtos = new List<SimpleAddressDto>();
            for (var index = 0; index < count; index++)
            {
                dtos.Add(DtoHelper.GetSimpleAddressDto(index));
            }

            return dtos;
        }

        private IEnumerable<SimpleWelshAddressDto> GetSimpleWelshAddressDtos(int count)
        {
            var dtos = new List<SimpleWelshAddressDto>();
            for (var index = 0; index < count; index++)
            {
                dtos.Add(DtoHelper.GetSimpleWelshAddressDto(index));
            }

            return dtos;
        }

        private IEnumerable<SearchableAddress> GetFullSearchableAddressList(int count)
        {
            var searchableAddresses = new List<SearchableAddress>();
            var addressHelper = new SearchableAddressHelper();
            for (var index = 0; index < count; index++)
            {
                searchableAddresses.Add((SearchableAddress)addressHelper.NewFullAddress<SearchableAddress>(index));
            }

            return searchableAddresses;
        }

        private IEnumerable<SearchableAddress> GetFullWelshSearchableAddressList(int count)
        {
            var searchableAddresses = new List<SearchableAddress>();
            var addressHelper = new SearchableAddressHelper();
            for (var index = 0; index < count; index++)
            {
                searchableAddresses.Add((SearchableAddress)addressHelper.NewFullWelshAddress<SearchableAddress>(index));
            }

            return searchableAddresses;
        }

        private IEnumerable<FullAddressDto> GetFullAddressDtos(int count)
        {
            var dtos = new List<FullAddressDto>();
            for (var index = 0; index < count; index++)
            {
                dtos.Add(DtoHelper.GetFullAddressDto(index));
            }

            return dtos;
        }

        private IEnumerable<FullWelshAddressDto> GetFullWelshAddressDtos(int count)
        {
            var dtos = new List<FullWelshAddressDto>();
            for (var index = 0; index < count; index++)
            {
                dtos.Add(DtoHelper.GetFullWelshAddressDto(index));
            }

            return dtos;
        }

        private void AssertSimpleAddressesAreEqual(List<SearchableAddress> entities, List<SimpleAddressDto> dtos)
        {
            for (var index = 0; index < entities.Count; index++)
            {
                this.AssertSimpleAddress(entities[index], dtos[index]);
            };
        }

        private void AssertSimpleWelshAddressesAreEqual(List<SearchableAddress> entities, List<SimpleWelshAddressDto> dtos)
        {
            for (var index = 0; index < entities.Count; index++)
            {
                this.AssertSimpleWelshAddress(entities[index], dtos[index]);
            };
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

        private void AssertFullAddressAreEqual(List<SearchableAddress> entities, List<FullAddressDto> dtos)
        {
            for (var index = 0; index < entities.Count; index++)
            {
                this.AssertFullAddress(entities[index], dtos[index]);
            };
        }

        private void AssertSimpleAddress(SearchableAddress entity, SimpleAddressDto dto)
        {
            dto.Should().BeEquivalentTo(new SimpleAddressDto
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

        private void AssertFullAddress(SearchableAddress entity, FullAddressDto dto)
        {
            dto.Should().BeEquivalentTo(new FullAddressDto
            {
                Uprn = entity.Uprn,
                AddressLine1 = entity.AddressLine1,
                AddressLine2 = entity.AddressLine2,
                AddressLine3 = entity.AddressLine3,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                Postcode = entity.Postcode,
                PostTown = entity.PostTown,
                SingleLineAddress = entity.SingleLineAddress,
                XCoordinateEasting = entity.XCoordinateEasting,
                YCoordinateNorthing = entity.YCoordinateNorthing,
                BuildingName = entity.BuildingName,
                BuildingNumber = entity.BuildingNumber,
                DepartmentName = entity.DepartmentName,
                DependentLocality = entity.DependentLocality,
                DependentThoroughfare = entity.DependentThoroughfare,
                DoubleDependentLocality = entity.DoubleDependentLocality,
                OrganisationName = entity.OrganisationName,
                PoBoxNumber = entity.PoBoxNumber,
                SubBuildingName = entity.SubBuildingName,
                Thoroughfare = entity.ThoroughFare,
                WelshAddressLine1 = entity.WelshAddressLine1,
                WelshAddressLine2 = entity.WelshAddressLine2,
                WelshAddressLine3 = entity.WelshAddressLine3,
                WelshDependentLocality = entity.WelshDependentLocality,
                WelshDependentThoroughfare = entity.WelshDependentThoroughfare,
                WelshDoubleDependentLocality = entity.WelshDoubleDependentLocality,
                WelshPostTown = entity.WelshPostTown,
                WelshSingleLineAddress = entity.WelshSingleLineAddress,
                WelshThoroughfare = entity.WelshThoroughfare,
                Country = entity.Country
            });
        }

        private void AssertFullWelshAddressAreEqual(List<SearchableAddress> entities, List<FullWelshAddressDto> dtos)
        {
            for (var index = 0; index < entities.Count; index++)
            {
                this.AssertFullWelshAddress(entities[index], dtos[index]);
            };
        }

        private void AssertFullWelshAddress(SearchableAddress entity, FullWelshAddressDto dto)
        {
            dto.Should().BeEquivalentTo(new FullWelshAddressDto
            {
                Uprn = entity.Uprn,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                Postcode = entity.Postcode,
                XCoordinateEasting = entity.XCoordinateEasting,
                YCoordinateNorthing = entity.YCoordinateNorthing,
                BuildingName = entity.BuildingName,
                BuildingNumber = entity.BuildingNumber,
                DepartmentName = entity.DepartmentName,
                OrganisationName = entity.OrganisationName,
                PoBoxNumber = entity.PoBoxNumber,
                SubBuildingName = entity.SubBuildingName,
                AddressLine1 = entity.WelshAddressLine1,
                AddressLine2 = entity.WelshAddressLine2,
                AddressLine3 = entity.WelshAddressLine3,
                DependentLocality = entity.WelshDependentLocality,
                DependentThoroughfare = entity.WelshDependentThoroughfare,
                DoubleDependentLocality = entity.WelshDoubleDependentLocality,
                PostTown = entity.WelshPostTown,
                SingleLineAddress = entity.WelshSingleLineAddress,
                Thoroughfare = entity.WelshThoroughfare,
                Country = entity.Country
            });
        }

        private IAddressPostcodeSearch CreateSut()
        {
            return new AddressPostcodeSearch(this.mockReadOnlyRepository,
                                            this.mockAddressConverter,
                                            this.mockWelshAddressConverter,
                                            this.mockAuditLoggerHelper,
                                            this.mockConfigurationSettings);
        }
    }
}