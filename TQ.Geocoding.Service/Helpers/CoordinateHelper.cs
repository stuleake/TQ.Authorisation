using System;
using System.Collections.Generic;
using TQ.Geocoding.Models;
using TQ.Geocoding.Service.Interface;
using TQ.Geocoding.Service.Interface.Helpers;
using TQ.Geocoding.Service.Interface.Search;
using TQ.Geocoding.Service.Search;

namespace TQ.Geocoding.Service.Helpers
{
    public class CoordinateHelper : ICoordinateHelper
    {
        /// <inheritdoc/>
        public ICoordinates GetMaxCoordinates(ICoordinates coordinates)
        {
            coordinates.MaxX = GetMaxCoordinate(coordinates.MinX, coordinates.IsLongitudeLatitude);
            coordinates.MaxY = GetMaxCoordinate(coordinates.MinY, coordinates.IsLongitudeLatitude);

            return coordinates;
        }

        /// <inheritdoc/>
        public IClosestCoordinates GetClosestCoordinates(IEnumerable<SearchableAddress> searchableAddresses, decimal xCoordinate, decimal yCoordinate, bool isLongitudeLatitude)
        {
            decimal closestDistance = 100000;
            decimal closestEasting = 0;
            decimal closestNorthing = 0;

            foreach (var address in searchableAddresses)
            {
                try
                {
                    // a^2 + b^2 = c^2
                    double deltaX = 0;
                    double deltaY = 0;

                    if (isLongitudeLatitude)
                    {
                        deltaX = Math.Abs(Convert.ToDouble(xCoordinate) - Convert.ToDouble(address.Longitude));
                        deltaY = Math.Abs(Convert.ToDouble(yCoordinate) - Convert.ToDouble(address.Latitude));
                    }
                    else
                    {
                        deltaX = Math.Abs(Convert.ToDouble(xCoordinate) - Convert.ToDouble(address.XCoordinateEasting));
                        deltaY = Math.Abs(Convert.ToDouble(yCoordinate) - Convert.ToDouble(address.YCoordinateNorthing));
                    }

                    var distance = Convert.ToDecimal(Math.Sqrt(deltaX * deltaX + deltaY * deltaY));
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEasting = address.XCoordinateEasting;
                        closestNorthing = address.YCoordinateNorthing;
                    }
                }
                catch (InvalidCastException)
                {
                    // TODO placeholder for logging this when logging added back to the program
                    throw;
                }
            }

            return new ClosestCoordinates
            {
                ClosestEasting = closestEasting,
                ClosestNorthing = closestNorthing
            };
        }

        private decimal GetMaxCoordinate(decimal coordinateValue, bool isLongitudeLatitude)
        {
            const int LongitudeLatitudeMaxDecimalDigits = 7; // e.g. -3.6893709 or 51.4872072
            const int EastingNorthingMaxDecimalDigits = 2; // e.g. 282801.52 or 177826.54
            const string DecimalPoint = ".";
            const string ExtraDigitToCreateRange = "9";
            const string DecimalDigitFiller = "9999999";

            string coordinateValueAsString = coordinateValue.ToString();
            int numberDecimalDigits = coordinateValueAsString.IndexOf(DecimalPoint) > -1 ?
                    coordinateValueAsString.Substring(coordinateValueAsString.IndexOf(DecimalPoint) + 1).Length : 0;
            int maxDecimalDigits = isLongitudeLatitude ? LongitudeLatitudeMaxDecimalDigits : EastingNorthingMaxDecimalDigits;

            // User already supplied all digits
            if (numberDecimalDigits == maxDecimalDigits)
            {
                return Convert.ToDecimal(coordinateValueAsString + ExtraDigitToCreateRange); // add an extra digit beyond those populated in the database to simplify searching to always use a range
            }

            // Too many decimal digits supplied e.g. 51.4872072333 when the max is 7 e.g. 51.4872072 - truncate excess digits and replace with a 9
            if (numberDecimalDigits > maxDecimalDigits)
            {
                return Convert.ToDecimal(coordinateValueAsString.Substring(0, coordinateValueAsString.IndexOf(DecimalPoint) + 1)
                                        + coordinateValueAsString.Substring(coordinateValueAsString.IndexOf(DecimalPoint) + 1, maxDecimalDigits)
                                        + ExtraDigitToCreateRange); // add an extra digit beyond those populated in the database to simplify searching to always use a range
            }

            if (numberDecimalDigits == 0)
            {
                coordinateValueAsString += ".";
            }

            // Fewer decimal digits supplied than the maximum e.g. 282801 or 282801.5 instead of 282801.52 - return 282801.99 or 282801.59
            return Convert.ToDecimal(coordinateValueAsString + DecimalDigitFiller.Substring(0, maxDecimalDigits - numberDecimalDigits));
        }
    }
}