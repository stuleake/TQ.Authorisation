using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TQ.Geocoding.Dto.Address;
using TQ.Geocoding.Models;
using TQ.Geocoding.Repository;
using TQ.Geocoding.Requests;
using TQ.Geocoding.Service.Config;
using TQ.Geocoding.Service.Interface.Builders;
using TQ.Geocoding.Service.Interface.Converters;
using TQ.Geocoding.Service.Interface.Helpers;
using TQ.Geocoding.Service.Interface.Search;

namespace TQ.Geocoding.Service.Search
{
    public class AddressTextSearch : IAddressTextSearch
    {
        private readonly IReadOnlyRepository<SearchableAddress> readOnlyRepository;
        private readonly IReadOnlyRepository<vwSearchableAddress> welshReadOnlyRepository;
        private readonly ISearchableAddressConverter addressConverter;
        private readonly ISearchableWelshAddressConverter welshAddressConverter;
        private readonly IPredicateBuilder predicateBuilder;
        private readonly IAddressAuditLoggerHelper auditLoggerHelper;
        private readonly ConfigurationSettings configurationSettings;

        /// <summary>
        /// Creates a new instance of the <see cref="AddressTextSearch"/> class
        /// </summary>
        /// <param name="readOnlyRepository">the read only repository to use</param>
        /// <param name="welshReadOnlyRepository">the welsh read only repository to use</param>
        /// <param name="addressConverter">the address converter to use</param>
        /// <param name="welshAddressConverter">the welsh address converter to use</param>
        /// <param name="predicateBuilder">the predicate builder to use</param>
        /// <param name="auditLoggerHelper">the audit logger helper to use</param>
        /// <param name="configurationSettings">the configuration settings to use</param>
        public AddressTextSearch(IReadOnlyRepository<SearchableAddress> readOnlyRepository,
                                        IReadOnlyRepository<vwSearchableAddress> welshReadOnlyRepository,
                                        ISearchableAddressConverter addressConverter,
                                        ISearchableWelshAddressConverter welshAddressConverter,
                                        IPredicateBuilder predicateBuilder,
                                        IAddressAuditLoggerHelper auditLoggerHelper,
                                        IOptions<ConfigurationSettings> configurationSettings)
        {
            this.readOnlyRepository = readOnlyRepository;
            this.welshReadOnlyRepository = welshReadOnlyRepository;
            this.addressConverter = addressConverter;
            this.welshAddressConverter = welshAddressConverter;
            this.predicateBuilder = predicateBuilder;
            this.auditLoggerHelper = auditLoggerHelper;
            this.configurationSettings = configurationSettings.Value;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SimpleAddressDto>> GetSimpleAddressByTextAsync(GetAddressByTextRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException($"{nameof(request)} is null");
            }

            Expression<Func<SearchableAddress, bool>> pred = predicateBuilder.BuildPredicate(request.SearchString);
            var result = await readOnlyRepository.ListAsync(pred, configurationSettings.MaxRowCount);

            await this.auditLoggerHelper.LogAsync(new ReadOnlyDictionary<string, object>(new Dictionary<string, object> { { nameof(request.SearchString), request.SearchString } }), result?.Count());

            return addressConverter.ToSimpleAddressDtoList(result);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SimpleWelshAddressDto>> GetSimpleWelshAddressByTextAsync(GetAddressByTextRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException($"{nameof(request)} is null");
            }

            Expression<Func<vwSearchableAddress, bool>> pred = predicateBuilder.BuildWelshPredicate(request.SearchString);
            var result = await welshReadOnlyRepository.ListAsync(pred, configurationSettings.MaxRowCount);

            await this.auditLoggerHelper.LogAsync(new ReadOnlyDictionary<string, object>(new Dictionary<string, object> { { nameof(request.SearchString), request.SearchString } }), result?.Count());

            return welshAddressConverter.ToSimpleAddressDtoList(result);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<FullAddressDto>> GetFullAddressByTextAsync(GetAddressByTextRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException($"{nameof(request)} is null");
            }

            Expression<Func<SearchableAddress, bool>> pred = predicateBuilder.BuildPredicate(request.SearchString);
            var result = await readOnlyRepository.ListAsync(pred, configurationSettings.MaxRowCount);

            await this.auditLoggerHelper.LogAsync(new ReadOnlyDictionary<string, object>(new Dictionary<string, object> { { nameof(request.SearchString), request.SearchString } }), result?.Count());

            return addressConverter.ToFullAddressDtoList(result);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<FullWelshAddressDto>> GetFullWelshAddressByTextAsync(GetAddressByTextRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException($"{nameof(request)} is null");
            }

            Expression<Func<vwSearchableAddress, bool>> pred = predicateBuilder.BuildWelshPredicate(request.SearchString);
            var result = await welshReadOnlyRepository.ListAsync(pred, configurationSettings.MaxRowCount);

            await this.auditLoggerHelper.LogAsync(new ReadOnlyDictionary<string, object>(new Dictionary<string, object> { { nameof(request.SearchString), request.SearchString } }), result?.Count());

            return welshAddressConverter.ToFullAddressDtoList(result);
        }
    }
}