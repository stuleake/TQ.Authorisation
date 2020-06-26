using TQ.Geocoding.Models;

namespace TQ.Geocoding.API.Test.Helpers.Model
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SearchableAddressHelper
    {
        public ISearchableAddress NewFullWelshAddress<TEntity>(int index)
        {
            if (typeof(TEntity) == typeof(SearchableAddress))
            {
                return NewFullWelshAddress(index);
            }

            return NewFullWelshAddressView(index);
        }

        public ISearchableAddress NewSimpleWelshAddress<TEntity>(int index)
        {
            if (typeof(TEntity) == typeof(SearchableAddress))
            {
                return NewSimpleWelshAddress(index);
            }

            return NewSimpleWelshAddressView(index);
        }

        public ISearchableAddress NewFullAddress<TEntity>(int index)
        {
            if (typeof(TEntity) == typeof(SearchableAddress))
            {
                return NewFullAddress(index);
            }

            return NewFullAddressView(index);
        }

        public ISearchableAddress NewSimpleAddress<TEntity>(int index)
        {
            if (typeof(TEntity) == typeof(SearchableAddress))
            {
                return NewSimpleAddress(index);
            }

            return NewSimpleAddressView(index);
        }

        private static SearchableAddress NewFullWelshAddress(int index)
        {
            return new SearchableAddress
            {
                Id = index,
                LoadId = index,
                PostcodeNoSpaces = $"postcodenospaces{index}",
                Uprn = index,
                BuildingName = $"BuildingName {index}",
                BuildingNumber = index,
                DepartmentName = $"DepartmentName {index}",
                Latitude = index,
                Longitude = index,
                OrganisationName = $"OrganisationName {index}",
                PoBoxNumber = $"PoBoxNumber {index}",
                Postcode = $"Postcode {index}",
                SubBuildingName = $"SubBuildingName {index}",
                ThoroughFare = $"ThoroughFare {index}",
                XCoordinateEasting = index,
                YCoordinateNorthing = index,
                WelshAddressLine1 = $"WelshAddressLine1 {index}",
                WelshAddressLine2 = $"WelshAddressLine2 {index}",
                WelshAddressLine3 = $"WelshAddressLine3 {index}",
                WelshDependentLocality = $"WelshDependentLocality {index}",
                WelshDependentThoroughfare = $"WelshDependentThoroughfare {index}",
                WelshDoubleDependentLocality = $"WelshDoubleDependentLocality {index}",
                WelshSingleLineAddress = $"WelshSingleLineAddress {index}",
                WelshPostTown = $"WelshPostTown {index}",
                WelshThoroughfare = $"WelshThoroughfare {index}",
                Country = "W"
            };
        }

        private static vwSearchableAddress NewFullWelshAddressView(int index)
        {
            return new vwSearchableAddress
            {
                Id = index,
                LoadId = index,
                PostcodeNoSpaces = $"postcodenospaces{index}",
                Uprn = index,
                AddressLine1 = $"AddressLine1 {index}",
                AddressLine2 = $"AddressLine2 {index}",
                AddressLine3 = $"AddressLine3 {index}",
                BuildingName = $"BuildingName {index}",
                BuildingNumber = index,
                DepartmentName = $"DepartmentName {index}",
                DependentLocality = $"DependentLocality {index}",
                DependentThoroughfare = $"DependentThoroughfare {index}",
                DoubleDependentLocality = $"DoubleDependentLocality {index}",
                Latitude = index,
                Longitude = index,
                OrganisationName = $"OrganisationName {index}",
                PoBoxNumber = $"PoBoxNumber {index}",
                PostTown = $"PostTown {index}",
                Postcode = $"Postcode {index}",
                SingleLineAddress = $"SingleLineAddress {index}",
                SubBuildingName = $"SubBuildingName {index}",
                ThoroughFare = $"ThoroughFare {index}",
                XCoordinateEasting = index,
                YCoordinateNorthing = index,
                WelshAddressLine1 = $"WelshAddressLine1 {index}",
                WelshAddressLine2 = $"WelshAddressLine2 {index}",
                WelshAddressLine3 = $"WelshAddressLine3 {index}",
                WelshDependentLocality = $"WelshDependentLocality {index}",
                WelshDependentThoroughfare = $"WelshDependentThoroughfare {index}",
                WelshDoubleDependentLocality = $"WelshDoubleDependentLocality {index}",
                WelshSingleLineAddress = $"WelshSingleLineAddress {index}",
                WelshPostTown = $"WelshPostTown {index}",
                WelshThoroughfare = $"WelshThoroughfare {index}",
                Country = "W"
            };
        }

        private static SearchableAddress NewSimpleWelshAddress(int index)
        {
            return new SearchableAddress
            {
                Uprn = index,
                WelshSingleLineAddress = $"WelshSingleLineAddress {index}",
                WelshAddressLine1 = $"WelshAddressLine1 {index}",
                WelshAddressLine2 = $"WelshAddressLine2 {index}",
                WelshAddressLine3 = $"WelshAddressLine3 {index}",
                Postcode = $"Postcode {index}",
                Latitude = index,
                Longitude = index,
                WelshPostTown = $"WelshPostTown {index}",
                YCoordinateNorthing = index,
                XCoordinateEasting = index,
                Country = "W"
            };
        }

        private static vwSearchableAddress NewSimpleWelshAddressView(int index)
        {
            return new vwSearchableAddress
            {
                Uprn = index,
                WelshSingleLineAddress = $"WelshSingleLineAddress {index}",
                WelshAddressLine1 = $"WelshAddressLine1 {index}",
                WelshAddressLine2 = $"WelshAddressLine2 {index}",
                WelshAddressLine3 = $"WelshAddressLine3 {index}",
                Postcode = $"Postcode {index}",
                Latitude = index,
                Longitude = index,
                WelshPostTown = $"WelshPostTown {index}",
                YCoordinateNorthing = index,
                XCoordinateEasting = index,
                Country = "W"
            };
        }

        private static SearchableAddress NewSimpleAddress(int index)
        {
            return new SearchableAddress
            {
                Uprn = index,
                SingleLineAddress = $"single line address {index}",
                AddressLine1 = $"AddressLine1 {index}",
                AddressLine2 = $"AddressLine2 {index}",
                AddressLine3 = $"AddressLine3 {index}",
                Postcode = $"Postcode {index}",
                Latitude = index,
                Longitude = index,
                PostTown = $"PostTown {index}",
                YCoordinateNorthing = index,
                XCoordinateEasting = index,
                Country = "E"
            };
        }

        private static vwSearchableAddress NewSimpleAddressView(int index)
        {
            return new vwSearchableAddress
            {
                Uprn = index,
                SingleLineAddress = $"single line address {index}",
                AddressLine1 = $"AddressLine1 {index}",
                AddressLine2 = $"AddressLine2 {index}",
                AddressLine3 = $"AddressLine3 {index}",
                Postcode = $"Postcode {index}",
                Latitude = index,
                Longitude = index,
                PostTown = $"PostTown {index}",
                YCoordinateNorthing = index,
                XCoordinateEasting = index,
                Country = "E"
            };
        }

        private static SearchableAddress NewFullAddress(int index)
        {
            return new SearchableAddress
            {
                Id = index,
                LoadId = index,
                PostcodeNoSpaces = $"postcodenospaces{index}",
                Uprn = index,
                AddressLine1 = $"AddressLine1 {index}",
                AddressLine2 = $"AddressLine2 {index}",
                AddressLine3 = $"AddressLine3 {index}",
                BuildingName = $"BuildingName {index}",
                BuildingNumber = index,
                DepartmentName = $"DepartmentName {index}",
                DependentLocality = $"DependentLocality {index}",
                DependentThoroughfare = $"DependentThoroughfare {index}",
                DoubleDependentLocality = $"DoubleDependentLocality {index}",
                Latitude = index,
                Longitude = index,
                OrganisationName = $"OrganisationName {index}",
                PoBoxNumber = $"PoBoxNumber {index}",
                PostTown = $"PostTown {index}",
                Postcode = $"Postcode {index}",
                SingleLineAddress = $"SingleLineAddress {index}",
                SubBuildingName = $"SubBuildingName {index}",
                ThoroughFare = $"ThoroughFare {index}",
                XCoordinateEasting = index,
                YCoordinateNorthing = index,
                WelshAddressLine1 = $"WelshAddressLine1 {index}",
                WelshAddressLine2 = $"WelshAddressLine2 {index}",
                WelshAddressLine3 = $"WelshAddressLine3 {index}",
                WelshDependentLocality = $"WelshDependentLocality {index}",
                WelshDependentThoroughfare = $"WelshDependentThoroughfare {index}",
                WelshDoubleDependentLocality = $"WelshDoubleDependentLocality {index}",
                WelshSingleLineAddress = $"WelshSingleLineAddress {index}",
                WelshPostTown = $"WelshPostTown {index}",
                WelshThoroughfare = $"WelshThoroughfare {index}",
                Country = "E"
            };
        }

        private static vwSearchableAddress NewFullAddressView(int index)
        {
            return new vwSearchableAddress
            {
                Id = index,
                LoadId = index,
                PostcodeNoSpaces = $"postcodenospaces{index}",
                Uprn = index,
                AddressLine1 = $"AddressLine1 {index}",
                AddressLine2 = $"AddressLine2 {index}",
                AddressLine3 = $"AddressLine3 {index}",
                BuildingName = $"BuildingName {index}",
                BuildingNumber = index,
                DepartmentName = $"DepartmentName {index}",
                DependentLocality = $"DependentLocality {index}",
                DependentThoroughfare = $"DependentThoroughfare {index}",
                DoubleDependentLocality = $"DoubleDependentLocality {index}",
                Latitude = index,
                Longitude = index,
                OrganisationName = $"OrganisationName {index}",
                PoBoxNumber = $"PoBoxNumber {index}",
                PostTown = $"PostTown {index}",
                Postcode = $"Postcode {index}",
                SingleLineAddress = $"SingleLineAddress {index}",
                SubBuildingName = $"SubBuildingName {index}",
                ThoroughFare = $"ThoroughFare {index}",
                XCoordinateEasting = index,
                YCoordinateNorthing = index,
                WelshAddressLine1 = $"WelshAddressLine1 {index}",
                WelshAddressLine2 = $"WelshAddressLine2 {index}",
                WelshAddressLine3 = $"WelshAddressLine3 {index}",
                WelshDependentLocality = $"WelshDependentLocality {index}",
                WelshDependentThoroughfare = $"WelshDependentThoroughfare {index}",
                WelshDoubleDependentLocality = $"WelshDoubleDependentLocality {index}",
                WelshSingleLineAddress = $"WelshSingleLineAddress {index}",
                WelshPostTown = $"WelshPostTown {index}",
                WelshThoroughfare = $"WelshThoroughfare {index}",
                Country = "E"
            };
        }
    }
}
