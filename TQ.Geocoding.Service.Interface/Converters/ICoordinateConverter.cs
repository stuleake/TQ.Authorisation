namespace TQ.Geocoding.Service.Interface.Converters
{
    public interface ICoordinateConverter
    {
        /// <summary>
        /// Apply Radius To Coordinates to calculate minimum and maximum values
        /// </summary>
        /// <param name="xCoordinate">a value representing the x coordinate</param>
        /// <param name="yCoordinate">a value representing the y coordinate</param>
        /// <param name="maximumRadius">a value representing the maximum radius</param>
        /// <returns>A <see cref="ICoordinates"></see> that contains the longitude and latitude coordinates</returns>
        ICoordinates ApplyRadiusToCoordinates(decimal xCoordinate, decimal yCoordinate, double maximumRadius);
    }
}