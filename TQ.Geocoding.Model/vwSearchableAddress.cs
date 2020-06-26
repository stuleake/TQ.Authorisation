using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Geocoding.Model
{
    /// <summary>
    /// EF model class for the vwSearchableAddress table
    /// </summary>
    [Table("vwSearchableAddress")]
    public class vwSearchableAddress : ISearchableAddress
    {
        /// <summary>
        /// Gets or sets the primary key
        /// </summary>
        [Key]
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the Uprn
        /// </summary>
        [Required]
        public long Uprn { get; set; }

        /// <summary>
        /// Gets or sets the organisation name
        /// </summary>
        [StringLength(60)]
        public string OrganisationName { get; set; }

        /// <summary>
        /// Gets or sets the department name
        /// </summary>
        [StringLength(60)]
        public string DepartmentName { get; set; }

        /// <summary>
        /// Gets or sets the sub building name
        /// </summary>
        [StringLength(30)]
        public string SubBuildingName { get; set; }

        /// <summary>
        /// Gets or sets the building name
        /// </summary>
        [StringLength(50)]
        public string BuildingName { get; set; }

        /// <summary>
        /// Gets or sets the building number
        /// </summary>
        public int? BuildingNumber { get; set; }

        /// <summary>
        /// Gets or sets the PO-box number
        /// </summary>
        [StringLength(6)]
        public string PoBoxNumber { get; set; }

        /// <summary>
        /// Gets or sets the dependent thoroughfare
        /// </summary>
        [StringLength(80)]
        public string DependentThoroughfare { get; set; }

        /// <summary>
        /// Gets or sets the ThoroughFare
        /// </summary>
        [StringLength(80)]
        public string ThoroughFare { get; set; }

        /// <summary>
        /// Gets or sets the double dependent locality
        /// </summary>
        [StringLength(35)]
        public string DoubleDependentLocality { get; set; }

        /// <summary>
        /// Gets or sets the dependent locality
        /// </summary>
        [StringLength(35)]
        public string DependentLocality { get; set; }

        /// <summary>
        /// Gets or sets the post town
        /// </summary>
        [StringLength(30)]
        public string PostTown { get; set; }

        /// <summary>
        /// Gets or sets the post code
        /// </summary>
        [StringLength(10)]
        public string Postcode { get; set; }

        /// <summary>
        /// Gets or sets the post code with no spaces
        /// </summary>
        [StringLength(8)]
        public string PostcodeNoSpaces { get; set; }

        /// <summary>
        /// Gets or sets the welsh dependent thoroughfare
        /// </summary>
        [StringLength(80)]
        public string WelshDependentThoroughfare { get; set; }

        /// <summary>
        /// Gets or sets the welsh thoroughfare
        /// </summary>
        [StringLength(80)]
        public string WelshThoroughfare { get; set; }

        /// <summary>
        /// Gets or sets the welsh double dependent locality
        /// </summary>
        [StringLength(35)]
        public string WelshDoubleDependentLocality { get; set; }

        /// <summary>
        /// Gets or sets the welsh dependent locality
        /// </summary>
        [StringLength(35)]
        public string WelshDependentLocality { get; set; }

        /// <summary>
        /// Gets or sets the welsh post town
        /// </summary>
        [StringLength(30)]
        public string WelshPostTown { get; set; }

        /// <summary>
        /// Gets or sets the XCoordinateEasting
        /// </summary>
        [Required]
        public decimal XCoordinateEasting { get; set; }

        /// <summary>
        /// Gets or sets the YCoordinateNorthing
        /// </summary>
        [Required]
        public decimal YCoordinateNorthing { get; set; }

        /// <summary>
        /// Gets or sets the latitude
        /// </summary>
        [Required]
        public decimal Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude
        /// </summary>
        [Required]
        public decimal Longitude { get; set; }

        /// <summary>
        /// Gets or sets the LoadId
        /// </summary>
        [Required]
        public int LoadId { get; set; }

        /// <summary>
        /// Gets or sets the address line 1
        /// </summary>
        [StringLength(250)]
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the address line 2
        /// </summary>
        [StringLength(250)]
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the address line 3
        /// </summary>
        [StringLength(250)]
        public string AddressLine3 { get; set; }

        /// <summary>
        /// Gets or sets the welsh address line 1
        /// </summary>
        [StringLength(250)]
        public string WelshAddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the welsh address line 2
        /// </summary>
        [StringLength(250)]
        public string WelshAddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the welsh address line 3
        /// </summary>
        [StringLength(250)]
        public string WelshAddressLine3 { get; set; }

        /// <summary>
        /// Gets or sets the single line address
        /// </summary>
        [StringLength(500)]
        public string SingleLineAddress { get; set; }

        /// <summary>
        /// Gets or sets the welsh single line address
        /// </summary>
        [StringLength(500)]
        public string WelshSingleLineAddress { get; set; }
    }
}