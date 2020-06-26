using System.Collections.Generic;
using TQ.Geocoding.Dto.Address;
using TQ.Geocoding.Models;

namespace TQ.Geocoding.Service.Interface.Converters
{
    public interface ISearchableAddressConverter
    {
        /// <summary>
        /// Converts a searchable address to a full address dto
        /// </summary>
        /// <param name="model">the model to convert</param>
        /// <returns>a <see cref="FullAddressDto"/> converted from the model></returns>
        FullAddressDto ToFullAddressDto(SearchableAddress model);

        /// <summary>
        /// Converts a searchable address to a simple address dto
        /// </summary>
        /// <param name="model">the model to convert</param>
        /// <returns>a <see cref="SimpleAddressDto"/> converted from the model></returns>
        SimpleAddressDto ToSimpleAddressDto(SearchableAddress model);

        /// <summary>
        /// Converts a collection of searchable address to a collection of simple address dto's
        /// </summary>
        /// <param name="models">the collection of model to convert</param>
        /// <returns>a <see cref="IEnumerable{SimpleAddressDto}"/> converted from the models></returns>
        IEnumerable<SimpleAddressDto> ToSimpleAddressDtoList(IEnumerable<SearchableAddress> models);

        /// <summary>
        /// Converts a collection of searchable address to a collection of full address dto's
        /// </summary>
        /// <param name="models">the collection of model to convert</param>
        /// <returns>a <see cref="IEnumerable{FullAddressDto}"/> converted from the models></returns>
        IEnumerable<FullAddressDto> ToFullAddressDtoList(IEnumerable<SearchableAddress> models);
    }
}