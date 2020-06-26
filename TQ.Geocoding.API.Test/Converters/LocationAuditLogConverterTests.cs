using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using TQ.Geocoding.Dtos.Address;
using TQ.Geocoding.Models;
using TQ.Geocoding.Service.Converters;
using TQ.Geocoding.Service.Interface.Converters;
using Xunit;

namespace TQ.Geocoding.API.Test.Converters
{
    public class LocationAuditLogConverterTests
    {
        [Fact]
        public void ToLocationSearchAuditLogDtosSuccess()
        {
            // Arrange
            var models = new List<LocationSearchAuditLog>
            {
                { CreateLocationSeachAuditLog() },
                { CreateLocationSeachAuditLog() },
                { CreateLocationSeachAuditLog() },
            };

            var sut = this.CreateSut();

            // Act
            var dtos = sut.ToLocationSearchAuditLogDtos(models);

            // Assert
            dtos.Should().NotBeNull();
            dtos.Should().NotBeEmpty();
            this.AssertDtosMatchModels(models, dtos.ToList());
        }

        private void AssertDtosMatchModels(List<LocationSearchAuditLog> models, List<LocationSearchAuditLogDto> dtos)
        {
            for (var index = 0; index < models.Count; index++)
            {
                this.AssertAddressSearchAuditLog(models[index], dtos[index]);
            }
        }

        private void AssertAddressSearchAuditLog(LocationSearchAuditLog entity, LocationSearchAuditLogDto dto)
        {
            dto.Should().BeEquivalentTo(new LocationSearchAuditLogDto
            {
                LocationSearch = entity.LocationSearch,
                Id = entity.Id,
                LogDate = entity.LogDate,
                ResultCount = entity.ResultCount
            });
        }

        private static LocationSearchAuditLog CreateLocationSeachAuditLog()
        {
            return new LocationSearchAuditLog
            {
                Id = Guid.NewGuid(),
                LocationSearch = "location search",
                LogDate = DateTime.Now,
                ResultCount = 1
            };
        }

        private ILocationAuditLogConverter CreateSut()
        {
            return new LocationAuditLogConverter();
        }
    }
}