using System.Collections.Generic;
using TQ.Geocoding.Dto.Location;
using TQ.Geocoding.Models;

namespace TQ.Geocoding.Service.Interface.Converters
{
    public interface ILocationCoordinateConverter
    {
        /// <summary>
        /// Converts a location coordinate entity into a location coordinate dto
        /// </summary>
        /// <param name="entity">the entity to convert</param>
        /// <returns>a <see cref="LocationCoordinateDto"/> converted from the model></returns>
        LocationCoordinateDto ToLocationCoordinateDto(vwSearchableLocationDetails entity);

        /// <summary>
        /// Converts an enumeration of location coordinate entities into a location coordinate dto list
        /// </summary>
        /// <param name="entities">the entities to convert</param>
        /// <returns>a <see cref="IEnumerable{LocationCoordinateDto}"/> converted from the entities></returns>
        IEnumerable<LocationCoordinateDto> ToLocationCoordinateDtoList(IEnumerable<vwSearchableLocationDetails> entities);
    }
}