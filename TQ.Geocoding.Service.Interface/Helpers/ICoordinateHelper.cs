using System.Collections.Generic;
using TQ.Geocoding.Models;
using TQ.Geocoding.Service.Interface.Search;

namespace TQ.Geocoding.Service.Interface.Helpers
{
    public interface ICoordinateHelper
    {
        /// <summary>
        /// Returns a <see cref="ICoordinates"/> with any missing decimal digits filled-in to create Min/Max ranges 
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns>a <see cref="ICoordinates"/> with Max coordinates set up/>
        ICoordinates GetMaxCoordinates(ICoordinates coordinates);

        /// <summary>
        /// Returns a <see cref="IClosestCoordinates"/> that represent the closest coordinates for the searchable addresses
        /// </summary>
        /// <param name="searchableAddresses">the collection of addresses to search</param>
        /// <param name="xCoordinate">the x coordinate</param>
        /// <param name="yCoordinate">the y coordinate</param>
        /// <param name="isLongitudeLatitude">a flag to specify that the coordinates are longitude and latitude </param>
        /// <returns>a <see cref="IClosestCoordinates"/> with the closest coordinates set</returns>
        IClosestCoordinates GetClosestCoordinates(IEnumerable<SearchableAddress> searchableAddresses, decimal xCoordinate, decimal yCoordinate, bool isLongitudeLatitude);
    }
}