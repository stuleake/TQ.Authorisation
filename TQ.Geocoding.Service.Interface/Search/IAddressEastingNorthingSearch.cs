using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.Geocoding.Dto.Address;
using TQ.Geocoding.Requests;

namespace TQ.Geocoding.Service.Interface.Search
{
    public interface IAddressEastingNorthingSearch
    {
        /// <summary>
        /// Returns a <see cref="IEnumerable{SimpleAddressDto}"/> that match the coordinates in the request
        /// </summary>
        /// <param name="request">the request that contains the coordinates to search for</param>
        /// <returns>a <see cref="Task"/>that contains a <see cref="IEnumerable{SimpleAddressDto}"/>
        Task<IEnumerable<SimpleAddressDto>> GetAddressesByEastingNorthingAsync(GetAddressByEastingNorthingRequest request);

        /// <summary>
        /// Returns a <see cref="IEnumerable{SimpleWelshAddressDto}"/> that match the coordinates in the request
        /// </summary>
        /// <param name="request">the request that contains the coordinates to search for</param>
        /// <returns>a <see cref="Task"/>that contains a <see cref="IEnumerable{SimpleWelshAddressDto}"/>
        Task<IEnumerable<SimpleWelshAddressDto>> GetWelshAddressesByEastingNorthingAsync(GetAddressByEastingNorthingRequest request);
    }
}