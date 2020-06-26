namespace TQ.Geocoding.Service.Interface
{
    public interface ICoordinates
    {
        /// <summary>
        /// Gets or sets the min x coordinate
        /// </summary>
        decimal MinX { get; set; }

        /// <summary>
        /// Gets or sets the max y coordinate
        /// </summary>
        decimal MaxX { get; set; }

        /// <summary>
        /// Gets or sets the min y coordinate
        /// </summary>
        decimal MinY { get; set; }

        /// <summary>
        /// Gets or sets the max y coordinate
        /// </summary>
        decimal MaxY { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether IsLongitudeLatitude
        /// </summary>
        bool IsLongitudeLatitude { get; set; }
    }
}
