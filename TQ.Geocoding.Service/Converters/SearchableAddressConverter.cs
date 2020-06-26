using System.Collections.Generic;
using System.Linq;
using TQ.Geocoding.Dto.Address;
using TQ.Geocoding.Models;
using TQ.Geocoding.Service.Helpers;
using TQ.Geocoding.Service.Interface.Converters;

namespace TQ.Geocoding.Service.Converters
{
    public class SearchableAddressConverter : ISearchableAddressConverter
    {
        /// <inheritdoc/>
        public FullAddressDto ToFullAddressDto(SearchableAddress model)
        {
            return model != null ? NewFullAddressDto(model) : null;
        }

        /// <inheritdoc/>
        public SimpleAddressDto ToSimpleAddressDto(SearchableAddress model)
        {
            return model != null ? NewSimpleAddressDto(model) : null;
        }

        /// <inheritdoc/>
        public IEnumerable<SimpleAddressDto> ToSimpleAddressDtoList(IEnumerable<SearchableAddress> models)
        {
            var simpleAddressList = models?.ToList().Select(sa => NewSimpleAddressDto(sa)).ToList();
            simpleAddressList?.Sort(new AddressComparerHelper());
            return simpleAddressList;
        }

        /// <inheritdoc/>
        public IEnumerable<FullAddressDto> ToFullAddressDtoList(IEnumerable<SearchableAddress> models)
        {
            var fullAddressList = models?.ToList().Select(sa => NewFullAddressDto(sa)).ToList();
            fullAddressList?.Sort(new AddressComparerHelper());
            return fullAddressList;
        }

        private static FullAddressDto NewFullAddressDto(SearchableAddress model)
        {
            return new FullAddressDto
            {
                Uprn = model.Uprn,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                AddressLine3 = model.AddressLine3,
                BuildingName = model.BuildingName,
                BuildingNumber = model.BuildingNumber,
                DepartmentName = model.DepartmentName,
                DependentLocality = model.DependentLocality,
                DependentThoroughfare = model.DependentThoroughfare,
                DoubleDependentLocality = model.DoubleDependentLocality,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                OrganisationName = model.OrganisationName,
                PoBoxNumber = model.PoBoxNumber,
                PostTown = model.PostTown,
                Postcode = model.Postcode,
                SingleLineAddress = model.SingleLineAddress,
                SubBuildingName = model.SubBuildingName,
                Thoroughfare = model.ThoroughFare,
                XCoordinateEasting = model.XCoordinateEasting,
                YCoordinateNorthing = model.YCoordinateNorthing,
                WelshAddressLine1 = model.WelshAddressLine1,
                WelshAddressLine2 = model.WelshAddressLine2,
                WelshAddressLine3 = model.WelshAddressLine3,
                WelshDependentThoroughfare = model.WelshDependentThoroughfare,
                WelshThoroughfare = model.WelshThoroughfare,
                WelshDoubleDependentLocality = model.WelshDoubleDependentLocality,
                WelshDependentLocality = model.WelshDependentLocality,
                WelshPostTown = model.WelshPostTown,
                WelshSingleLineAddress = model.WelshSingleLineAddress,
                Country = model.Country
            };
        }

        private static SimpleAddressDto NewSimpleAddressDto(SearchableAddress model)
        {
            return new SimpleAddressDto
            {
                Uprn = model.Uprn,
                SingleLineAddress = model.SingleLineAddress,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                AddressLine3 = model.AddressLine3,
                Postcode = model.Postcode,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                Town = model.PostTown,
                YCoordinateNorthing = model.YCoordinateNorthing,
                XCoordinateEasting = model.XCoordinateEasting,
                Country = model.Country
            };
        }
    }
}