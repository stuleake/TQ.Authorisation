using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TQ.Geocoding.Models;
using TQ.Geocoding.Service.Interface.Audit;
using TQ.Geocoding.Service.Interface.Helpers;

namespace TQ.Geocoding.Service.Helpers
{
    public class LocationAuditLoggerHelper : ILocationAuditLoggerHelper
    {
        private readonly ILogger<LocationAuditLoggerHelper> logger;
        private readonly IAuditLogger<LocationSearchAuditLog> auditLogger;

        /// <summary>
        /// Creates a new instance of the <see cref="LocationAuditLoggerHelper"/>
        /// </summary>
        /// <param name="logger">the logger to use</param>
        /// <param name="auditLogger">the audit logger to use</param>
        public LocationAuditLoggerHelper(ILogger<LocationAuditLoggerHelper> logger, IAuditLogger<LocationSearchAuditLog> auditLogger)
        {
            this.logger = logger;
            this.auditLogger = auditLogger;
        }

        /// <inheritdoc/>
        public async Task LogAsync(ReadOnlyDictionary<string, object> searchParameters, int? resultsCount)
        {
            if (searchParameters?.Any() != true)
            {
                throw new ArgumentNullException($"{nameof(searchParameters)}");
            }

            try
            {
                await auditLogger.LogAsync(NewLocationSearchAuditLog(searchParameters, resultsCount));
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, $"Location audit log error.  Search parameters: {string.Join(";", searchParameters.Select(sp => sp.Key + "=" + sp.Value.ToString()))}");
            }
        }

        private static LocationSearchAuditLog NewLocationSearchAuditLog(ReadOnlyDictionary<string, object> searchParameters, int? resultCount)
        {
            return new LocationSearchAuditLog
            {
                Id = Guid.NewGuid(),
                LogDate = DateTime.Now,
                ResultCount = resultCount.HasValue ? resultCount.Value : 0,
                LocationSearch = string.Join(";", searchParameters.Select(sp => sp.Key + "=" + sp.Value.ToString()))
            };
        }
    }
}