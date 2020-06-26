using System.Threading.Tasks;
using TQ.Geocoding.Dto.Address;

namespace TQ.Geocoding.Service.Interface.Search
{
    public interface IAddressUprnSearch
    {
        /// <summary>
        /// Returns a <see cref="FullAddressDto"/> that matches the specified uprn
        /// </summary>
        /// <param name="uprn">the uprn to search for</param>
        /// <returns>a <see cref="Task"/>that contains a <see cref="FullAddressDto"/></returns>
        Task<FullAddressDto> GetFullAddressByUprnAsync(long uprn);

        /// <summary>
        /// Returns a <see cref="SimpleAddressDto"/> that matches the specified uprn
        /// </summary>
        /// <param name="uprn">the uprn to search for</param>
        /// <returns>a <see cref="Task"/>that contains a <see cref="SimpleAddressDto"/></returns>
        Task<SimpleAddressDto> GetSimpleAddressByUprnAsync(long uprn);

        /// <summary>
        /// Returns a <see cref="FullWelshAddressDto"/> that matches the specified uprn
        /// </summary>
        /// <param name="uprn">the uprn to search for</param>
        /// <returns>a <see cref="Task"/>that contains a <see cref="FullWelshAddressDto"/></returns>
        Task<FullWelshAddressDto> GetFullWelshAddressByUprnAsync(long uprn);

        /// <summary>
        /// Returns a <see cref="SimpleWelshAddressDto"/> that matches the specified uprn
        /// </summary>
        /// <param name="uprn">the uprn to search for</param>
        /// <returns>a <see cref="Task"/>that contains a <see cref="SimpleWelshAddressDto"/></returns>
        Task<SimpleWelshAddressDto> GetSimpleWelshAddressByUprnAsync(long uprn);
    }
}