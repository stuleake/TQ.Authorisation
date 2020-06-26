namespace TQ.Geocoding.Model
{
    public interface ISearchableAddress
    {
        /// <summary>
        /// Gets or sets the id
        /// </summary>
        long Id { get; set; }

        /// <summary>
        /// Gets or sets the uprn
        /// </summary>
        long Uprn { get; set; }

        /// <summary>
        /// Gets or sets the organisation name
        /// </summary>
        string OrganisationName { get; set; }

        /// <summary>
        /// Gets or sets the department name
        /// </summary>
        string DepartmentName { get; set; }

        /// <summary>
        /// Gets or sets the sub building name
        /// </summary>
        string SubBuildingName { get; set; }

        /// <summary>
        /// Gets or sets the building name
        /// </summary>
        string BuildingName { get; set; }

        /// <summary>
        /// Gets or sets the building number
        /// </summary>
        int? BuildingNumber { get; set; }

        /// <summary>
        /// Gets or sets the PO box number
        /// </summary>
        string PoBoxNumber { get; set; }

        /// <summary>
        /// Gets or sets the dependent thoroughfare
        /// </summary>
        string DependentThoroughfare { get; set; }

        /// <summary>
        /// Gets or sets the thoroughfare
        /// </summary>
        string ThoroughFare { get; set; }

        /// <summary>
        /// Gets or sets the double dependent locality
        /// </summary>
        string DoubleDependentLocality { get; set; }

        /// <summary>
        /// Gets or sets the dependent locality
        /// </summary>
        string DependentLocality { get; set; }

        /// <summary>
        /// Gets or sets the post town
        /// </summary>
        string PostTown { get; set; }

        /// <summary>
        /// Gets or sets the post code
        /// </summary>
        string Postcode { get; set; }

        /// <summary>
        /// Gets or sets the postcode with no spaces
        /// </summary>
        string PostcodeNoSpaces { get; set; }

        /// <summary>
        /// Gets or sets the welsh dependent thoroughfare
        /// </summary>
        string WelshDependentThoroughfare { get; set; }

        /// <summary>
        /// Gets or sets the welsh thoroughfare
        /// </summary>
        string WelshThoroughfare { get; set; }

        /// <summary>
        /// Gets or sets the welsh double dependent locality
        /// </summary>
        string WelshDoubleDependentLocality { get; set; }

        /// <summary>
        /// Gets or sets the welsh dependent locality
        /// </summary>
        string WelshDependentLocality { get; set; }

        /// <summary>
        /// Gets or sets the welsh post town
        /// </summary>
        string WelshPostTown { get; set; }

        /// <summary>
        /// Gets or sets the x coordinate easting
        /// </summary>
        decimal XCoordinateEasting { get; set; }

        /// <summary>
        /// Gets or sets the y coordinate northing
        /// </summary>
        decimal YCoordinateNorthing { get; set; }

        /// <summary>
        /// Gets or sets the latitude
        /// </summary>
        decimal Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude
        /// </summary>
        decimal Longitude { get; set; }

        /// <summary>
        /// Gets or sets the load id
        /// </summary>
        int LoadId { get; set; }

        /// <summary>
        /// Gets or sets the address line1
        /// </summary>
        string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the address line2
        /// </summary>
        string AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the address line3
        /// </summary>
        string AddressLine3 { get; set; }

        /// <summary>
        /// Gets or sets the welsh address line1
        /// </summary>
        string WelshAddressLine1 { get; set; }


        /// <summary>
        /// Gets or sets the welsh address line2
        /// </summary>
        string WelshAddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the welsh address line3
        /// </summary>
        string WelshAddressLine3 { get; set; }

        /// <summary>
        /// Gets or sets the single line address
        /// </summary>
        string SingleLineAddress { get; set; }

        /// <summary>
        /// Gets or sets the welsh single line address
        /// </summary>
        string WelshSingleLineAddress { get; set; }
    }
}