using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Geocoding.DataLoad.Models.Classes
{
    public class StagingFilter
    {
        [Required]
        public long StagingFilterId { get; set; }
        public long? Uprn { get; set; }
        public int? Usrn { get; set; }
        [StringLength(14)]
        public string LpiKey { get; set; }
        [StringLength(14)]
        public string XrefKey { get; set; }
        [StringLength(14)]
        public string ClassificationKey { get; set; }
        public long? Udprn { get; set; }
        [StringLength(14)]
        public string OrgKey { get; set; } // PRIMARY KEY
        public long? HeaderId { get; set; }
        public long? MetadataId { get; set; }
        public long? TrailerId { get; set; }
    }
}
