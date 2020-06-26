using System;
using System.Linq.Expressions;

namespace TQ.Geocoding.Service.Extensions
{
    public static class PredicateExtensions
    {
        /// <summary>
        /// AndAlso operator to combine expression with an existing one
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(
                    left.Body,
                    new ExpressionParameterReplacer(right.Parameters, left.Parameters).Visit(right.Body)),
                left.Parameters);
        }
    }
}
