using System;
using System.Linq.Expressions;
using TQ.Geocoding.Models;

namespace TQ.Geocoding.Service.Interface.Builders
{
    public interface IPredicateBuilder
    {
        /// <summary>
        /// Returns a new predicate based up on the supplied search string
        /// </summary>
        /// <param name="searchString">the search string to use</param>
        /// <returns>an <see cref="Expression"/> that contains the new predicate></returns>
        public Expression<Func<SearchableAddress, bool>> BuildPredicate(string searchString);

        /// <summary>
        /// Returns a new welsh predicate based up on the supplied search string
        /// </summary>
        /// <param name="searchString">the search string to use</param>
        /// <returns>an <see cref="Expression"/> that contains the new predicate></returns>
        public Expression<Func<vwSearchableAddress, bool>> BuildWelshPredicate(string searchString);
    }
}