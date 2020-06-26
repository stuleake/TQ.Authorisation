using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Geocoding.DataLoad.Models.Classes
{
    public class Metadata
    {
        [Required]
        public long MetadataId { get; set; }
        [Required]
        public short RecordIdentifier { get; set; }
        [Required]
        [StringLength(60)]
        public string GazName { get; set; }
        [Required]
        [StringLength(60)]
        public string GazScope { get; set; }
        [Required]
        [StringLength(60)]
        public string TerOfUse { get; set; }
        [Required]
        [StringLength(100)]
        public string LinkedData { get; set; }
        [Required]
        [StringLength(15)]
        public string GazOwner { get; set; }
        [Required]
        [StringLength(1)]
        public string NgazFreq { get; set; }
        [Required]
        [StringLength(40)]
        public string CustodianName { get; set; }
        [Required]
        public long CustodianUprn { get; set; }
        [Required]
        public short LocalCustodianCode { get; set; }
        [Required]
        [StringLength(40)]
        public string CoordSystem { get; set; }
        [Required]
        [StringLength(10)]
        public string CoordUnit { get; set; }
        [Required]
        public DateTime MetaDate { get; set; }
        [Required]
        [StringLength(60)]
        public string ClassScheme { get; set; }
        [Required]
        public DateTime GazDate { get; set; }
        [Required]
        [StringLength(3)]
        public string Language { get; set; }
        [Required]
        [StringLength(30)]
        public string CharacterSet { get; set; }
        [Required]
        public int LoadId { get; set; }
    }
}
