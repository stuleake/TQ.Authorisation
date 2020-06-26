using System;
using System.Collections.Generic;
using System.Linq;

namespace TQ.Authentication.UnitTests.Validators
{
    public static class ValidatorHelpers
    {
        public static string GetMaxLengthString(int length)
        {
            return String.Concat(RandomSequence().Where(x => !char.IsControl(x)).Take(length));
        }

        private static IEnumerable<char> RandomSequence()
        {
            var random = new Random();
            while (true)
            {
                yield return (char)random.Next(char.MinValue, char.MaxValue);
            }
        }
    }
}