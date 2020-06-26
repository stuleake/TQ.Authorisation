using System.Collections.Generic;

namespace TQ.Geocoding.Service.Interface
{
    public interface ISearchTerms
    {
        /// <summary>
        /// Gets or sets the string search terms
        /// </summary>
        string StringSearchTerms { get; set; }
        
        /// <summary>
        /// Gets or sets the numeric search terms
        /// </summary>
        List<int> NumericSearchTerms { get; set; }
    }
}
