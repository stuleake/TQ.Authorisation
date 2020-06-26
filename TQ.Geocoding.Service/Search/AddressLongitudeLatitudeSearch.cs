using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TQ.Geocoding.Dto.Address;
using TQ.Geocoding.Models;
using TQ.Geocoding.Repository;
using TQ.Geocoding.Requests;
using TQ.Geocoding.Service.Config;
using TQ.Geocoding.Service.Interface;
using TQ.Geocoding.Service.Interface.Converters;
using TQ.Geocoding.Service.Interface.Helpers;
using TQ.Geocoding.Service.Interface.Search;

namespace TQ.Geocoding.Service.Search
{
    public class AddressLongitudeLatitudeSearch : AddressCoordinateValidation, IAddressLongitudeLatitudeSearch
    {
        private readonly IReadOnlyRepository<SearchableAddress> readOnlyRepository;
        private readonly ISearchableAddressConverter addressConverter;
        private readonly ICoordinateConverter coordinateConverter;
        private readonly ISearchableWelshAddressConverter welshAddressConverter;
        private readonly ICoordinateHelper coordinateHelper;
        private readonly IAddressAuditLoggerHelper auditLoggerHelper;
        private readonly ConfigurationSettings configurationSettings;

        /// <summary>
        /// Creates a new instance of the <see cref="AddressLongitudeLatitudeSearch"/>class
        /// </summary>
        /// <param name="readOnlyRepository">the read only repository to use</param>
        /// <param name="addressConverter">the address converter to use</param>
        /// <param name="coordinateConverter">the coordinate converter to use</param>
        /// <param name="welshAddressConverter">the welsh address converter to use</param>
        /// <param name="coordinateHelper">the coordinate helper to use</param>
        /// <param name="auditLoggerHelper">the audit logger helper to use</param>
        /// <param name="configurationSettings">the configuration settings to use</param>
        public AddressLongitudeLatitudeSearch(IReadOnlyRepository<SearchableAddress> readOnlyRepository,
                                        ISearchableAddressConverter addressConverter,
                                        ICoordinateConverter coordinateConverter,
                                        ISearchableWelshAddressConverter welshAddressConverter,
                                        ICoordinateHelper coordinateHelper,
                                        IAddressAuditLoggerHelper auditLoggerHelper,
                                        IOptions<ConfigurationSettings> configurationSettings)
        {
            this.readOnlyRepository = readOnlyRepository;
            this.addressConverter = addressConverter;
            this.coordinateConverter = coordinateConverter;
            this.welshAddressConverter = welshAddressConverter;
            this.coordinateHelper = coordinateHelper;
            this.auditLoggerHelper = auditLoggerHelper;
            this.configurationSettings = configurationSettings.Value;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SimpleAddressDto>> GetAddressesByLongitudeLatitudeAsync(GetAddressByLongitudeLatitudeRequest request)
        {
            this.ValidateRequest(request);
            this.ValidateCoordinates(request.Longitude, request.Latitude);
            
            var searchableAddresses = await GetSearchableAddressesAsync(request);

            return addressConverter.ToSimpleAddressDtoList(searchableAddresses);
        }

        public async Task<IEnumerable<SimpleWelshAddressDto>> GetWelshAddressesByLongitudeLatitudeAsync(GetAddressByLongitudeLatitudeRequest request)
        {
            this.ValidateRequest(request);
            this.ValidateCoordinates(request.Longitude, request.Latitude);

            var searchableAddresses = await GetSearchableAddressesAsync(request);

            return welshAddressConverter.ToSimpleAddressDtoList(searchableAddresses);
        }

        private async Task<IEnumerable<SearchableAddress>> GetSearchableAddressesAsync(GetAddressByLongitudeLatitudeRequest request)
        {
            ICoordinates coordinates = coordinateConverter.ApplyRadiusToCoordinates(request.Longitude, request.Latitude, request.Radius);
            
            if (request.Radius == 0)
            {
                coordinates = this.coordinateHelper.GetMaxCoordinates(coordinates);
            }

            var searchableAddresses = await GetAddressesAsync(coordinates);

            await this.auditLoggerHelper.LogAsync(new ReadOnlyDictionary<string, object>(new Dictionary<string, object>
            {
                { nameof(request.ByClosestCoordinates), request.ByClosestCoordinates },
                { nameof(request.Latitude), request.Latitude },
                { nameof(request.Longitude), request.Longitude },
                { nameof(request.Radius), request.Radius},

            }), searchableAddresses?.Count());

            if (request.ByClosestCoordinates == false)
            {
                return searchableAddresses;
            }

            var closestCoordinates = this.coordinateHelper.GetClosestCoordinates(searchableAddresses, request.Longitude, request.Latitude, coordinates.IsLongitudeLatitude);
            
            if (closestCoordinates.ClosestEasting == 0)
            {
                return null;
            }

            var results = await readOnlyRepository
                            .ListAsync(address => address.XCoordinateEasting == closestCoordinates.ClosestEasting 
                                        && address.YCoordinateNorthing == closestCoordinates.ClosestNorthing,
                                       configurationSettings.MaxRowCount);

            return results;
        }

        private async Task<IEnumerable<SearchableAddress>> GetAddressesAsync(ICoordinates coordinates)
        {
            if (coordinates.MaxX > coordinates.MinX)
            {
                return await readOnlyRepository.ListAsync(f => f.Longitude >= coordinates.MinX
                                                && f.Longitude <= coordinates.MaxX
                                                && f.Latitude >= coordinates.MinY
                                                && f.Latitude <= coordinates.MaxY,
                                                configurationSettings.MaxRowCount);
            }

            return await readOnlyRepository.ListAsync(f => f.Longitude >= coordinates.MaxX
                                            && f.Longitude <= coordinates.MinX
                                            && f.Latitude >= coordinates.MinY
                                            && f.Latitude <= coordinates.MaxY,
                                            configurationSettings.MaxRowCount);
        }
    }
}