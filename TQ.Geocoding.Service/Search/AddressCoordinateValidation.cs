using System;
using TQ.Geocoding.Service.Interface.Search;

namespace TQ.Geocoding.Service.Search
{
    public class AddressCoordinateValidation : IAddressCoordinateValidation
    {
        /// <inheritdoc/>
        public void ValidateCoordinates(decimal xCoordinate, decimal yCoordinate)
        {
            if (xCoordinate == default)
            {
                throw new ArgumentException($"{nameof(xCoordinate)} is not valid");
            }

            if (yCoordinate == default)
            {
                throw new ArgumentException($"{nameof(yCoordinate)} is not valid");
            }
        }

        /// <inheritdoc/>
        public void ValidateCoordinates(decimal xCoordinate, decimal yCoordinate, int radius)
        {
            this.ValidateCoordinates(xCoordinate, yCoordinate);

            if (radius == default)
            {
                throw new ArgumentException($"{nameof(radius)} is not valid");
            }
        }

        /// <inheritdoc/>
        public void ValidateRequest(object request)
        {
            if (request == null)
            {
                throw new ArgumentNullException($"{nameof(request)} is null");
            }
        }
    }
}