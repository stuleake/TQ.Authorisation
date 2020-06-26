using FluentAssertions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Geocoding.Api.Test;
using TQ.Geocoding.API.Test.Helpers.Dto;
using TQ.Geocoding.API.Test.Helpers.Model;
using TQ.Geocoding.Dto.Address;
using TQ.Geocoding.Models;
using TQ.Geocoding.Repository;
using TQ.Geocoding.Requests;
using TQ.Geocoding.Service.Config;
using TQ.Geocoding.Service.Interface.Builders;
using TQ.Geocoding.Service.Interface.Converters;
using TQ.Geocoding.Service.Interface.Helpers;
using TQ.Geocoding.Service.Interface.Search;
using TQ.Geocoding.Service.Search;
using Xunit;

namespace TQ.Geocoding.API.Test.Services
{
    public class AddressTextSearchTests
    {
        private readonly IReadOnlyRepository<SearchableAddress> mockReadOnlyRepository = Mock.Create<IReadOnlyRepository<SearchableAddress>>();
        private readonly IReadOnlyRepository<vwSearchableAddress> mockWelshReadOnlyRepository = Mock.Create<IReadOnlyRepository<vwSearchableAddress>>();
        private readonly ISearchableAddressConverter mockConverter = Mock.Create<ISearchableAddressConverter>();
        private readonly ISearchableWelshAddressConverter mockWelshConverter = Mock.Create<ISearchableWelshAddressConverter>();
        private readonly IPredicateBuilder mockPredicateBuilder = Mock.Create<IPredicateBuilder>();
        private readonly IAddressAuditLoggerHelper mockAuditLoggerHelper = Mock.Create<IAddressAuditLoggerHelper>();
        private readonly IOptions<ConfigurationSettings> mockConfigurationSettings = Options.Create(TestHelper.GetConfigurationSettings());

        private const int ListCount = 1;
        private const string ExpectedErrorMessage = "Value cannot be null. (Parameter 'request is null')";
        private const string RequestSearchString = "REST BAY COURT CF36 3UN";
        private const string SearchableAddressSingleLineAddress = "FLAT 2 REST BAY COURT, 53 REST BAY CLOSE, PORTHCAWL, CF36 3UN";
        private const string PredicateSingleLineAddress = "SingleLineAddress 0";
        private const string PredicateWelshSingleLineAddress = "WelshSingleLineAddress 0";

        [Fact]
        public async void GetSimpleAddressByTextSuccess()
        {
            // Arrange
            var maxRowCount = mockConfigurationSettings.Value.MaxRowCount;
            IEnumerable<SearchableAddress> expectedModels = this.GetFullSearchableAddressList(ListCount);
            IEnumerable<SimpleAddressDto> expectedDtos = this.GetSimpleAddressDtos(ListCount);

            GetAddressByTextRequest getAddressRequest = new GetAddressByTextRequest()
            {
                SearchString = RequestSearchString
            };

            Expression<Func<SearchableAddress, bool>> predicate = searchableAddress => searchableAddress.SingleLineAddress == PredicateSingleLineAddress;

            Mock.Arrange(() => mockPredicateBuilder.BuildPredicate(getAddressRequest.SearchString)).Returns(predicate).MustBeCalled();
            Mock.Arrange(() => mockReadOnlyRepository.ListAsync(predicate, maxRowCount)).Returns(Task.FromResult(expectedModels)).MustBeCalled();
            Mock.Arrange(() => mockConverter.ToSimpleAddressDtoList(expectedModels)).Returns(expectedDtos).MustBeCalled();

            var sut = this.CreateSut();

            // Act
            var result = await sut.GetSimpleAddressByTextAsync(getAddressRequest);

            // Assert
            Mock.Assert(() => mockPredicateBuilder.BuildPredicate(getAddressRequest.SearchString), Occurs.Once());
            Mock.Assert(() => mockReadOnlyRepository.ListAsync(predicate, maxRowCount), Occurs.Once());
            Mock.Assert(() => mockConverter.ToSimpleAddressDtoList(expectedModels), Occurs.Once());
            result.Should().NotBeNull();
            result.Should().Equal(expectedDtos);
        }

