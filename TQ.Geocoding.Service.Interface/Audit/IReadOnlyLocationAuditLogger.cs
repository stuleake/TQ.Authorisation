using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.Geocoding.Dtos.Address;

namespace TQ.Geocoding.Service.Interface.Audit
{
    /// <summary>
    /// The interface for the <see cref="IReadOnlyAddressAuditLogger"/>
    /// </summary>
    public interface IReadOnlyLocationAuditLogger 
    {
        /// <summary>
        /// Returns all of the audit log entries
        /// </summary>
        /// <returns>a <see cref="Task"/> that contains the result of the async operation</returns>
        Task<IEnumerable<LocationSearchAuditLogDto>> GetAllAsync();

        /// <summary>
        /// Returns the audit log entries that match the supplied parameters
        /// </summary>
        /// <param name="searchText">the text to search for</param>
        /// <returns>a <see cref="Task"/> that contains the result of the async operation</returns>
        Task<IEnumerable<LocationSearchAuditLogDto>> GetAllBySearchTextAsync(string searchText);

        /// <summary>
        /// Returns the audit log entries that match the supplied parameters
        /// </summary>
        /// <param name="startDate">the start date to search for</param>
        /// <param name="endDate">the end date to search for</param>
        /// <returns>a <see cref="Task"/> that contains the result of the async operation</returns>
        Task<IEnumerable<LocationSearchAuditLogDto>> GetAllByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Returns the audit log entries that match the supplied parameters
        /// </summary>
        /// <param name="startCount">the start count to search for</param>
        /// <param name="endCount">the end count to search for</param>
        /// <returns>a <see cref="Task"/> that contains the result of the async operation</returns>
        Task<IEnumerable<LocationSearchAuditLogDto>> GetAllByResultCountRangeAsync(int startCount, int endCount);
    }
}