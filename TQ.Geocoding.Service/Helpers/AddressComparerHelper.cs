using System;
using System.Collections.Generic;
using System.Linq;
using TQ.Geocoding.Dto.Address;
using TQ.Geocoding.Service.Extensions;

namespace TQ.Geocoding.Service.Helpers
{
    public class AddressComparerHelper : IComparer<SimpleAddressDto>, IComparer<FullAddressDto>, IComparer<SimpleWelshAddressDto>, IComparer<FullWelshAddressDto>
    {
        private const StringComparison stringComparison = StringComparison.OrdinalIgnoreCase;
        private const string PrefixPushingNumbersToEnd = "ZZZ";
        private const string Comma = ",";
        private const char SpaceChar = ' ';
        private const char ZeroChar = '0';
        private const int StartOfString = 0;
        private const int DigitsToPadNumbersTo = 5;

        public int Compare(SimpleAddressDto thisAddress, SimpleAddressDto otherAddress)
        {
            return String.Compare(GetSortableAddress(thisAddress.SingleLineAddress), 
                                    GetSortableAddress(otherAddress.SingleLineAddress), 
                                    stringComparison);
        }

        public int Compare(FullAddressDto thisAddress, FullAddressDto otherAddress)
        {
            return String.Compare(GetSortableAddress(thisAddress.SingleLineAddress), 
                                    GetSortableAddress(otherAddress.SingleLineAddress), 
                                    stringComparison);
        }

        public int Compare(SimpleWelshAddressDto thisAddress, SimpleWelshAddressDto otherAddress)
        {
            return String.Compare(GetSortableAddress(thisAddress.SingleLineAddress),
                                    GetSortableAddress(otherAddress.SingleLineAddress),
                                    stringComparison);
        }

        public int Compare(FullWelshAddressDto thisAddress, FullWelshAddressDto otherAddress)
        {
            return String.Compare(GetSortableAddress(thisAddress.SingleLineAddress),
                                    GetSortableAddress(otherAddress.SingleLineAddress),
                                    stringComparison);
        }

        //GetSortableAddress based on the contents of the single-line address: 
        //Alphabetic first
        //Then numbers in ascending numeric order of the entire number (not digit by digit), where alphabetic suffixes to those numbers immediately follow
        //e.g. A, B, 1, 1A, 1B, 2, 3
        private string GetSortableAddress(string singleLineAddress)
        {
            var singleLineAddressWordsForSort = new List<string>();
            var numberSuffixesToAppend = string.Empty;

            foreach (string singleLineAddressWord in singleLineAddress.Replace(Comma, string.Empty).Split(SpaceChar))
            {
                // If word is a number, add prefix so it follows words and pad to equal length so numbers can be sorted by whole number not digit
                if (singleLineAddressWord.IsNumber())
                {
                    singleLineAddressWordsForSort.Add($"{PrefixPushingNumbersToEnd}{singleLineAddressWord.PadLeft(DigitsToPadNumbersTo, ZeroChar)}");
                }
                else
                {
                    // If not a number but starts with a number e.g. 1A, sort it to be after 1 and before 2 by moving the "number suffix" A to the end of the address line. 
                    // Exclude postcode incodes as no point turning e.g. 2PJ into ZZZ00002 and adding PJ to the end 
                    if (!singleLineAddress.EndsWith(singleLineAddressWord) && singleLineAddressWord.Substring(StartOfString, 1).IsNumber())
                    {
                        int numberLength = StartOfString;

                        while (singleLineAddressWord[numberLength].ToString().IsNumber())
                        {
                            numberLength++;
                        }

                        singleLineAddressWordsForSort.Add($"{PrefixPushingNumbersToEnd}{singleLineAddressWord.Substring(StartOfString, numberLength).PadLeft(DigitsToPadNumbersTo, ZeroChar)}");
                        numberSuffixesToAppend += singleLineAddressWord.Substring(numberLength);
                    }
                    else // neither a number, nor starts with a number (though could be a postcode incode)
                    {
                        singleLineAddressWordsForSort.Add(singleLineAddressWord);
                    }
                }
            }

            // Append any number suffixes found in this SingleLineAddress
            singleLineAddressWordsForSort.Add(numberSuffixesToAppend);

            return singleLineAddressWordsForSort.Aggregate((combinationOfWords, word) => combinationOfWords + word);
        }
    }
}
