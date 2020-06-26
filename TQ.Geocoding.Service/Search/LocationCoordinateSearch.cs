using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TQ.Geocoding.Dto.Location;
using TQ.Geocoding.Models;
using TQ.Geocoding.Repository.Location;
using TQ.Geocoding.Service.Config;
using TQ.Geocoding.Service.Interface.Converters;
using TQ.Geocoding.Service.Interface.Helpers;
using TQ.Geocoding.Service.Interface.Search;

namespace TQ.Geocoding.Service.Search
{
    public class LocationCoordinateSearch : ILocationCoordinateSearch
    {
        private readonly ILocationReadOnlyRepository<vwSearchableLocationDetails> readOnlyRepository;
        private readonly ILocationCoordinateConverter locationCoordinateConverter;
        private readonly ILocationAuditLoggerHelper auditLoggerHelper;
        private readonly ConfigurationSettings configurationSettings;

        /// <summary>
        /// Creates a new <see cref="LocationCoordinateSearch"/>
        /// </summary>
        /// <param name="readOnlyRepository">the read-only repository to use</param>
        /// <param name="locationCoordinateConverter">the location coordinate converter to use</param>
        /// <param name="auditLoggerHelper">the audit logger helper to use</param>
        /// <param name="configurationSettings">the configuration settings to use</param>
        public LocationCoordinateSearch(ILocationReadOnlyRepository<vwSearchableLocationDetails> readOnlyRepository,
                                        ILocationCoordinateConverter locationCoordinateConverter,
                                        ILocationAuditLoggerHelper auditLoggerHelper,
                                        IOptions<ConfigurationSettings> configurationSettings)
        {
            this.readOnlyRepository = readOnlyRepository;
            this.locationCoordinateConverter = locationCoordinateConverter;
            this.auditLoggerHelper = auditLoggerHelper;
            this.configurationSettings = configurationSettings.Value;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<LocationCoordinateDto>> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException($"{nameof(id)} is null or empty");
            }

            id = id.Replace(" ", string.Empty);

            var entities = await this.readOnlyRepository.ListAsync(locationcoordinate => locationcoordinate.SearchLocation.StartsWith(id), configurationSettings.MaxRowCount);

            await this.auditLoggerHelper.LogAsync(new ReadOnlyDictionary<string, object>(new Dictionary<string, object> { { nameof(id), id } }), entities?.Count());

            return this.locationCoordinateConverter.ToLocationCoordinateDtoList(entities);
        }
    }
}