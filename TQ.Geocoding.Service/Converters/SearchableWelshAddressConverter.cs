using System.Collections.Generic;
using System.Linq;
using TQ.Geocoding.Dto.Address;
using TQ.Geocoding.Models;
using TQ.Geocoding.Service.Helpers;
using TQ.Geocoding.Service.Interface.Converters;

namespace TQ.Geocoding.Service.Converters
{
    public class SearchableWelshAddressConverter : ISearchableWelshAddressConverter
    {
        /// <inheritdoc/>
        public FullWelshAddressDto ToFullAddressDto(ISearchableAddress entity)
        {
            return entity != null
                ? CreateFullWelshAddressDto(entity, GetFullWelshDefaultAddressDto(entity))
                : null;
        }

        /// <inheritdoc/>
        public IEnumerable<FullWelshAddressDto> ToFullAddressDtoList(IEnumerable<ISearchableAddress> entities)
        {
            var fullWelshAddressList = entities?.ToList().Select(sa => CreateFullWelshAddressDto(sa, GetFullWelshDefaultAddressDto(sa))).ToList();
            fullWelshAddressList?.Sort(new AddressComparerHelper());
            return fullWelshAddressList;
        }

        /// <inheritdoc/>
        public SimpleWelshAddressDto ToSimpleAddressDto(ISearchableAddress entity)
        {
            return entity != null
                ? CreateSimpleWelshAddressDto(entity, GetSimpleWelshDefaultAddressDto(entity))
                : null;
        }

        /// <inheritdoc/>
        public IEnumerable<SimpleWelshAddressDto> ToSimpleAddressDtoList(IEnumerable<ISearchableAddress> entities)
        {
            var simpleWelshAddressList = entities?.ToList().Select(sa => CreateSimpleWelshAddressDto(sa, GetSimpleWelshDefaultAddressDto(sa))).ToList();
            simpleWelshAddressList?.Sort(new AddressComparerHelper());
            return simpleWelshAddressList;
        }

        private SimpleWelshDefaultAddressDto GetSimpleWelshDefaultAddressDto(ISearchableAddress entity)
        {
            return new SimpleWelshDefaultAddressDto
            {
                SingleLineAddress = string.IsNullOrWhiteSpace(entity.WelshSingleLineAddress) ? entity.SingleLineAddress : entity.WelshSingleLineAddress,
                AddressLine1 = string.IsNullOrWhiteSpace(entity.WelshAddressLine1) ? entity.AddressLine1 : entity.WelshAddressLine1,
                AddressLine2 = string.IsNullOrWhiteSpace(entity.WelshAddressLine2) ? entity.AddressLine2 : entity.WelshAddressLine2,
                AddressLine3 = string.IsNullOrWhiteSpace(entity.WelshAddressLine1) ? entity.AddressLine3 : entity.WelshAddressLine3,
                PostTown = string.IsNullOrWhiteSpace(entity.WelshPostTown) ? entity.PostTown : entity.WelshPostTown,
            };
        }

        private FullWelshDefaultAddressDto GetFullWelshDefaultAddressDto(ISearchableAddress entity)
        {
            return new FullWelshDefaultAddressDto()
            {
                AddressLine1 = string.IsNullOrWhiteSpace(entity.WelshAddressLine1) ? entity.AddressLine1 : entity.WelshAddressLine1,
                AddressLine2 = string.IsNullOrWhiteSpace(entity.WelshAddressLine2) ? entity.AddressLine2 : entity.WelshAddressLine2,
                AddressLine3 = string.IsNullOrWhiteSpace(entity.WelshAddressLine3) ? entity.AddressLine3 : entity.WelshAddressLine3,
                DependentLocality = string.IsNullOrWhiteSpace(entity.WelshDependentLocality) ? entity.DependentLocality : entity.WelshDependentLocality,
                DependentThoroughfare = string.IsNullOrWhiteSpace(entity.WelshDependentThoroughfare) ? entity.DependentThoroughfare : entity.WelshDependentThoroughfare,
                DoubleDependentLocality = string.IsNullOrWhiteSpace(entity.WelshDoubleDependentLocality) ? entity.DoubleDependentLocality : entity.WelshDoubleDependentLocality,
                PostTown = string.IsNullOrWhiteSpace(entity.WelshPostTown) ? entity.PostTown : entity.WelshPostTown,
                SingleLineAddress = string.IsNullOrWhiteSpace(entity.WelshSingleLineAddress) ? entity.SingleLineAddress : entity.WelshSingleLineAddress,
                Thoroughfare = string.IsNullOrWhiteSpace(entity.WelshThoroughfare) ? entity.ThoroughFare : entity.WelshThoroughfare,
            };
        }

        private SimpleWelshAddressDto CreateSimpleWelshAddressDto(ISearchableAddress entity, SimpleWelshDefaultAddressDto defaultAddressDto)
        {
            return new SimpleWelshAddressDto()
            {
                Uprn = entity.Uprn,
                SingleLineAddress = defaultAddressDto.SingleLineAddress,
                AddressLine1 = defaultAddressDto.AddressLine1,
                AddressLine2 = defaultAddressDto.AddressLine2,
                AddressLine3 = defaultAddressDto.AddressLine3,
                Postcode = entity.Postcode,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                PostTown = defaultAddressDto.PostTown,
                YCoordinateNorthing = entity.YCoordinateNorthing,
                XCoordinateEasting = entity.XCoordinateEasting,
                Country = entity.Country
            };
        }

        private FullWelshAddressDto CreateFullWelshAddressDto(ISearchableAddress entity, FullWelshDefaultAddressDto defaultAddressDto)
        {
            return new FullWelshAddressDto
            {
                Uprn = entity.Uprn,
                AddressLine1 = defaultAddressDto.AddressLine1,
                AddressLine2 = defaultAddressDto.AddressLine2,
                AddressLine3 = defaultAddressDto.AddressLine3,
                BuildingName = entity.BuildingName,
                BuildingNumber = entity.BuildingNumber,
                DepartmentName = entity.DepartmentName,
                DependentLocality = defaultAddressDto.DependentLocality,
                DependentThoroughfare = defaultAddressDto.DependentThoroughfare,
                DoubleDependentLocality = defaultAddressDto.DoubleDependentLocality,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                OrganisationName = entity.OrganisationName,
                PoBoxNumber = entity.PoBoxNumber,
                PostTown = defaultAddressDto.PostTown,
                Postcode = entity.Postcode,
                SingleLineAddress = defaultAddressDto.SingleLineAddress,
                SubBuildingName = entity.SubBuildingName,
                Thoroughfare = defaultAddressDto.Thoroughfare,
                XCoordinateEasting = entity.XCoordinateEasting,
                YCoordinateNorthing = entity.YCoordinateNorthing,
                Country = entity.Country
            };
        }
    }
}