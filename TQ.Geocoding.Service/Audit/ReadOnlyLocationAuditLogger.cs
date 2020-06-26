using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TQ.Geocoding.Dtos.Address;
using TQ.Geocoding.Models;
using TQ.Geocoding.Repository.Location;
using TQ.Geocoding.Service.Extensions;
using TQ.Geocoding.Service.Config;
using TQ.Geocoding.Service.Interface.Audit;
using TQ.Geocoding.Service.Interface.Converters;
using System.Linq;

namespace TQ.Geocoding.Service.Audit
{
    public class ReadOnlyLocationAuditLogger : IReadOnlyLocationAuditLogger
    {
        private readonly ILocationReadOnlyRepository<LocationSearchAuditLog> readOnlyRepository;
        private readonly ILocationAuditLogConverter locationAuditLogConverter;
        private readonly ConfigurationSettings configurationSettings;

        /// <summary>
        /// Creates a new instance of the <see cref="ReadOnlyLocationAuditLogger"/>
        /// </summary>
        /// <param name="readOnlyRepository">the read-only repository to use</param>
        /// <param name="locationAuditLogConverter">the location audit log converter to use</param>
        /// <param name="configurationSettings">the configuration settings to use</param>
        public ReadOnlyLocationAuditLogger(ILocationReadOnlyRepository<LocationSearchAuditLog> readOnlyRepository, 
                                          ILocationAuditLogConverter locationAuditLogConverter,
                                          IOptions<ConfigurationSettings> configurationSettings)
        {
            this.readOnlyRepository = readOnlyRepository;
            this.locationAuditLogConverter = locationAuditLogConverter;
            this.configurationSettings = configurationSettings.Value;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<LocationSearchAuditLogDto>> GetAllAsync()
        {
            var entities = await this.readOnlyRepository.ListAsync(configurationSettings.MaxRowCount);

            return this.locationAuditLogConverter.ToLocationSearchAuditLogDtos(entities.OrderBy(log => log.LogDate));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<LocationSearchAuditLogDto>> GetAllByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate == default)
            {
                throw new ArgumentException($"{nameof(startDate)} is not valid");
            }

            if (endDate == default)
            {
                throw new ArgumentException($"{nameof(endDate)} is not valid");
            }

            if (endDate < startDate)
            {
                throw new ArgumentException($"{nameof(startDate)} {startDate.ToShortDateString()} must be before {nameof(endDate)} {endDate.ToShortDateString()}");
            }

            var entities = await this.readOnlyRepository.ListAsync(auditLog => auditLog.LogDate >= startDate && auditLog.LogDate <= endDate.EndOfDay(), configurationSettings.MaxRowCount);

            return this.locationAuditLogConverter.ToLocationSearchAuditLogDtos(entities.OrderBy(log => log.LogDate));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<LocationSearchAuditLogDto>> GetAllByResultCountRangeAsync(int startCount, int endCount)
        {
            if (startCount < 0)
            {
                throw new ArgumentException($"{nameof(startCount)} must be greater than or equal to zero");
            }

            if (endCount < 0)
            {
                throw new ArgumentException($"{nameof(endCount)} must be greater than or equal to zero");
            }

            if (endCount < startCount)
            {
                throw new ArgumentException($"{nameof(startCount)} {startCount} must be less than {nameof(endCount)} {endCount}");
            }

            var entities = await this.readOnlyRepository.ListAsync(auditLog => auditLog.ResultCount >= startCount && auditLog.ResultCount <= endCount, configurationSettings.MaxRowCount);

            return this.locationAuditLogConverter.ToLocationSearchAuditLogDtos(entities.OrderBy(log => log.ResultCount));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<LocationSearchAuditLogDto>> GetAllBySearchTextAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                throw new ArgumentNullException($"{nameof(searchText)}");
            }

            var entities = await this.readOnlyRepository.ListAsync(auditLog => auditLog.LocationSearch.Contains(searchText), configurationSettings.MaxRowCount);

            return this.locationAuditLogConverter.ToLocationSearchAuditLogDtos(entities.OrderBy(log => log.LogDate));
        }
    }
}