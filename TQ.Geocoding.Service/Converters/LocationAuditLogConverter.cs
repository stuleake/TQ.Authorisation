using System.Collections.Generic;
using System.Linq;
using TQ.Geocoding.Dtos.Address;
using TQ.Geocoding.Models;
using TQ.Geocoding.Service.Interface.Converters;

namespace TQ.Geocoding.Service.Converters
{
    public class LocationAuditLogConverter : ILocationAuditLogConverter
    {
        /// <inheritdoc/>
        public IEnumerable<LocationSearchAuditLogDto> ToLocationSearchAuditLogDtos(IEnumerable<LocationSearchAuditLog> models)
        {
            return  models?
                .Select(auditLog => ToLocationSearchAuditLogDto(auditLog))
                .ToList();
        }

        private LocationSearchAuditLogDto ToLocationSearchAuditLogDto(LocationSearchAuditLog model)
        {
            return new LocationSearchAuditLogDto
            {
                Id = model.Id,
                LocationSearch = model.LocationSearch,
                LogDate = model.LogDate,
                ResultCount = model.ResultCount
            };
        }
    }
}