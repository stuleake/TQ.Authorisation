using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.Geocoding.Dto.Location;

namespace TQ.Geocoding.Service.Interface.Search
{
    public interface ILocationCoordinateSearch 
    {
        /// <summary>
        /// Returns an <see cref="IEnumerable{LocationCoordinateDto}"/> that matches the specified id
        /// </summary>
        /// <param name="id">the id to search for</param>
        /// <returns>a <see cref="Task"/>that contains an <see cref="IEnumerable{LocationCoordinateDto}"/></returns>
        Task<IEnumerable<LocationCoordinateDto>> GetByIdAsync(string id);
    }
}