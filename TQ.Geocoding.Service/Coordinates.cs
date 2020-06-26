using TQ.Geocoding.Service.Interface;

namespace TQ.Geocoding.Service
{
    public class Coordinates : ICoordinates
    {
        /// <inheritdoc/>
        public decimal MinX { get; set; }

        /// <inheritdoc/>
        public decimal MaxX { get; set; }

        /// <inheritdoc/>
        public decimal MinY { get; set; }

        /// <inheritdoc/>
        public decimal MaxY { get; set; }

        /// <inheritdoc/>
        public bool IsLongitudeLatitude { get; set; }
    }
}