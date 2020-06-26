using System;
using System.Threading.Tasks;
using TQ.Geocoding.Repository;
using TQ.Geocoding.Service.Interface.Audit;

namespace TQ.Geocoding.Service.Audit
{
    public class AuditLogger<TEntity> : IAuditLogger<TEntity> where TEntity : class
    {
        private readonly IWriteOnlyRepository<TEntity> writeOnlyRepository;

        /// <summary>
        /// Creates a new instance of the <see cref="AuditLogger{TEntity}"/>
        /// </summary>
        /// <param name="writeOnlyRepository">the write-only repository to use</param>
        public AuditLogger(IWriteOnlyRepository<TEntity> writeOnlyRepository)
        {
            this.writeOnlyRepository = writeOnlyRepository;
        }

        /// <inheritdoc/>
        public async Task LogAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(entity)}");
            }

            await this.writeOnlyRepository.CreateAsync(entity);
        }
    }
}