using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Geocoding.DataLoad.Models.Classes
{
    public class Header
    {
        [Required]
        public long HeaderId { get; set; }
        [Required]
        public short RecordIdentifier { get; set; }
        [Required]
        [StringLength(40)]
        public string CustodianName { get; set; }
        [Required]
        public int LocalCustodianCode { get; set; }
        [Required]
        public DateTime ProcessDate { get; set; }
        [Required]
        public short VolumeNumber { get; set; }
        [Required]
        public DateTime EntryDate { get; set; }
        [Required]
        public TimeSpan TimeStamp { get; set; } // TimeSpan gets converted to time in SQL 
        [Required]
        [StringLength(7)]
        public string Version { get; set; }
        [Required]
        [StringLength(1)]
        public string FileType { get; set; }
        [Required]
        public int LoadId { get; set; }
    }
}
