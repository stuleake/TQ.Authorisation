namespace TQ.Geocoding.Service.Interface.Search
{
    public interface IAddressCoordinateValidation
    {
        /// <summary>
        /// Validates that the XCoordinate and YCoordinate parameters have a value other than default.
        /// Throws an ArgumentException if either value is default
        /// </summary>
        /// <param name="xCoordinate">the XCoordinate to validate</param>
        /// <param name="yCoordinate">the YCoordinate to validate</param>
        void ValidateCoordinates(decimal xCoordinate, decimal yCoordinate);

        /// <summary>
        /// Validates that the XCoordinate, YCoordinate and Radius parameters have a value other than default.
        /// Throws an ArgumentException if either any values are default
        /// </summary>
        /// <param name="xCoordinate">the XCoordinate to validate</param>
        /// <param name="yCoordinate">the YCoordinate to validate</param>
        /// <param name="radius">the radius to validate</param>
        void ValidateCoordinates(decimal xCoordinate, decimal yCoordinate, int radius);
        
        /// <summary>
        /// Validate that the request is not null
        /// Throws an ArgumentNullException if the request is null
        /// </summary>
        /// <param name="request">the request to validate</param>
        void ValidateRequest(object request);
    }
}