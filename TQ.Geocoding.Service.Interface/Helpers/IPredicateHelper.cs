using System;
using System.Linq.Expressions;

namespace TQ.Geocoding.Service.Interface.Helpers
{
    public interface IPredicateHelper
    {
        /// <summary>
        /// Adds a new expression to an existing expression
        /// </summary>
        /// <typeparam name="ISearchableAddress">the searchable address</typeparam>
        /// <param name="currentExpression">the current expression</param>
        /// <param name="expressionToAdd">the expression to add to the current expression</param>
        /// <returns>an <see cref="Expression"/> that contains the new expression></returns>
        public Expression<Func<ISearchableAddress, bool>> AddExpression<ISearchableAddress>(Expression<Func<ISearchableAddress, bool>> currentExpression, Expression<Func<ISearchableAddress, bool>> expressionToAdd);
        
        
        /// <summary>
        /// Returns a new SearchTerm based on the supplied search string
        /// </summary>
        /// <param name="searchString">the search string</param>
        /// <returns>a new <see cref="ISearchTerms"/></returns>
        public ISearchTerms SaveSearchStringAsSearchTerms(string searchString);
    }
}