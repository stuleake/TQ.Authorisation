using TQ.Geocoding.Dto.Address;

namespace TQ.Geocoding.API.Test.Helpers.Dto
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class DtoHelper
    {
        public static SimpleAddressDto GetSimpleAddressDto(int count)
        {
            return new SimpleAddressDto
            {
                Uprn = count,
                SingleLineAddress = $"single line address {count}",
                AddressLine1 = $"AddressLine1 {count}",
                AddressLine2 = $"AddressLine2 {count}",
                AddressLine3 = $"AddressLine3 {count}",
                Postcode = $"Postcode {count}",
                Latitude = count,
                Longitude = count,
                Town = $"PostTown {count}",
                YCoordinateNorthing = count,
                XCoordinateEasting = count,
                Country = "E"
            };
        }

        public static SimpleWelshAddressDto GetSimpleWelshAddressDto(int count)
        {
            return new SimpleWelshAddressDto
            {
                Uprn = count,
                SingleLineAddress = $"WelshSingleLineAddress {count}",
                AddressLine1 = $"WelshAddressLine1 {count}",
                AddressLine2 = $"WelshAddressLine2 {count}",
                AddressLine3 = $"WelshAddressLine3 {count}",
                Postcode = $"Postcode {count}",
                Latitude = count,
                Longitude = count,
                PostTown = $"WelshPostTown {count}",
                YCoordinateNorthing = count,
                XCoordinateEasting = count,
                Country = "W"
            };
        }

        public static FullAddressDto GetFullAddressDto(int count)
        {
            return new FullAddressDto
            {
                AddressLine1 = $"AddressLine1 {count}",
                AddressLine2 = $"AddressLine2 {count}",
                AddressLine3 = $"AddressLine3 {count}",
                BuildingName = $"BuildingName {count}",
                BuildingNumber = count,
                DepartmentName = $"DepartmentName {count}",
                DependentLocality = $"DependentLocality {count}",
                DependentThoroughfare = $"DependentThoroughfare {count}",
                DoubleDependentLocality = $"DoubleDependentLocality {count}",
                Latitude = count,
                Longitude = count,
                OrganisationName = $"OrganisationName {count}",
                PoBoxNumber = $"PoBoxNumber {count}",
                Postcode = $"Postcode {count}",
                PostTown = $"PostTown {count}",
                SingleLineAddress = $"SingleLineAddress {count}",
                SubBuildingName = $"SubBuildingName {count}",
                Thoroughfare = $"ThoroughFare {count}",
                Uprn = count,
                WelshAddressLine1 = $"WelshAddressLine1 {count}",
                WelshAddressLine2 = $"WelshAddressLine2 {count}",
                WelshAddressLine3 = $"WelshAddressLine3 {count}",
                WelshDependentLocality = $"WelshDependentLocality {count}",
                WelshDependentThoroughfare = $"WelshDependentThoroughfare {count}",
                WelshDoubleDependentLocality = $"WelshDoubleDependentLocality {count}",
                WelshPostTown = $"WelshPostTown {count}",
                WelshSingleLineAddress = $"WelshSingleLineAddress {count}",
                WelshThoroughfare = $"WelshThoroughfare {count}",
                XCoordinateEasting = count,
                YCoordinateNorthing = count,
                Country = "E"
            };
        }

        public static FullWelshAddressDto GetFullWelshAddressDto(int count)
        {
            return new FullWelshAddressDto
            {
                BuildingName = $"BuildingName {count}",
                BuildingNumber = count,
                DepartmentName = $"DepartmentName {count}",
                Latitude = count,
                Longitude = count,
                OrganisationName = $"OrganisationName {count}",
                PoBoxNumber = $"PoBoxNumber {count}",
                Postcode = $"Postcode {count}",
                SubBuildingName = $"SubBuildingName {count}",
                Uprn = count,
                AddressLine1 = $"WelshAddressLine1 {count}",
                AddressLine2 = $"WelshAddressLine2 {count}",
                AddressLine3 = $"WelshAddressLine3 {count}",
                DependentLocality = $"WelshDependentLocality {count}",
                DependentThoroughfare = $"WelshDependentThoroughfare {count}",
                DoubleDependentLocality = $"WelshDoubleDependentLocality {count}",
                PostTown = $"WelshPostTown {count}",
                SingleLineAddress = $"WelshSingleLineAddress {count}",
                Thoroughfare = $"WelshThoroughfare {count}",
                XCoordinateEasting = count,
                YCoordinateNorthing = count,
                Country = "W"
            };
        }
    }
}
