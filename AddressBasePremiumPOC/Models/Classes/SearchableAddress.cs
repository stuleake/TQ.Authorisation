using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Geocoding.DataLoad.Models.Classes
{
    public class SearchableAddress // Data from DeliveryPointAddress plus coords from BasicLandAndPropertyUnit
    {
        [Required]
        public long SearchableAddressId { get; set; }
        [Required]
        public long Uprn { get; set; } // FOREIGN KEY
        [StringLength(60)]
        public string OrganisationName { get; set; } // RQP address & address_no_spaces
        [StringLength(60)]
        public string DepartmentName { get; set; } // RQP address & address_no_spaces
        [StringLength(30)]
        public string SubBuildingName { get; set; } // RQP address & address_no_spaces
        [StringLength(50)]
        public string BuildingName { get; set; } // RQP address & address_no_spaces
        public int? BuildingNumber { get; set; } // RQP address & address_no_spaces
        [StringLength(6)]
        public string PoBoxNumber { get; set; } // RQP address & address_no_spaces
        [StringLength(80)]
        public string DependentThoroughfare { get; set; } // address & address_no_spaces
        [StringLength(80)]
        public string ThoroughFare { get; set; } // RQP address & address_no_spaces
        [StringLength(35)]
        public string DoubleDependentLocality { get; set; } // RQP address & address_no_spaces
        [StringLength(35)]
        public string DependentLocality { get; set; } // RQP address & address_no_spaces
        [StringLength(30)]
        public string PostTown { get; set; } // RQP address & address_no_spaces
        [StringLength(10)]
        public string Postcode { get; set; } // RQP postcode & postcode_no_spaces
        [StringLength(8)]
        public string PostcodeNoSpaces { get; set; } // RQP postcode_no_spaces
        [StringLength(80)]
        public string WelshDependentThoroughfare { get; set; }
        [StringLength(80)]
        public string WelshThoroughfare { get; set; }
        [StringLength(35)]
        public string WelshDoubleDependentLocality { get; set; }
        [StringLength(35)]
        public string WelshDependentLocality { get; set; }
        [StringLength(30)]
        public string WelshPostTown { get; set; }
        [Required]
        public float XCoordinateEasting { get; set; } // from BLPU
        [Required]
        public float YCoordinateNorthing { get; set; } // from BLPU
        [Required]
        public float Latitude { get; set; } // from BLPU
        [Required]
        public float Longitude { get; set; } // from BLPU
        [Required]
        public int LoadId { get; set; }
        [StringLength(250)]
        public string AddressLine1 { get; set; }
        [StringLength(250)]
        public string AddressLine2 { get; set; }
        [StringLength(250)]
        public string AddressLine3 { get; set; }
        [StringLength(250)]
        public string WelshAddressLine1 { get; set; }
        [StringLength(250)]
        public string WelshAddressLine2 { get; set; }
        [StringLength(250)]
        public string WelshAddressLine3 { get; set; }
        [StringLength(500)]
        public string SingleLineAddress { get; set; }
        [StringLength(500)]
        public string WelshSingleLineAddress { get; set; }
        public BasicLandAndPropertyUnit BasicLandAndPropertyUnit { get; set; }
    }
}
