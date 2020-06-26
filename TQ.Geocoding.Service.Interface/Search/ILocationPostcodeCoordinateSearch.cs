using System.Threading.Tasks;
using TQ.Geocoding.Dto.Dtos;

namespace TQ.Geocoding.Service.Interface.Search
{
    public interface ILocationPostcodeCoordinateSearch
    {
        /// <summary>
        /// Returns a <see cref="PostcodeCoordinateDto"/> that matches the specified id
        /// </summary>
        /// <param name="id">the id to search for</param>
        /// <returns>a <see cref="Task"/>that contains a <see cref="PostcodeCoordinateDto"/></returns>
        Task<PostcodeCoordinateDto> GetByIdAsync(string id);
    }
}