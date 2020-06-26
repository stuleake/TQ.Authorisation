using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TQ.Geocoding.Data.Context.Addresses;

namespace TQ.Geocoding.Repository
{
    public class AddressReadOnlyRepository<TEntity> : IAddressReadOnlyRepository<TEntity> where TEntity : class
    {
        private readonly AddressDbContext dbContext;

        /// <summary>
        /// Creates a new instance of the <see cref="AddressReadOnlyRepository{TEntity}"/>class
        /// </summary>
        /// <param name="dbContext">the db context to use</param>
        public AddressReadOnlyRepository(AddressDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <inheritdoc/>
        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException($"{nameof(predicate)} is null");
            }

            return await this.dbContext.Set<TEntity>()
                .Where(predicate)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException($"{nameof(predicate)} is null");
            }

            return await this.dbContext.Set<TEntity>()
                .Where(predicate)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}