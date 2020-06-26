using System;
using System.Linq;
using System.Linq.Expressions;
using TQ.Geocoding.Service.Extensions;
using TQ.Geocoding.Service.Interface;
using TQ.Geocoding.Service.Interface.Helpers;

namespace TQ.Geocoding.Service.Helpers
{
    public class PredicateHelper : IPredicateHelper
    {
        /// <inheritdoc/>
        public Expression<Func<ISearchableAddress, bool>> AddExpression<ISearchableAddress>(Expression<Func<ISearchableAddress, bool>> currentExpression, Expression<Func<ISearchableAddress, bool>> expressionToAdd)
        {
            return currentExpression == null ? expressionToAdd : currentExpression.AndAlso(expressionToAdd);
        }

        /// <inheritdoc/>
        public ISearchTerms SaveSearchStringAsSearchTerms(string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
            {
                throw new ArgumentNullException($"{nameof(searchString)} is null or empty");
            }

            SearchTerms searchTerms = new SearchTerms();
            searchString.Split(" ")
                .Select(s => s)
                .ToList()
                .ForEach(word => searchTerms = SaveWordAsSearchTerm(searchTerms, word));

            return searchTerms;
        }

        private SearchTerms SaveWordAsSearchTerm(SearchTerms searchTerms, string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return searchTerms;
            }

            if (word.IsNumber())
            {
                searchTerms.NumericSearchTerms.Add(GetNumericSearchTerm(word));
                return searchTerms;
            }

            searchTerms.StringSearchTerms = IncludeContainsSearchTerm(searchTerms.StringSearchTerms, word);
            return searchTerms;
        }

        /// <summary>
        /// Add Number to list of numeric search terms
        /// </summary>
        /// <param name="numericSearchTerms"></param>
        /// <param name="word"></param>
        /// <returns>a list of integers/returns>
        private int GetNumericSearchTerm(string word)
        {
            int numericSearchTerm;
            try
            {
                numericSearchTerm = Convert.ToInt32(word);
            }
            catch (OverflowException)
            {
                throw new ArgumentOutOfRangeException($"{nameof(word)} : {word} is not valid");
            }
            return numericSearchTerm;
        }

        /// <summary>
        /// Add String (plus " AND " as required) to string of search terms
        /// </summary>
        /// <param name="stringSearchTerms"></param>
        /// <param name="word"></param>
        /// <returns>string</returns>
        private string IncludeContainsSearchTerm(string stringSearchTerms, string word)
        {
            const string And = " AND ";
            const string Asterisk = "*";
            const string DoubleQuote = "\"";

            if (string.IsNullOrWhiteSpace(stringSearchTerms))
            {
                return DoubleQuote + word + Asterisk + DoubleQuote;
            }

            return stringSearchTerms += And + DoubleQuote + word + Asterisk + DoubleQuote;
        }
    }
}
