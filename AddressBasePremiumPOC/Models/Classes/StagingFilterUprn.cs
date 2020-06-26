using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Geocoding.DataLoad.Models.Classes
{
    public class StagingFilterUprn
    {
        [Required]
        public long FilterUprn { get; set; }
    }
}
