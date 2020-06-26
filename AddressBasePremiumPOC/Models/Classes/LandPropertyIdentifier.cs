using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Geocoding.DataLoad.Models.Classes
{
    public class LandPropertyIdentifier
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
        [StringLength(14)]
        public string LpiKey { get; set; } // PRIMARY KEY
        [Required]
        [StringLength(3)]
        public string Language { get; set; }
        [Required]
        public short LogicalStatus { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        public DateTime LastUpdateDate { get; set; }
        [Required]
        public DateTime EntryDate { get; set; }
        public short? SaoStartNumber { get; set; } // ADP addressline & textsearchable
        [StringLength(2)]
        public string SaoStartSuffix { get; set; } // ADP addressline & textsearchable
        public short? SaoEndNumber { get; set; } // ADP addressline & textsearchable
        [StringLength(2)]
        public string SaoEndSuffix { get; set; } // ADP addressline & textsearchable
        [StringLength(90)]
        public string SaoText { get; set; } // ADP addressline & textsearchable
        public long? PaoStartNumber { get; set; } // ADP addressline & textsearchable
        [StringLength(2)]
        public string PaoStartSuffix { get; set; } // ADP addressline & textsearchable
        public long? PaoEndNumber { get; set; } // ADP addressline & textsearchable
        [StringLength(2)]
        public string PaoEndSuffix { get; set; } // ADP addressline & textsearchable
        [StringLength(90)]
        public string PaoText { get; set; } // ADP addressline & textsearchable
        [Required]
        public int Usrn { get; set; } // FOREIGN KEY
        [Required]
        public short UsrnMatchIndicator { get; set; }
        [StringLength(40)]
        public string AreaName { get; set; }
        [StringLength(30)]
        public string Level { get; set; }
        public Boolean? OfficialFlag { get; set; }
        [Required]
        public int LoadId { get; set; }
        public BasicLandAndPropertyUnit BasicLandAndPropertyUnit { get; set; }
        public Street Street { get; set; }
    }
}
