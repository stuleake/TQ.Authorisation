using System;
using TQ.Geocoding.Service.Interface;
using TQ.Geocoding.Service.Interface.Converters;

namespace TQ.Geocoding.Service.Converters
{
    /// <inheritdoc/>
    public class CoordinateConverter : ICoordinateConverter
    {
        /// <inheritdoc/>
        public ICoordinates ApplyRadiusToCoordinates(decimal xCoordinate, decimal yCoordinate, double radiusInMeters)
        {
            const int MaximumPossibleLongitudeOtherwiseMustBeAnEasting = 180;
            const double DegreeOfLongitudeInMeters = 111319.488D;
            const double DegreeOfLatitudeInMetersAtLatitude54Pt07469AsCentreOfUK = 111306.35256438493D; //  http://www.csgnetwork.com/degreelenllavcalc.html 

            var isLongitudeLatitude = xCoordinate <= MaximumPossibleLongitudeOtherwiseMustBeAnEasting;

            if (!isLongitudeLatitude || radiusInMeters == 0)
            {
                return new Coordinates
                {
                    MinX = xCoordinate - Convert.ToDecimal(radiusInMeters),
                    MaxX = xCoordinate + Convert.ToDecimal(radiusInMeters),
                    MinY = yCoordinate - Convert.ToDecimal(radiusInMeters),
                    MaxY = yCoordinate + Convert.ToDecimal(radiusInMeters),
                    IsLongitudeLatitude = isLongitudeLatitude
                };
            }

            var xCoordinateDouble = Convert.ToDouble(xCoordinate);
            var yCoordinateDouble = Convert.ToDouble(yCoordinate);
            double radiusInDegreesLat = radiusInMeters / DegreeOfLatitudeInMetersAtLatitude54Pt07469AsCentreOfUK;
            double radiusInDegreesLong = radiusInMeters / DegreeOfLongitudeInMeters;

            return new Coordinates
            {
                MinX = Convert.ToDecimal(xCoordinateDouble - radiusInDegreesLong / Math.Cos(yCoordinateDouble * (Math.PI / 180.0))),
                MaxX = Convert.ToDecimal(xCoordinateDouble + radiusInDegreesLong / Math.Cos(yCoordinateDouble * (Math.PI / 180.0))),
                MinY = Convert.ToDecimal(yCoordinateDouble - radiusInDegreesLat),
                MaxY = Convert.ToDecimal(yCoordinateDouble + radiusInDegreesLat),
                IsLongitudeLatitude = isLongitudeLatitude
            };
        }
    }
}