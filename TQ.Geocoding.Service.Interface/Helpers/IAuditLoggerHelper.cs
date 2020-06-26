using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace TQ.Geocoding.Service.Interface.Helpers
{
    /// <summary>
    /// Interface for the <see cref="IAuditLoggerHelper"/>
    /// </summary>
    public interface IAuditLoggerHelper
    {
        /// <summary>
        /// Logs the audit log item
        /// </summary>
        /// <param name="searchParameters">the search parameters</param>
        /// <param name="resultsCount">the result count</param>
        /// <returns>a <see cref="Task"/> that represents the outcome of the async operation</returns>
        Task LogAsync(ReadOnlyDictionary<string, object> searchParameters, int? resultsCount);
    }
}