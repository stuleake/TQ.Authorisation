using System;

namespace TQ.Geocoding.Service.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// A DateTime extension method that return a DateTime with the time set to "23:59:59:999". 
        /// The last moment of the day.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A DateTime of the day with the time set to "23:59:59:999".</returns>
        public static DateTime EndOfDay(this DateTime @this)
        {
            return @this.AddDays(1).Subtract(new TimeSpan(0, 0, 0, 0, 1));
        }
    }
}