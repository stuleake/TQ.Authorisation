using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TQ.Geocoding.Repository
{
    public interface IReadOnlyRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Returns the first model that matches the supplied predicate
        /// </summary>
        /// <param name="predicate">the predicate to apply</param>
        /// <returns>the model <see cref="{TEntity}"/> that matches the predicate or default if no match found</returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Returns a collection of models that match the supplied predicate
        /// </summary>
        /// <param name="predicate">the predicate to apply</param>
        /// <returns>a collection of models <see cref="{TEntity}"/> that match the predicate</returns>
        Task<IEnumerable<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate);
    }
}