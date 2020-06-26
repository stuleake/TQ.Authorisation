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
    public class AddressEastingNorthingSearch : AddressCoordinateValidation, IAddressEastingNorthingSearch
    {
        private readonly IReadOnlyRepository<SearchableAddress> readOnlyRepository;
        private readonly ISearchableAddressConverter addressConverter;
        private readonly ICoordinateConverter coordinateConverter;
        private readonly ISearchableWelshAddressConverter welshAddressConverter;
        private readonly ICoordinateHelper coordinateHelper;
        private readonly IAddressAuditLoggerHelper auditLoggerHelper;
        private readonly ConfigurationSettings configurationSettings;

        /// <summary>
        /// Creates a new instance of the <see cref="AddressUprnSearch"/>class
        /// </summary>
        /// <param name="readOnlyRepository">the read only repository to use</param>
        /// <param name="addressConverter">the address converter to use</param>
        /// <param name="coordinateConverter">the coordinate converter to use</param>
        /// <param name="welshAddressConverter">the welsh address converter to use</param>
        /// <param name="coordinateHelper">the coordinate helper to use</param>
        /// <param name="auditLoggerHelper">the audit logger helper to use</param>
        /// <param name="configurationSettings">the configuration settings to use</param>
        public AddressEastingNorthingSearch(IReadOnlyRepository<SearchableAddress> readOnlyRepository,
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
        public async Task<IEnumerable<SimpleAddressDto>> GetAddressesByEastingNorthingAsync(GetAddressByEastingNorthingRequest request)
        {
            this.ValidateRequest(request);
            this.ValidateCoordinates(request.XCoordinateEasting, request.YCoordinateNorthing);

            var searchableAddresses = await GetSearchableAddressesAsync(request);

            await this.auditLoggerHelper.LogAsync(new ReadOnlyDictionary<string, object>(new Dictionary<string, object> 
            { 
                { nameof(request.ByClosestCoordinates), request.ByClosestCoordinates },
                { nameof(request.Country), request.Country },
                { nameof(request.Radius), request.Radius },
                { nameof(request.XCoordinateEasting), request.XCoordinateEasting },
                { nameof(request.YCoordinateNorthing), request.YCoordinateNorthing }

            }), searchableAddresses?.Count());

            return addressConverter.ToSimpleAddressDtoList(searchableAddresses);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SimpleWelshAddressDto>> GetWelshAddressesByEastingNorthingAsync(GetAddressByEastingNorthingRequest request)
        {
            this.ValidateRequest(request);
            this.ValidateCoordinates(request.XCoordinateEasting, request.YCoordinateNorthing);

            var searchableAddresses = await GetSearchableAddressesAsync(request);

            await this.auditLoggerHelper.LogAsync(new ReadOnlyDictionary<string, object>(new Dictionary<string, object>
            {
                { nameof(request.ByClosestCoordinates), request.ByClosestCoordinates },
                { nameof(request.Country), request.Country },
                { nameof(request.Radius), request.Radius },
                { nameof(request.XCoordinateEasting), request.XCoordinateEasting },
                { nameof(request.YCoordinateNorthing), request.YCoordinateNorthing }

            }), searchableAddresses?.Count());

            return welshAddressConverter.ToSimpleAddressDtoList(searchableAddresses);
        }

        private async Task<IEnumerable<SearchableAddress>> GetSearchableAddressesAsync(GetAddressByEastingNorthingRequest request)
        {
            ICoordinates coordinates = coordinateConverter.ApplyRadiusToCoordinates(request.XCoordinateEasting, request.YCoordinateNorthing, request.Radius);
            
            if (request.Radius == 0)
            {
                coordinates = this.coordinateHelper.GetMaxCoordinates(coordinates);
            }

            var searchableAddresses = await GetAddressesAsync(coordinates);
            
            if (request.ByClosestCoordinates == false)
            {
                return searchableAddresses;
            }

            var closestCoordinates = this.coordinateHelper.GetClosestCoordinates(searchableAddresses, request.XCoordinateEasting, request.YCoordinateNorthing, coordinates.IsLongitudeLatitude);

            if (closestCoordinates.ClosestEasting == 0)
            {
                return null;
            }

            return await readOnlyRepository.ListAsync(address => address.XCoordinateEasting == closestCoordinates.ClosestEasting && address.YCoordinateNorthing == closestCoordinates.ClosestNorthing,
                                                      configurationSettings.MaxRowCount);
        }

        private async Task<IEnumerable<SearchableAddress>> GetAddressesAsync(ICoordinates coordinates)
        {
            return await readOnlyRepository.ListAsync(f => f.XCoordinateEasting >= coordinates.MinX
                                && f.XCoordinateEasting <= coordinates.MaxX
                                && f.YCoordinateNorthing >= coordinates.MinY
                                && f.YCoordinateNorthing <= coordinates.MaxY,
                                configurationSettings.MaxRowCount);
        }
    }
}