        [Fact]
        public async void GetSimpleAddressByTextThrowsArgumentNullExceptionWhenInputIsNull()
        {
            // Arrange
            var sut = this.CreateSut();
            GetAddressByTextRequest getAddressRequest = null;

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetSimpleAddressByTextAsync(getAddressRequest));

            ex.Message.Should().Be(ExpectedErrorMessage);
        }

        [Fact]
        public async void GetSimpleWelshAddressByTextSuccess()
        {
            // Arrange
            var maxRowCount = mockConfigurationSettings.Value.MaxRowCount;
            IEnumerable<vwSearchableAddress> expectedModels = this.GetFullWelshSearchableAddressList(ListCount);
            IEnumerable<SimpleWelshAddressDto> expectedDtos = this.GetSimpleWelshAddressDtos(ListCount);

            GetAddressByTextRequest getAddressRequest = new GetAddressByTextRequest()
            {
                SearchString = RequestSearchString
            };

            vwSearchableAddress searchableAddress = new vwSearchableAddress()
            {
                SingleLineAddress = SearchableAddressSingleLineAddress
            };

            Expression<Func<vwSearchableAddress, bool>> predicate = searchableAddress => searchableAddress.SingleLineAddress == PredicateWelshSingleLineAddress;

            Mock.Arrange(() => mockPredicateBuilder.BuildWelshPredicate(getAddressRequest.SearchString)).Returns(predicate).MustBeCalled();
            Mock.Arrange(() => mockWelshReadOnlyRepository.ListAsync(predicate, maxRowCount)).Returns(Task.FromResult(expectedModels)).MustBeCalled();
            Mock.Arrange(() => mockWelshConverter.ToSimpleAddressDtoList(expectedModels)).Returns(expectedDtos).MustBeCalled();

            var sut = this.CreateSut();

            // Act
            var result = await sut.GetSimpleWelshAddressByTextAsync(getAddressRequest);

            // Assert
            Mock.Assert(() => mockPredicateBuilder.BuildWelshPredicate(getAddressRequest.SearchString), Occurs.Once());
            Mock.Assert(() => mockWelshReadOnlyRepository.ListAsync(predicate, maxRowCount), Occurs.Once());
            Mock.Assert(() => mockWelshConverter.ToSimpleAddressDtoList(expectedModels), Occurs.Once());
            result.Should().NotBeNull();
            result.Should().Equal(expectedDtos);
        }

        [Fact]
        public async void GetSimpleWelshAddressByTextThrowsArgumentNullExceptionWhenInputIsNull()
        {
            // Arrange
            var sut = this.CreateSut();
            GetAddressByTextRequest getAddressRequest = null;

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetSimpleWelshAddressByTextAsync(getAddressRequest));

