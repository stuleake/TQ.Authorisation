using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Geocoding.DataLoad.Models.Classes
{
    public class Trailer
    {
        [Required]
        public long TrailerId { get; set; }
        [Required]
        public short RecordIdentifier { get; set; }
        [Required]
        public short NextVolumeName { get; set; }
        [Required]
        public long RecordCount { get; set; }
        [Required]
        public DateTime EntryDate { get; set; }
        [Required]
        public TimeSpan TimeStamp { get; set; }
        [Required]
        public int LoadId { get; set; }
    }
}
