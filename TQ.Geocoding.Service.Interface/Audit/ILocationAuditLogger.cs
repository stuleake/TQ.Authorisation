using System;
using System.Threading.Tasks;

namespace TQ.Geocoding.Service.Interface.Audit
{
    /// <summary>
    /// The interface for the <see cref="ILocationAuditLogger"/>
    /// </summary>
    public interface ILocationAuditLogger
    {
        /// <summary>
        /// Returns all of the audit log entries
        /// </summary>
        /// <returns>a <see cref="Task"/> that contains the result of the async operation</returns>
        Task GetAllAsync();

        /// <summary>
        /// Returns the audit log entries that match the supplied parameters
        /// </summary>
        /// <param name="searchText">the text to search for</param>
        /// <returns>a <see cref="Task"/> that contains the result of the async operation</returns>
        Task GetAllBySearchTextAsync(string searchText);

        /// <summary>
        /// Returns the audit log entries that match the supplied parameters
        /// </summary>
        /// <param name="startDate">the start date to search for</param>
        /// <param name="endDate">the end date to search for</param>
        /// <returns>a <see cref="Task"/> that contains the result of the async operation</returns>
        Task GetAllByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Returns the audit log entries that match the supplied parameters
        /// </summary>
        /// <param name="startCount">the start count to search for</param>
        /// <param name="endCount">the end count to search for</param>
        /// <returns>a <see cref="Task"/> that contains the result of the async operation</returns>
        Task GetAllByResultCountRangeAsync(int startCount, int endCount);
    }
}