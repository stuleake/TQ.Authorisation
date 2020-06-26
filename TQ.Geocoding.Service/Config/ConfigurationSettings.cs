using System.ComponentModel.DataAnnotations;

namespace TQ.Geocoding.Service.Config
{
    public class ConfigurationSettings
    {
        /// <summary>
        /// Gets or sets the MaxRowCount setting
        /// </summary>
        [Range(1, 100, ErrorMessage = "Max Row Count must be a value between 1 and 100")]
        public int MaxRowCount { get; set; }
    }
}