using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Geocoding.DataLoad.Models.Classes
{
    public class DeliveryPointAddress
    {
        [Required]
        public short RecordIdentifier { get; set; }
        [Required]
        public char ChangeType { get; set; }
        [Required]
        public long ProOrder { get; set; }
        [Required]
        public long Uprn { get; set; } // FOREIGN KEY
        [Required]
        public long Udprn { get; set; } // PRIMARY KEY
        [StringLength(60)]
        public string OrganisationName { get; set; } // RQP address_no_spaces
        [StringLength(60)]
        public string DepartmentName { get; set; } // RQP address_no_spaces
        [StringLength(30)]
        public string SubBuildingName { get; set; } // RQP address_no_spaces
        [StringLength(50)]
        public string BuildingName { get; set; } // RQP address_no_spaces
        public int? BuildingNumber { get; set; } // RQP address_no_spaces
        [StringLength(80)]
        public string DependentThoroughfare { get; set; } // RQP address_no_spaces
        [StringLength(80)]
        public string ThoroughFare { get; set; } // RQP address_no_spaces
        [StringLength(35)]
        public string DoubleDependentLocality { get; set; } // RQP address_no_spaces
        [StringLength(35)]
        public string DependentLocality { get; set; } // RQP address_no_spaces
        [StringLength(30)]
        public string PostTown { get; set; } // RQP address_no_spaces
        [StringLength(8)]
        public string Postcode { get; set; } // RQP postcode_no_spaces
        [StringLength(1)]
        public string PostcodeType { get; set; }
        [StringLength(2)]
        public string DeliveryPointSuffix { get; set; }
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
        [StringLength(6)]
        public string PoBoxNumber { get; set; } // RQP -- address_no_spaces
        [Required]
        public DateTime ProcessDate { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        public DateTime LastUpdateDate { get; set; }
        [Required]
        public DateTime EntryDate { get; set; }
        [Required]
        public int LoadId { get; set; }
        public BasicLandAndPropertyUnit BasicLandAndPropertyUnit { get; set; }
    }
}
