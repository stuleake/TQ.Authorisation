using System.Collections.Generic;
using System.Linq;
using TQ.Geocoding.Dtos.Address;
using TQ.Geocoding.Models;
using TQ.Geocoding.Service.Interface.Converters;

namespace TQ.Geocoding.Service.Converters
{
    public class AddressAuditLogConverter : IAddressAuditLogConverter
    {
        /// <inheritdoc/>
        public IEnumerable<AddressSearchAuditLogDto> ToAddressSearchAuditLogDtos(IEnumerable<AddressSearchAuditLog> models)
        {
            return models?
                .Select(auditLog => ToAddressSearchAuditLogDto(auditLog))
                .ToList();
        }

        private AddressSearchAuditLogDto ToAddressSearchAuditLogDto(AddressSearchAuditLog model)
        {
            return new AddressSearchAuditLogDto
            {
                Id = model.Id,
                AddressSearch = model.AddressSearch,
                LogDate = model.LogDate,
                ResultCount = model.ResultCount
            };
        }
    }
}