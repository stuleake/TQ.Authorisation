using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace TQ.Geocoding.Service.Extensions
{
    public static class StringExtensions
    {
        private const int MinPostCodeLength = 4;
        private const int MaxPostCodeLength = 8;
        private const string SingleSpace = " ";
        private const int InwardCodeLength = 3;

        /// <summary>
        /// Checks that the search string is a valid UK postcode  
        /// </summary>
        /// <param name="searchString">the string to check</param>
        /// <returns>true if the input is a valid postcode, false otherwise</returns>
        public static bool IsPostCode(this string searchString)
        {
            var searchStringWithoutSpaces = searchString.Replace(SingleSpace, string.Empty);

            if (searchStringWithoutSpaces.ToCharArray().Any(c => !char.IsLetter(c) && !char.IsNumber(c)))
            {
                return false;
            }

            return IsValidPostCodeWithoutSpace(searchStringWithoutSpaces);
        }

        /// <summary>
        /// Convert postcode to standard format including space
        /// </summary>
        /// <param name="postcode"></param>
        /// <returns>Reformatted postcode</returns>
        public static string ToPostcodeFormat(this string postcode)
        {
            if (!postcode.Contains(SingleSpace))
            {
                return postcode.Substring(0, postcode.Length - InwardCodeLength) + SingleSpace + postcode.Substring(postcode.Length - InwardCodeLength);
            }

            return postcode;
        }

        /// <summary>
        /// Converts the input into a post code formatted string
        /// </summary>
        /// <param name="searchString">the string to search for</param>
        /// <returns>a formatted postcode</returns>
        public static string ToPostCode(this string searchString)
        {
            var newPostCode = searchString;
            if (searchString.Contains(SingleSpace) || searchString.Length <= MinPostCodeLength || searchString.Length >= MaxPostCodeLength)
            {
                return newPostCode;
            }
            var outCode = newPostCode.Substring(newPostCode.Length - 3);
            newPostCode = newPostCode.Replace(outCode, " " + outCode);

            return newPostCode;
        }

        /// <summary>
        /// IsNumber to determine if a string only contains numbers
        /// </summary>
        /// <param name="word">the string to check</param>
        /// <returns>true if the word is a number, false otherwise</returns>
        public static bool IsNumber(this string word)
        {
            return word.ToCharArray().All(c => char.IsDigit(c));
        }

        /// <summary>
        /// Checks that the search string is a valid UK postcode but without the space
        /// </summary>
        /// <param name="searchString">the string to check</param>
        /// <returns>true if the input is a valid postcode but without the space, false otherwise</returns>
        private static bool IsValidPostCodeWithoutSpace(this string searchString)
        {
            return Regex.IsMatch(searchString, "^([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]))))[0-9][A-Za-z]{2})$");
        }
    }
}