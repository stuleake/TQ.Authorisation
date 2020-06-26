using System;
using Telerik.JustMock;
using TQ.Geocoding.Models;
using TQ.Geocoding.Repository;
using TQ.Geocoding.Service.Audit;
using TQ.Geocoding.Service.Interface.Audit;
using Xunit;

namespace TQ.Geocoding.API.Test.Services
{
    public class AuditLoggerTests
    {
        private readonly IWriteOnlyRepository<AddressSearchAuditLog> mockAddressRepo = Mock.Create<IWriteOnlyRepository<AddressSearchAuditLog>>(Behavior.Loose);
        private readonly IWriteOnlyRepository<LocationSearchAuditLog> mockLocationRepo = Mock.Create<IWriteOnlyRepository<LocationSearchAuditLog>>(Behavior.Loose);

        [Fact]
        public async void LogAsyncThrowsArgumentNullExceptionWhenEntityIsNull()
        {
            // Arrange
            var sut = this.CreateSutAddressSearchAuditLog();

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.LogAsync(null));
        }

        [Fact]
        public async void LogAsyncAddressSearchAuditLogSuccess()
        {
            // Arrange
            var addressSearchAuditLog = new AddressSearchAuditLog
            {
                AddressSearch = "Address Search",
                Id = Guid.NewGuid(),
                LogDate = DateTime.Now,
                ResultCount = 1
            };

            var sut = this.CreateSutAddressSearchAuditLog();

            // Act
            await sut.LogAsync(addressSearchAuditLog);

            // Assert
            Mock.Assert(() => mockAddressRepo.CreateAsync(Arg.IsAny<AddressSearchAuditLog>()), Occurs.Once());
        }

        [Fact]
        public async void LogAsyncLocationSearchAuditLogSuccess()
        {
            // Arrange
            var locationSearchAuditLog = new LocationSearchAuditLog
            {
                LocationSearch = "Location Search",
                Id = Guid.NewGuid(),
                LogDate = DateTime.Now,
                ResultCount = 1
            };

            var sut = this.CreateSutLocationSearchAuditLog();

            // Act
            await sut.LogAsync(locationSearchAuditLog);

            // Assert
            Mock.Assert(() => mockLocationRepo.CreateAsync(Arg.IsAny<LocationSearchAuditLog>()), Occurs.Once());
        }

        private IAuditLogger<AddressSearchAuditLog> CreateSutAddressSearchAuditLog()
        {
            return new AuditLogger<AddressSearchAuditLog>(this.mockAddressRepo);
        }

        private IAuditLogger<LocationSearchAuditLog> CreateSutLocationSearchAuditLog()
        {
            return new AuditLogger<LocationSearchAuditLog>(this.mockLocationRepo);
        }
    }
}