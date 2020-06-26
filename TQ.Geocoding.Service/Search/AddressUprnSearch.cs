using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TQ.Geocoding.Dto.Address;
using TQ.Geocoding.Models;
using TQ.Geocoding.Repository;
using TQ.Geocoding.Service.Interface.Converters;
using TQ.Geocoding.Service.Interface.Helpers;
using TQ.Geocoding.Service.Interface.Search;

namespace TQ.Geocoding.Service.Search
{
    public class AddressUprnSearch : IAddressUprnSearch
    {
        private readonly IReadOnlyRepository<SearchableAddress> readOnlyRepository;
        private readonly ISearchableAddressConverter addressConverter;
        private readonly ISearchableWelshAddressConverter welshAddressConverter;
        private readonly IAddressAuditLoggerHelper auditLoggerHelper;

        /// <summary>
        /// Creates a new instance of the <see cref="AddressUprnSearch"/>class
        /// </summary>
        /// <param name="readOnlyRepository">the read only repository to use</param>
        /// <param name="addressConverter">the address converter to use</param>
        /// <param name="welshAddressConverter">the welsh address converter to use</param>
        /// <param name="auditLoggerHelper">the audit logger helper to use</param>
        public AddressUprnSearch(IReadOnlyRepository<SearchableAddress> readOnlyRepository,
                                ISearchableAddressConverter addressConverter,
                                ISearchableWelshAddressConverter welshAddressConverter,
                                IAddressAuditLoggerHelper auditLoggerHelper)
        {
            this.readOnlyRepository = readOnlyRepository;
            this.addressConverter = addressConverter;
            this.welshAddressConverter = welshAddressConverter;
            this.auditLoggerHelper = auditLoggerHelper;
        }

        /// <inheritdoc/>
        public async Task<FullAddressDto> GetFullAddressByUprnAsync(long uprn)
        {
            var entity = await GetSearchableAddress(uprn);

            await this.auditLoggerHelper.LogAsync(new ReadOnlyDictionary<string, object>(new Dictionary<string, object> { { nameof(uprn), uprn } }), entity == null ? 0 : 1);

            return addressConverter.ToFullAddressDto(entity);
        }

        /// <inheritdoc/>
        public async Task<SimpleAddressDto> GetSimpleAddressByUprnAsync(long uprn)
        {
            var entity = await GetSearchableAddress(uprn);

            await this.auditLoggerHelper.LogAsync(new ReadOnlyDictionary<string, object>(new Dictionary<string, object> { { nameof(uprn), uprn } }), entity == null ? 0 : 1);

            return addressConverter.ToSimpleAddressDto(entity);
        }

        /// <inheritdoc/>
        public async Task<FullWelshAddressDto> GetFullWelshAddressByUprnAsync(long uprn)
        {
            var entity = await GetSearchableAddress(uprn);

            await this.auditLoggerHelper.LogAsync(new ReadOnlyDictionary<string, object>(new Dictionary<string, object> { { nameof(uprn), uprn.ToString() } }), entity == null ? 0 : 1);

            return welshAddressConverter.ToFullAddressDto(entity);
        }

        /// <inheritdoc/>
        public async Task<SimpleWelshAddressDto> GetSimpleWelshAddressByUprnAsync(long uprn)
        {
            var entity = await GetSearchableAddress(uprn);

            await this.auditLoggerHelper.LogAsync(new ReadOnlyDictionary<string, object>(new Dictionary<string, object> { { nameof(uprn), uprn.ToString() } }), entity == null ? 0 : 1);

            return welshAddressConverter.ToSimpleAddressDto(entity);
        }

        private async Task<SearchableAddress> GetSearchableAddress(long uprn)
        {
            if (uprn == default)
            {
                throw new ArgumentException($"{nameof(uprn)} is not valid");
            }

            return await readOnlyRepository.FirstOrDefaultAsync(u => u.Uprn == uprn);
        }
    }
}