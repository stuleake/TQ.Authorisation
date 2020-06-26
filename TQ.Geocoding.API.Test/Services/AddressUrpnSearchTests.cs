using FluentAssertions;
using System;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Geocoding.Dto.Address;
using TQ.Geocoding.Models;
using TQ.Geocoding.Repository;
using TQ.Geocoding.Service.Interface.Converters;
using TQ.Geocoding.Service.Interface.Helpers;
using TQ.Geocoding.Service.Interface.Search;
using TQ.Geocoding.Service.Search;
using Xunit;

namespace TQ.Geocoding.API.Test.Services
{
    public class AddressUrpnSearchTests
    {
        private readonly IReadOnlyRepository<SearchableAddress> mockReadOnlyRepository = Mock.Create<IReadOnlyRepository<SearchableAddress>>();
        private readonly ISearchableAddressConverter mockAddressConverter = Mock.Create<ISearchableAddressConverter>();
        private readonly ISearchableWelshAddressConverter mockWelshAddressConverter = Mock.Create<ISearchableWelshAddressConverter>();
        private readonly IAddressAuditLoggerHelper mockAuditLoggerHelper = Mock.Create<IAddressAuditLoggerHelper>();

        private const string ExpectedErrorMessage = "uprn is not valid";
        private const long Uprn = 1030438;

        [Fact]
        public async void GetFullAddressByUprnSuccess()
        {
            // Arrange
            var expectedModel = this.GetSearchableAddresses();
            var expectedDto = new FullAddressDto
            {
                Uprn = Uprn
            };

            Mock.Arrange(() => mockReadOnlyRepository.FirstOrDefaultAsync(u => u.Uprn == Uprn)).Returns(Task.FromResult(expectedModel)).MustBeCalled();
            Mock.Arrange(() => mockAddressConverter.ToFullAddressDto(expectedModel)).Returns(expectedDto).MustBeCalled();
            
            var sut = this.CreateSut();

            // Act
            var result = await sut.GetFullAddressByUprnAsync(Uprn);

            // Assert
            Mock.Assert(() => mockReadOnlyRepository.FirstOrDefaultAsync(u => u.Uprn == Uprn), Occurs.Once());
            Mock.Assert(() => mockAddressConverter.ToFullAddressDto(expectedModel), Occurs.Once());
            result.Should().NotBeNull();
        }

        [Fact]
        public async void GetSimpleAddressByUprnSuccess()
        {
            // Arrange
            var expectedModel = this.GetSearchableAddresses();
            var expectedDto = new SimpleAddressDto
            {
                Uprn = Uprn
            };

            Mock.Arrange(() => mockReadOnlyRepository.FirstOrDefaultAsync(u => u.Uprn == Uprn)).Returns(Task.FromResult(expectedModel)).MustBeCalled();
            Mock.Arrange(() => mockAddressConverter.ToSimpleAddressDto(expectedModel)).Returns(expectedDto).MustBeCalled();

            var sut = this.CreateSut();

            // Act
            var result = await sut.GetSimpleAddressByUprnAsync(Uprn);

            // Assert
            Mock.Assert(() => mockReadOnlyRepository.FirstOrDefaultAsync(u => u.Uprn == Uprn), Occurs.Once());
            Mock.Assert(() => mockAddressConverter.ToSimpleAddressDto(expectedModel), Occurs.Once());
            result.Should().NotBeNull();
        }

        [Fact]
        public async void GetFullWelshAddressByUprnSuccess()
        {
            // Arrange
            var expectedModel = this.GetSearchableAddresses();
            var expectedDto = new FullWelshAddressDto
            {
                Uprn = Uprn
            };

            Mock.Arrange(() => mockReadOnlyRepository.FirstOrDefaultAsync(u => u.Uprn == Uprn)).Returns(Task.FromResult(expectedModel)).MustBeCalled();
            Mock.Arrange(() => mockWelshAddressConverter.ToFullAddressDto(expectedModel)).Returns(expectedDto).MustBeCalled();

            var sut = this.CreateSut();

            // Act
            var result = await sut.GetFullWelshAddressByUprnAsync(Uprn);

            // Assert
            Mock.Assert(() => mockReadOnlyRepository.FirstOrDefaultAsync(u => u.Uprn == Uprn), Occurs.Once());
            Mock.Assert(() => mockWelshAddressConverter.ToFullAddressDto(expectedModel), Occurs.Once());
            result.Should().NotBeNull();
        }

        [Fact]
        public async void GetSimpleWelshAddressByUprnSuccess()
        {
            // Arrange
            var expectedModel = this.GetSearchableAddresses(); 
            var expectedDto = new SimpleWelshAddressDto
            {
                Uprn = Uprn
            };

            Mock.Arrange(() => mockReadOnlyRepository.FirstOrDefaultAsync(u => u.Uprn == Uprn)).Returns(Task.FromResult(expectedModel)).MustBeCalled();
            Mock.Arrange(() => mockWelshAddressConverter.ToSimpleAddressDto(expectedModel)).Returns(expectedDto).MustBeCalled();

            var sut = this.CreateSut();

            // Act
            var result = await sut.GetSimpleWelshAddressByUprnAsync(Uprn);

            // Assert
            Mock.Assert(() => mockReadOnlyRepository.FirstOrDefaultAsync(u => u.Uprn == Uprn), Occurs.Once());
            Mock.Assert(() => mockWelshAddressConverter.ToSimpleAddressDto(expectedModel), Occurs.Once());
            result.Should().NotBeNull();
        }

        [Fact]
        public async void GetFullAddressByUprnThrowsArgumentExceptionWhenInputIsDefault()
        {
            // Arrange
            var sut = this.CreateSut();
            var urpn = default(long); 

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ArgumentException>(() => sut.GetFullAddressByUprnAsync(urpn));
            ex.Message.Should().Be(ExpectedErrorMessage);
        }

        [Fact]
        public async void GetSimpleAddressByUprnThrowsArgumentExceptionWhenInputIsDefault()
        {
            // Arrange
            var sut = this.CreateSut();
            var urpn = default(long);

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ArgumentException>(() => sut.GetSimpleAddressByUprnAsync(urpn));

            ex.Message.Should().Be(ExpectedErrorMessage);
        }

        [Fact]
        public async void GetFullWelshAddressByUprnThrowsArgumentExceptionWhenInputIsDefault()
        {
            // Arrange
            var sut = this.CreateSut();
            var urpn = default(long);

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ArgumentException>(() => sut.GetFullWelshAddressByUprnAsync(urpn));

            ex.Message.Should().Be(ExpectedErrorMessage);
        }

        [Fact]
        public async void GetSimpleWelshAddressByUprnThrowsArgumentExceptionWhenInputIsDefault()
        {
            // Arrange
            var sut = this.CreateSut();
            var urpn = default(long);

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ArgumentException>(() => sut.GetSimpleWelshAddressByUprnAsync(urpn));

            ex.Message.Should().Be(ExpectedErrorMessage);
        }

        private SearchableAddress GetSearchableAddresses()
        {
            return new SearchableAddress
            {
                Uprn = Uprn
            };
        }

        private IAddressUprnSearch CreateSut()
        {
            return new AddressUprnSearch(mockReadOnlyRepository,
                                        mockAddressConverter,
                                        mockWelshAddressConverter,
                                        mockAuditLoggerHelper);
        }
    }
}