            ex.Message.Should().Be(ExpectedErrorMessage);
        }

        [Fact]
        public async void GetFullAddressByTextSuccess()
        {
            // Arrange
            var maxRowCount = mockConfigurationSettings.Value.MaxRowCount;
            IEnumerable<SearchableAddress> expectedModels = this.GetFullSearchableAddressList(ListCount);
            IEnumerable<FullAddressDto> expectedDtos = this.GetFullAddressDtos(ListCount);

            GetAddressByTextRequest getAddressRequest = new GetAddressByTextRequest()
            {
                SearchString = RequestSearchString
            };

            Expression<Func<SearchableAddress, bool>> predicate = searchableAddress => searchableAddress.SingleLineAddress == PredicateSingleLineAddress;

            Mock.Arrange(() => mockPredicateBuilder.BuildPredicate(getAddressRequest.SearchString)).Returns(predicate).MustBeCalled();
            Mock.Arrange(() => mockReadOnlyRepository.ListAsync(predicate, maxRowCount)).Returns(Task.FromResult(expectedModels)).MustBeCalled();
            Mock.Arrange(() => mockConverter.ToFullAddressDtoList(expectedModels)).Returns(expectedDtos).MustBeCalled();

            var sut = this.CreateSut();

            // Act
            var result = await sut.GetFullAddressByTextAsync(getAddressRequest);

            // Assert
            Mock.Assert(() => mockPredicateBuilder.BuildPredicate(getAddressRequest.SearchString), Occurs.Once());
            Mock.Assert(() => mockReadOnlyRepository.ListAsync(predicate, maxRowCount), Occurs.Once());
            Mock.Assert(() => mockConverter.ToFullAddressDtoList(expectedModels), Occurs.Once());
            result.Should().NotBeNull();
            result.Should().Equal(expectedDtos);
        }

        [Fact]
        public async void GetFullAddressByTextThrowsArgumentNullExceptionWhenInputIsNull()
        {
            // Arrange
            var sut = this.CreateSut();
            GetAddressByTextRequest getAddressRequest = null;

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetFullAddressByTextAsync(getAddressRequest));

            ex.Message.Should().Be(ExpectedErrorMessage);
        }

        [Fact]
        public async void GetFullWelshAddressByTextSuccess()
        {
            // Arrange
            var maxRowCount = mockConfigurationSettings.Value.MaxRowCount;
            IEnumerable<vwSearchableAddress> expectedModels = this.GetFullWelshSearchableAddressList(ListCount);
            IEnumerable<FullWelshAddressDto> expectedDtos = this.GetFullWelshAddressDtos(ListCount);

            GetAddressByTextRequest getAddressRequest = new GetAddressByTextRequest()
            {
                SearchString = RequestSearchString
            };

            Expression<Func<vwSearchableAddress, bool>> predicate = searchableAddress => searchableAddress.WelshSingleLineAddress == PredicateWelshSingleLineAddress;

            Mock.Arrange(() => mockPredicateBuilder.BuildWelshPredicate(getAddressRequest.SearchString)).Returns(predicate).MustBeCalled();
            Mock.Arrange(() => mockWelshReadOnlyRepository.ListAsync(predicate, maxRowCount)).Returns(Task.FromResult(expectedModels)).MustBeCalled();
            Mock.Arrange(() => mockWelshConverter.ToFullAddressDtoList(expectedModels)).Returns(expectedDtos).MustBeCalled();

            var sut = this.CreateSut();

            // Act
            var result = await sut.GetFullWelshAddressByTextAsync(getAddressRequest);

            // Assert
            Mock.Assert(() => mockPredicateBuilder.BuildWelshPredicate(getAddressRequest.SearchString), Occurs.Once());
            Mock.Assert(() => mockWelshReadOnlyRepository.ListAsync(predicate, maxRowCount), Occurs.Once());
            Mock.Assert(() => mockWelshConverter.ToFullAddressDtoList(expectedModels), Occurs.Once());
            result.Should().NotBeNull();
            result.Should().Equal(expectedDtos);
        }

        [Fact]
        public async void GetFullWelshAddressByTextThrowsArgumentNullExceptionWhenInputIsNull()
        {
            // Arrange
            var sut = this.CreateSut();
            GetAddressByTextRequest getAddressRequest = null;

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetFullWelshAddressByTextAsync(getAddressRequest));

            ex.Message.Should().Be(ExpectedErrorMessage);
        }

        private IEnumerable<SearchableAddress> GetFullSearchableAddressList(int count)
        {
            var searchableAddresses = new List<SearchableAddress>();
            var modelHelper = new SearchableAddressHelper();
            for (var index = 0; index < count; index++)
            {
                searchableAddresses.Add((SearchableAddress)modelHelper.NewFullAddress<SearchableAddress>(index));
            }

            return searchableAddresses;
        }

        private IEnumerable<vwSearchableAddress> GetFullWelshSearchableAddressList(int count)
        {
            var searchableAddresses = new List<vwSearchableAddress>();
            var modelHelper = new SearchableAddressHelper();
            for (var index = 0; index < count; index++)
            {
                searchableAddresses.Add((vwSearchableAddress)modelHelper.NewFullWelshAddress<vwSearchableAddress>(count));
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

        private IEnumerable<SimpleWelshAddressDto> GetSimpleWelshAddressDtos(int count)
        {
            var dtos = new List<SimpleWelshAddressDto>();
            for (var index = 0; index < count; index++)
            {
                dtos.Add(DtoHelper.GetSimpleWelshAddressDto(index));
            }

            return dtos;
        }

        private IAddressTextSearch CreateSut()
        {
            return new AddressTextSearch(mockReadOnlyRepository,
                mockWelshReadOnlyRepository,
                mockConverter,
                mockWelshConverter,
                mockPredicateBuilder,
                mockAuditLoggerHelper,
                mockConfigurationSettings);
        }
    }
}