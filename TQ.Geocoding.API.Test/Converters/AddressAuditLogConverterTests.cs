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
    public class AddressAuditLogConverterTests
    {
        [Fact]
        public void ToLAddressSearchAuditLogDtosSuccess()
        {
            // Arrange
            var models = new List<AddressSearchAuditLog>
            {
                { CreateAddressSearchAuditLog() },
                { CreateAddressSearchAuditLog() },
                { CreateAddressSearchAuditLog() },
            };

            var sut = this.CreateSut();

            // Act
            var dtos = sut.ToAddressSearchAuditLogDtos(models);

            // Assert
            dtos.Should().NotBeNull();
            dtos.Should().NotBeEmpty();
            this.AssertDtosMatchModels(models, dtos.ToList());
        }

        private void AssertDtosMatchModels(List<AddressSearchAuditLog> models, List<AddressSearchAuditLogDto> dtos)
        {
            for (var index = 0; index < models.Count; index++)
            {
                this.AssertAddressSearchAuditLog(models[index], dtos[index]);
            }
        }

        private void AssertAddressSearchAuditLog(AddressSearchAuditLog entity, AddressSearchAuditLogDto dto)
        {
            dto.Should().BeEquivalentTo(new AddressSearchAuditLogDto
            {
                AddressSearch = entity.AddressSearch,
                Id = entity.Id,
                LogDate = entity.LogDate,
                ResultCount = entity.ResultCount
            });
        }

        private static AddressSearchAuditLog CreateAddressSearchAuditLog()
        {
            return new AddressSearchAuditLog
            {
                Id = Guid.NewGuid(),
                AddressSearch = "address search",
                LogDate = DateTime.Now,
                ResultCount = 1
            };
        }

        private IAddressAuditLogConverter CreateSut()
        {
            return new AddressAuditLogConverter();
        }
    }
}