using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.Geocoding.Dto.Address;

namespace TQ.Geocoding.Service.Interface.Search
{
    public interface IAddressPostcodeSearch
    {
        /// <summary>
        /// Returns a <see cref="IEnumerable{SimpleAddressDto}"/> that match the specified postcode
        /// </summary>
        /// <param name="postcode">the postcode to search for</param>
        /// <returns>a <see cref="IEnumerable{SimpleAddressDto}"/></returns>
        Task<IEnumerable<SimpleAddressDto>> GetSimpleAddressesByPostcodeAsync(string postcode);

        /// <summary>
        /// Returns a <see cref="IEnumerable{FullAddressDto}"/> that match the specified postcode
        /// </summary>
        /// <param name="postcode">the postcode to search for</param>
        /// <returns>a <see cref="IEnumerable{FullAddressDto}"/></returns>
        Task<IEnumerable<FullAddressDto>> GetFullAddressesByPostcodeAsync(string postcode);

        /// <summary>
        /// Returns a <see cref="IEnumerable{SimpleWelshAddressDto}"/> that match the specified postcode
        /// </summary>
        /// <param name="postcode">the postcode to search for</param>
        /// <returns>a <see cref="IEnumerable{SimpleWelshAddressDto}"/></returns>
        Task<IEnumerable<SimpleWelshAddressDto>> GetSimpleWelshAddressesByPostcodeAsync(string postcode);

        /// <summary>
        /// Returns a <see cref="IEnumerable{FullWelshAddressDto}"/> that match the specified postcode
        /// </summary>
        /// <param name="postcode">the postcode to search for</param>
        /// <returns>a <see cref="IEnumerable{FullWelshAddressDto}"/></returns>
        Task<IEnumerable<FullWelshAddressDto>> GetFullWelshAddressesByPostcodeAsync(string postcode);
    }
}