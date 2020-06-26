using System.Collections.Generic;
using TQ.Geocoding.Service.Interface;

namespace TQ.Geocoding.Service
{
    public class SearchTerms : ISearchTerms
    {
        /// <summary>
        /// Creates a new instance of the <see cref="SearchTerms"/>class
        /// </summary>
        public SearchTerms()
        {
            this.NumericSearchTerms = new List<int>();
        }

        /// <inheritdoc/>
        public string StringSearchTerms { get; set; }

        /// <inheritdoc/>
        public List<int> NumericSearchTerms { get; set; }
    }
}