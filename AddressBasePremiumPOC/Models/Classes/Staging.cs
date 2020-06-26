using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Geocoding.DataLoad.Models.Classes
{
    public class Staging
    {
        [Required]
        [StringLength(2)]
        public string RecordIdentifier { get; set; }
        [StringLength(100)]
        public string Field1 { get; set; }
        [StringLength(100)]
        public string Field2 { get; set; }
        [StringLength(100)]
        public string Field3 { get; set; }
        [StringLength(100)]
        public string Field4 { get; set; }
        [StringLength(100)]
        public string Field5 { get; set; }
        [StringLength(100)]
        public string Field6 { get; set; }
        [StringLength(100)]
        public string Field7 { get; set; }
        [StringLength(100)]
        public string Field8 { get; set; }
        [StringLength(100)]
        public string Field9 { get; set; }
        [StringLength(100)]
        public string Field10 { get; set; }
        [StringLength(100)]
        public string Field11 { get; set; }
        [StringLength(100)]
        public string Field12 { get; set; }
        [StringLength(100)]
        public string Field13 { get; set; }
        [StringLength(100)]
        public string Field14 { get; set; }
        [StringLength(100)]
        public string Field15 { get; set; }
        [StringLength(100)]
        public string Field16 { get; set; }
        [StringLength(100)]
        public string Field17 { get; set; }
        [StringLength(100)]
        public string Field18 { get; set; }
        [StringLength(100)]
        public string Field19 { get; set; }
        [StringLength(100)]
        public string Field20 { get; set; }
        [StringLength(100)]
        public string Field21 { get; set; }
        [StringLength(100)]
        public string Field22 { get; set; }
        [StringLength(100)]
        public string Field23 { get; set; }
        [StringLength(100)]
        public string Field24 { get; set; }
        [StringLength(100)]
        public string Field25 { get; set; }
        [StringLength(100)]
        public string Field26 { get; set; }
        [StringLength(100)]
        public string Field27 { get; set; }
        [StringLength(100)]
        public string Field28 { get; set; }
        [Required]
        public long StagingRecordId { get; set; }
    }
}
