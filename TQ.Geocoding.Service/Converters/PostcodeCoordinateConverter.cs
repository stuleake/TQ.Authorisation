using TQ.Geocoding.Dto.Dtos;
using TQ.Geocoding.Models;
using TQ.Geocoding.Service.Interface.Converters;

namespace TQ.Geocoding.Service.Converters
{
    public class PostcodeCoordinateConverter : IPostcodeCoordinateConverter
    {
        /// <inheritdoc/>
        public PostcodeCoordinateDto ToPostcodeCoordinateDto(PostcodeCoordinates entity)
        {
            return entity != null ? NewPostcodeCoordinateDto(entity) : null;
        }

        private PostcodeCoordinateDto NewPostcodeCoordinateDto(PostcodeCoordinates entity)
        {
            return new PostcodeCoordinateDto
            {
                Country = entity.Country,
                CountyUnitary = entity.CountyUnitary,
                DistrictBorough = entity.DistrictBorough,
                GeometryX = entity.GeometryX,
                GeometryY = entity.GeometryY,
                PopulatedPlace = entity.PopulatedPlace,
                Postcode = entity.Postcode
            };
        }
    }
}