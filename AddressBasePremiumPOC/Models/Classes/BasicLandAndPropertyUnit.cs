using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Geocoding.DataLoad.Models.Classes
{
    public class BasicLandAndPropertyUnit
    {
        [Required]
        public short RecordIdentifier { get; set; }
        [Required]
        public char ChangeType { get; set; }
        [Required]
        public long ProOrder { get; set; }
        [Required]
        public long Uprn { get; set; } // PRIMARY KEY & ADP
        [Required]
        public long LogicalStatus { get; set; }
        [Required]
        public long BlpuState { get; set; }
        public DateTime? BlpuStateDate { get; set; }
        public long? ParentUprn { get; set; }
        [Required]
        public float XCoordinate { get; set; }
        [Required]
        public float YCoordinate { get; set; }
        [Required]
        public float Latitude { get; set; }
        [Required]
        public float Longitude { get; set; }
        [Required]
        public int Rpc { get; set; }
        [Required]
        public int LocalCustodianCode { get; set; }
        [Required]
        [StringLength(1)]
        public string Country { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        public DateTime LastUpdateDate { get; set; }
        [Required]
        public DateTime EntryDate { get; set; }
        [Required]
        [StringLength(1)]
        public string AddressBasePostal { get; set; }
        [Required]
        [StringLength(8)]
        public string PostcodeLocator { get; set; } // ADP addressline & textsearchable
        [Required]
        public int MultiOccCount { get; set; }
        [Required]
        public int LoadId { get; set; }
    }
}
