using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TQ.Authentication.Data.Repositories
{
    /// <summary>
    /// Represents a base IRepository interface.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TPrimaryKey">The type of the primary key.</typeparam>
    public interface IRepository<TEntity, TPrimaryKey>
    {
        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>a <see cref="Task{TEntity}"/></returns>
        Task<TEntity> GetAsync(TPrimaryKey id);

        /// <summary>
        /// Determines whether [is any matched asynchronous] [the specified predicate].
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>a <see cref="Task{bool}"/></returns>
        Task<bool> IsAnyMatchedAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>a <see cref="Task{TPrimaryKey}"/></returns>
        Task<TPrimaryKey> CreateAsync(TEntity entity);
    }
}