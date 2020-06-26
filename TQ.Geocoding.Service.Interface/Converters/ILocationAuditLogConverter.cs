using System.Collections.Generic;
using TQ.Geocoding.Dtos.Address;
using TQ.Geocoding.Models;

namespace TQ.Geocoding.Service.Interface.Converters
{
    public interface ILocationAuditLogConverter
    {
        /// <summary>
        /// Converts a collection of <see cref="LocationSearchAuditLog"/> models into a collection of <see cref="LocationSearchAuditLogDto"/>
        /// </summary>
        /// <param name="models">the models to convert</param>
        /// <returns>a <see cref="IEnumerable{LocationSearchAuditLogDto}"/> converted from the models></returns>
        IEnumerable< LocationSearchAuditLogDto> ToLocationSearchAuditLogDtos(IEnumerable<LocationSearchAuditLog> models);
    }
}