using System.Collections.Generic;
using TQ.Geocoding.Dto.Address;
using TQ.Geocoding.Models;

namespace TQ.Geocoding.Service.Interface.Converters
{
    public interface ISearchableWelshAddressConverter
    {
        /// <summary>
        /// Converts a searchable address to a simple welsh address dto
        /// </summary>
        /// <param name="model">the model to convert</param>
        /// <returns>a <see cref="SimpleWelshAddressDto"/> converted from the model></returns>
        SimpleWelshAddressDto ToSimpleAddressDto(ISearchableAddress model);

        /// <summary>
        /// Converts a searchable address to a full welsh address dto
        /// </summary>
        /// <param name="model">the model to convert</param>
        /// <returns>a <see cref="FullWelshAddressDto"/> converted from the model></returns>
        FullWelshAddressDto ToFullAddressDto(ISearchableAddress model);

        /// <summary>
        /// Converts a collection of searchable addresses to a collection of simple welsh address dto's
        /// </summary>
        /// <param name="models">the collection of models to convert</param>
        /// <returns>a <see cref="IEnumerable{SimpleWelshAddressDto}"/> converted from the models></returns>
        IEnumerable<SimpleWelshAddressDto> ToSimpleAddressDtoList(IEnumerable<ISearchableAddress> models);

        /// <summary>
        /// Converts a collection of searchable addresses to a collection of full welsh address dto's
        /// </summary>
        /// <param name="models">the collection of models to convert</param>
        /// <returns>a <see cref="IEnumerable{FullWelshAddressDto}"/> converted from the models></returns>
        IEnumerable<FullWelshAddressDto> ToFullAddressDtoList(IEnumerable<ISearchableAddress> models);
    }
}