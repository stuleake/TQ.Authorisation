using TQ.Geocoding.Service.Interface.Search;

namespace TQ.Geocoding.Service.Search
{
    public class ClosestCoordinates : IClosestCoordinates
    {
        /// <inheritdoc/>
        public decimal ClosestEasting { get; set; }

        /// <inheritdoc/>
        public decimal ClosestNorthing { get; set; }
    }
}