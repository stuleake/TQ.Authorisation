using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Geocoding.DataLoad.Models.Classes
{
    public class StreetDescriptor
    {
        [Required]
        public short RecordIdentifier { get; set; }
        [Required]
        public char ChangeType { get; set; }
        [Required]
        public long ProOrder { get; set; }
        [Required]
        public int Usrn { get; set; } // FOREIGN KEY
        [Required]
        [StringLength(100)]
        public string StreetDescription { get; set; } // ADP addressline & textsearchable
        [StringLength(35)]
        public string Locality { get; set; } // ADP addressline & textsearchable
        [Required]
        [StringLength(30)]
        public string TownName { get; set; } // ADP addressline & textsearchable
        [Required]
        [StringLength(30)]
        public string AdminstrativeArea { get; set; } // ADP textsearchable
        [Required]
        [StringLength(3)]
        public string Language { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        public DateTime LastUpdateDate { get; set; }
        [Required]
        public DateTime EntryDate { get; set; }
        [Required]
        public int LoadId { get; set; }

        public Street Street { get; set; }
    }
}
