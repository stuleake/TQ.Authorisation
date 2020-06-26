using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TQ.Geocoding.Models;
using TQ.Geocoding.Service.Interface;
using TQ.Geocoding.Service.Interface.Builders;
using TQ.Geocoding.Service.Interface.Helpers;

namespace TQ.Geocoding.Service.Builders
{
    public class PredicateBuilder : IPredicateBuilder
    {
        private readonly IPredicateHelper predicateHelper;

        /// <summary>
        /// Creates a new instance of the <see cref="PredicateBuilder"/> class
        /// </summary>
        /// <param name="predicateHelper">the predicate helper to use</param>
        public PredicateBuilder(IPredicateHelper predicateHelper)
        {
            this.predicateHelper = predicateHelper;
        }

        /// <inheritdoc/>
        public Expression<Func<SearchableAddress, bool>> BuildPredicate(string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
            {
                throw new ArgumentNullException($"{nameof(searchString)} is null or empty");
            }

            DbFunctions dbFunctions = null;
            Expression<Func<SearchableAddress, bool>> searchExpression = null;
            ISearchTerms searchTerms = predicateHelper.SaveSearchStringAsSearchTerms(searchString);

            if (!string.IsNullOrWhiteSpace(searchTerms.StringSearchTerms))
            {
                searchExpression = predicateHelper.AddExpression(searchExpression, a => dbFunctions.Contains(a.SingleLineAddress, searchTerms.StringSearchTerms));
            }

            if (searchTerms.NumericSearchTerms?.Any() ?? false)
            {
                searchTerms.NumericSearchTerms.Select(s => s).ToList().ForEach(searchTerm => searchExpression = predicateHelper.AddExpression(searchExpression, a => dbFunctions.Like(a.SingleLineAddress, string.Format("%{0}%", searchTerm))));
            }

            return searchExpression;
        }

        /// <inheritdoc/>
        public Expression<Func<vwSearchableAddress, bool>> BuildWelshPredicate(string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
            {
                throw new ArgumentNullException($"{nameof(searchString)} is null or empty");
            }

            DbFunctions dbFunctions = null;
            Expression<Func<vwSearchableAddress, bool>> searchExpression = null;
            ISearchTerms searchTerms = predicateHelper.SaveSearchStringAsSearchTerms(searchString);

            if (!string.IsNullOrWhiteSpace(searchTerms.StringSearchTerms))
            {
                searchExpression = predicateHelper.AddExpression(searchExpression, a => dbFunctions.Contains(a.WelshSingleLineAddress, searchTerms.StringSearchTerms));
            }

            if (searchTerms.NumericSearchTerms?.Any() ?? false)
            {
                searchTerms.NumericSearchTerms.Select(s => s).ToList().ForEach(searchTerm => searchExpression = predicateHelper.AddExpression(searchExpression, a => dbFunctions.Like(a.WelshSingleLineAddress, string.Format("%{0}%", searchTerm))));
            }

            return searchExpression;
        }
    }
}