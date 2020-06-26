using System.Collections.Generic;
using System.Linq;
using TQ.Geocoding.Dto.Location;
using TQ.Geocoding.Models;
using TQ.Geocoding.Service.Interface.Converters;

namespace TQ.Geocoding.Service.Converters
{
    public class LocationCoordinateConverter : ILocationCoordinateConverter
    {
        /// <inheritdoc/>
        public LocationCoordinateDto ToLocationCoordinateDto(vwSearchableLocationDetails entity)
        {
            return entity != null ? NewLocationCoordinateDto(entity) : null;
        }

        /// <inheritdoc/>
        public IEnumerable<LocationCoordinateDto> ToLocationCoordinateDtoList(IEnumerable<vwSearchableLocationDetails> entities)
        {
            return entities?.ToList().Select(sa => ToLocationCoordinateDto(sa));
        }

        private LocationCoordinateDto NewLocationCoordinateDto(vwSearchableLocationDetails entity)
        {
            return new LocationCoordinateDto
            {
                LocationId = entity.LocationId,
                GeometryX = entity.GeometryX,
                GeometryY = entity.GeometryY,
                SearchLocation = entity.SearchLocation,
                SearchLanguage = entity.SearchLanguage,
                Place = entity.Place,
                PlaceType = entity.PlaceType,
                PlaceSubType = entity.PlaceSubType,
                PostcodeDistrict = entity.PostcodeDistrict,
                PopulatedPlace = entity.PopulatedPlace,
                DistrictBorough = entity.DistrictBorough,
                CountyUnitary = entity.CountyUnitary,
                Region = entity.Region,
                Country = entity.Country
            };
        }
    }
}