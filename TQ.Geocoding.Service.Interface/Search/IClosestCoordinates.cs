namespace TQ.Geocoding.Service.Interface.Search
{
    public interface IClosestCoordinates
    {
        /// <summary>
        /// Gets or sets the closest easting
        /// </summary>
        decimal ClosestEasting { get; set; }

        /// <summary>
        /// Gets or sets the closest northing
        /// </summary>
        decimal ClosestNorthing { get; set; }
    }
}