using System.Collections.Generic;

namespace TQ.Geocoding.Service.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsNullEmptyOrDefault<T>(this T value)
        {
            if (value == null)
            {
                return true;
            }

            if (typeof(T) == typeof(string))
            {
                return string.IsNullOrWhiteSpace(value.ToString());
            }

            return EqualityComparer<T>.Default.Equals(value, default);
        }
    }
}