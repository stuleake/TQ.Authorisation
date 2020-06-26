using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TQ.Geocoding.Dtos.Address;
using TQ.Geocoding.Models;
using TQ.Geocoding.Repository.Address;
using TQ.Geocoding.Service.Config;
using TQ.Geocoding.Service.Extensions;
using TQ.Geocoding.Service.Interface.Audit;
using TQ.Geocoding.Service.Interface.Converters;

namespace TQ.Geocoding.Service.Audit
{
    public class ReadOnlyAddressAuditLogger : IReadOnlyAddressAuditLogger
    {
        private readonly IAddressReadOnlyRepository<AddressSearchAuditLog> readOnlyRepository;
        private readonly IAddressAuditLogConverter addressAuditLogConverter;
        private readonly ConfigurationSettings configurationSettings;

        /// <summary>
        /// Creates a new instance of the <see cref="ReadOnlyAddressAuditLogger"/>
        /// </summary>
        /// <param name="readOnlyRepository">the read-only repository to use</param>
        /// <param name="addressAuditLogConverter">the address audit log converter to use</param>
        /// <param name="configurationSettings">the configuration settings to use</param> 
        public ReadOnlyAddressAuditLogger(IAddressReadOnlyRepository<AddressSearchAuditLog> readOnlyRepository, 
                                         IAddressAuditLogConverter addressAuditLogConverter,
                                         IOptions<ConfigurationSettings> configurationSettings)
        {
            this.readOnlyRepository = readOnlyRepository;
            this.addressAuditLogConverter = addressAuditLogConverter;
            this.configurationSettings = configurationSettings.Value;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<AddressSearchAuditLogDto>> GetAllAsync()
        {
            var entities = await this.readOnlyRepository.ListAsync(configurationSettings.MaxRowCount);

            return this.addressAuditLogConverter.ToAddressSearchAuditLogDtos(entities.OrderBy(log => log.LogDate));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<AddressSearchAuditLogDto>> GetAllByDateRangeAsync(DateTime startDate, DateTime endDate) 
        {
            if (startDate == default)
            {
                throw new ArgumentNullException($"{nameof(startDate)} is not valid");
            }

            if (endDate == default)
            {
                throw new ArgumentNullException($"{nameof(endDate)} is not valid");
            }

            if (endDate < startDate)
            {
                throw new ArgumentException($"{nameof(startDate)} {startDate.ToShortDateString()} must be before {nameof(endDate)} {endDate.ToShortDateString()}");
            }

            var entities = await this.readOnlyRepository.ListAsync(log => log.LogDate >= startDate && log.LogDate <= endDate.EndOfDay(), configurationSettings.MaxRowCount);

            return this.addressAuditLogConverter.ToAddressSearchAuditLogDtos(entities.OrderBy(log => log.LogDate));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<AddressSearchAuditLogDto>> GetAllByResultCountRangeAsync(int startCount, int endCount)
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

            var entities = await this.readOnlyRepository.ListAsync(log => log.ResultCount >= startCount && log.ResultCount <= endCount, configurationSettings.MaxRowCount);

            return this.addressAuditLogConverter.ToAddressSearchAuditLogDtos(entities.OrderBy(log => log.ResultCount));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<AddressSearchAuditLogDto>> GetAllBySearchTextAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                throw new ArgumentNullException($"{nameof(searchText)}");
            }

            var entities = await this.readOnlyRepository.ListAsync(log => log.AddressSearch.ToLower() == searchText.ToLower(), configurationSettings.MaxRowCount);

            return this.addressAuditLogConverter.ToAddressSearchAuditLogDtos(entities.OrderBy(log => log.LogDate));
        }
    }
}