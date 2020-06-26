using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using TQ.Geocoding.DataLoad.Models.Enums;

namespace TQ.Geocoding.DataLoad.Models.Classes
{
    public class Street
    {
        [Required]
        public short RecordIdentifier { get; set; }
        [Required]
        public char ChangeType { get; set; }
        [Required]
        public long ProOrder { get; set; } // processing order
        [Required]
        public int Usrn { get; set; } // PRIMARY KEY Unique Street Reference Number
        [Required]
        public short RecordType { get; set; } // Street Record Type Code
        [Required]
        public int SwaOrgRefNaming { get; set; } // code identifying Street Naming and Numbering Authority or Local Highway Authority
        public short? State { get; set; } // state of the street e.g. Open
        public DateTime? StateDate { get; set; } // date current state achieved
        public short? StreetSurface { get; set; } // Street Surface Code
        public short? StreetClassification { get; set; } // Street Classification Code
        [Required]
        public short Version { get; set; } // version of street record
        [Required]
        public DateTime StreetStartDate { get; set; } // date current version started
        public DateTime? StreetEndDate { get; set; } // date street closed
        [Required]
        public DateTime LastUpdateDate { get; set; }
        [Required]
        public DateTime RecordEntryDate { get; set; } // field was missed
        [Required]
        public float StreetStartX { get; set; }
        [Required]
        public float StreetStartY { get; set; }
        [Required]
        public float StreetStartLat { get; set; }
        [Required]
        public float StreetStartLong { get; set; }
        [Required]
        public float StreetEndX { get; set; }
        [Required]
        public float StreetEndY { get; set; }
        [Required]
        public float StreetEndLat { get; set; }
        [Required]
        public float StreetEndLong { get; set; }
        [Required]
        public short StreetTolerance { get; set; }
        [Required]
        public int LoadId { get; set; }
    }
}
