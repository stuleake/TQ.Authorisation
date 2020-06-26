using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Geocoding.DataLoad.Models.Classes
{
    public class ApplicationCrossReference
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
        public string XrefKey { get; set; } // PRIMARY KEY
        [Required]
        [StringLength(50)]
        public string CrossReference { get; set; }
        public short? Version { get; set; }
        [Required]
        [StringLength(6)]
        public string Source { get; set; }
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
