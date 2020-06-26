using System.Threading.Tasks;

namespace TQ.Geocoding.Service.Interface.Audit
{
    /// <summary>
    /// The interface for the <see cref="IAuditLogger{TEntity}"/>
    /// </summary>
    /// <typeparam name="TEntity">the generic type <see cref="TEntity"/></typeparam>
    public interface IAuditLogger<TEntity> where TEntity : class
    {
        /// <summary>
        /// Logs the audit log entry
        /// </summary>
        /// <param name="entity">the generic entity <see cref="TEntity"/></param>
        /// <returns></returns>
        Task LogAsync(TEntity entity);
    }
}