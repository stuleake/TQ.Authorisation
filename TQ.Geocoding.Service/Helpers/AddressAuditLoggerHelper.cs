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
    public class AddressAuditLoggerHelper : IAddressAuditLoggerHelper
    {
        private readonly ILogger<AddressAuditLoggerHelper> logger;
        private readonly IAuditLogger<AddressSearchAuditLog> auditLogger;

        /// <summary>
        /// Creates a new instance of the <see cref="AddressAuditLoggerHelper"/>
        /// </summary>
        /// <param name="logger">the logger to use</param>
        /// <param name="auditLogger">the audit logger to use</param>
        public AddressAuditLoggerHelper(ILogger<AddressAuditLoggerHelper> logger, IAuditLogger<AddressSearchAuditLog> auditLogger)
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
                await auditLogger.LogAsync(NewAddressSearchAuditLog(searchParameters, resultsCount));
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, $"Address audit log error.  Search parameters: {string.Join(";", searchParameters.Select(sp => sp.Key + "=" + sp.Value.ToString()))}");
            }
        }

        private static AddressSearchAuditLog NewAddressSearchAuditLog(ReadOnlyDictionary<string, object> searchParameters, int? resultCount)
        {
            return new AddressSearchAuditLog
            {
                Id = Guid.NewGuid(),
                LogDate = DateTime.Now,
                ResultCount = resultCount.HasValue ? resultCount.Value : 0,
                AddressSearch = string.Join(";", searchParameters.Select(sp => sp.Key + "=" + sp.Value.ToString()))
            };
        }
    }
}