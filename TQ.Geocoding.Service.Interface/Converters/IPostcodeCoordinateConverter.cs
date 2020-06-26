using TQ.Geocoding.Dto.Dtos;
using TQ.Geocoding.Models;

namespace TQ.Geocoding.Service.Interface.Converters
{
    public interface IPostcodeCoordinateConverter
    {
        /// <summary>
        /// Converts a postcode coordinate entity into a postcode coordinate dto
        /// </summary>
        /// <param name="entity">the entity to convert</param>
        /// <returns>a <see cref="PostcodeCoordinateDto"/> converted from the model></returns>
        PostcodeCoordinateDto ToPostcodeCoordinateDto(PostcodeCoordinates entity);
    }
}