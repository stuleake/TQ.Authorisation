using System.Collections.Generic;
using TQ.Geocoding.Dtos.Address;
using TQ.Geocoding.Models;

namespace TQ.Geocoding.Service.Interface.Converters
{
    /// <summary>
    /// Interface for the <see cref="IAddressAuditLogConverter"/>
    /// </summary>
    public interface IAddressAuditLogConverter
    {
        /// <summary>
        /// Converts a collection of <see cref="AddressSearchAuditLog"/> models into a collection of <see cref="AddressSearchAuditLogDto"/>
        /// </summary>
        /// <param name="models">the models to convert</param>
        /// <returns>a <see cref="IEnumerable{AddressSearchAuditLogDto}"/> converted from the models></returns>
        IEnumerable<AddressSearchAuditLogDto> ToAddressSearchAuditLogDtos(IEnumerable<AddressSearchAuditLog> models);
    }
}