using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.Geocoding.Dto.Address;
using TQ.Geocoding.Requests;

namespace TQ.Geocoding.Service.Interface.Search
{
    public interface IAddressTextSearch
    {
        /// <summary>
        /// Returns a collection of <see cref="SimpleAddressDto"/> that match the search text
        /// </summary>
        /// <param name="request">the request that contains the text to search for</param>
        /// <returns>a <see cref="Task"/>that contains a <see cref="IEnumerable{SimpleAddressDto}"/>
        Task<IEnumerable<SimpleAddressDto>> GetSimpleAddressByTextAsync(GetAddressByTextRequest request);

        /// <summary>
        /// Returns a collection of <see cref="SimpleWelshAddressDto"/> that match the search text
        /// </summary>
        /// <param name="request">the request that contains the text to search for</param>
        /// <returns>a <see cref="Task"/>that contains a <see cref="IEnumerable{SimpleWelshAddressDto}"/>
        Task<IEnumerable<SimpleWelshAddressDto>> GetSimpleWelshAddressByTextAsync(GetAddressByTextRequest request);

        /// <summary>
        /// Returns a collection of <see cref="FullAddressDto"/> that match the search text
        /// </summary>
        /// <param name="request">the request that contains the text to search for</param>
        /// <returns>a <see cref="Task"/>that contains a <see cref="IEnumerable{FullAddressDto}"/>
        Task<IEnumerable<FullAddressDto>> GetFullAddressByTextAsync(GetAddressByTextRequest request);

        /// <summary>
        /// Returns a collection of <see cref="FullWelshAddressDto"/> that match the search text
        /// </summary>
        /// <param name="request">the request that contains the text to search for</param>
        /// <returns>a <see cref="Task"/>that contains a <see cref="IEnumerable{FullWelshAddressDto}"/>
        Task<IEnumerable<FullWelshAddressDto>> GetFullWelshAddressByTextAsync(GetAddressByTextRequest request);
    }
}