using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Geocoding.DataLoad.Models.Classes
{
    public class SearchableGeographic
    // Data from LandPropertyIdentifier, 
    // Uprn and PostcodeLocator from BasicLandAndPropertyUnit, 
    // StreetDescription, Locality, TownName from StreetDescriptor 
    {
        [Required]
        public long SearchableGeographicId { get; set; }
        [Required]
        public long Uprn { get; set; } // from BLPU - FOREIGN KEY
        [StringLength(90)]
        public string SaoText { get; set; } // Secondary Addressable Object - ADP addressline & textsearchable
        public short? SaoStartNumber { get; set; } // ADP addressline & textsearchable
        [StringLength(2)]
        public string SaoStartSuffix { get; set; } // ADP addressline & textsearchable
        public short? SaoEndNumber { get; set; } // ADP addressline & textsearchable
        [StringLength(2)]
        public string SaoEndSuffix { get; set; } // ADP addressline & textsearchable
        [StringLength(90)]
        public string PaoText { get; set; } // Primary Addressable Object - ADP addressline & textsearchable
        public short? PaoStartNumber { get; set; } // ADP addressline & textsearchable
        [StringLength(2)]
        public string PaoStartSuffix { get; set; } // ADP addressline & textsearchable
        public short? PaoEndNumber { get; set; } // ADP addressline & textsearchable
        [StringLength(2)]
        public string PaoEndSuffix { get; set; } // ADP addressline & textsearchable
        [StringLength(100)]
        public string StreetDescription { get; set; } // from StreetDescriptor - ADP addressline & textsearchable
        [StringLength(35)]
        public string Locality { get; set; } // from StreetDescriptor - ADP addressline & textsearchable
        [StringLength(30)]
        public string TownName { get; set; } // from StreetDescriptor- ADP addressline & textsearchable
        [StringLength(30)]
        public string AdministrativeArea { get; set; } // ADP textsearchable
        [StringLength(8)]
        public string PostcodeLocator { get; set; } // from BLPU - ADP addressline & textsearchable
        [Required]
        public int LoadId { get; set; }
        public BasicLandAndPropertyUnit BasicLandAndPropertyUnit { get; set; }
    }
}
