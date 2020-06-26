using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TQ.Geocoding.Dto.Address;
using TQ.Geocoding.Models;
using TQ.Geocoding.Repository;
using TQ.Geocoding.Service.Config;
using TQ.Geocoding.Service.Extensions;
using TQ.Geocoding.Service.Interface.Converters;
using TQ.Geocoding.Service.Interface.Helpers;
using TQ.Geocoding.Service.Interface.Search;
using TQ.Geocoding.Service.Validators;

namespace TQ.Geocoding.Service.Search
{
    public class AddressPostcodeSearch : IAddressPostcodeSearch
    {
        private readonly IReadOnlyRepository<SearchableAddress> readOnlyRepository;
        private readonly ISearchableAddressConverter addressConverter;
        private readonly ISearchableWelshAddressConverter welshAddressConverter;
        private readonly IAddressAuditLoggerHelper auditLoggerHelper;
        private readonly ConfigurationSettings configurationSettings;

        /// <summary>
        /// Creates a new instance of the <see cref="AddressUprnSearch"/>class
        /// </summary>
        /// <param name="readOnlyRepository">the read only repository to use</param>
        /// <param name="addressConverter">the address converter to use</param>
        /// <param name="welshAddressConverter">the welsh address converter to use</param>
        /// <param name="auditLoggerHelper">the audit logger helper to use</param>
        /// <param name="configurationSettings">the configuration settings to use</param>
        public AddressPostcodeSearch(IReadOnlyRepository<SearchableAddress> readOnlyRepository,
                                    ISearchableAddressConverter addressConverter,
                                    ISearchableWelshAddressConverter welshAddressConverter,
                                    IAddressAuditLoggerHelper auditLoggerHelper,
                                    IOptions<ConfigurationSettings> configurationSettings)
        {
            this.readOnlyRepository = readOnlyRepository;
            this.addressConverter = addressConverter;
            this.welshAddressConverter = welshAddressConverter;
            this.auditLoggerHelper = auditLoggerHelper;
            this.configurationSettings = configurationSettings.Value;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SimpleAddressDto>> GetSimpleAddressesByPostcodeAsync(string postcode)
        {
            Validate(postcode);

            var entities = await GetSearchableAddresses(postcode);

            await this.auditLoggerHelper.LogAsync(new ReadOnlyDictionary<string, object>(new Dictionary<string, object> { { nameof(postcode), postcode } }), entities?.Count());

            return addressConverter.ToSimpleAddressDtoList(entities);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<FullAddressDto>> GetFullAddressesByPostcodeAsync(string postcode)
        {
            Validate(postcode);

            var entities = await GetSearchableAddresses(postcode);

            await this.auditLoggerHelper.LogAsync(new ReadOnlyDictionary<string, object>(new Dictionary<string, object> { { nameof(postcode), postcode } }), entities?.Count());

            return addressConverter.ToFullAddressDtoList(entities);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SimpleWelshAddressDto>> GetSimpleWelshAddressesByPostcodeAsync(string postcode)
        {
            Validate(postcode);

            var entities = await GetSearchableAddresses(postcode);

            await this.auditLoggerHelper.LogAsync(new ReadOnlyDictionary<string, object>(new Dictionary<string, object> { { nameof(postcode), postcode } }), entities?.Count());

            return welshAddressConverter.ToSimpleAddressDtoList(entities);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<FullWelshAddressDto>> GetFullWelshAddressesByPostcodeAsync(string postcode)
        {
            Validate(postcode);

            var entities = await GetSearchableAddresses(postcode);

            await this.auditLoggerHelper.LogAsync(new ReadOnlyDictionary<string, object>(new Dictionary<string, object> { { nameof(postcode), postcode } }), entities?.Count());

            return welshAddressConverter.ToFullAddressDtoList(entities);
        }

        private async Task<IEnumerable<SearchableAddress>> GetSearchableAddresses(string postcode)
        {
            return await readOnlyRepository.ListAsync(p => p.Postcode == postcode.ToPostcodeFormat(), configurationSettings.MaxRowCount);
        }

        public void Validate(string postcode)
        {
            ServiceValidator.ValidateArguments(new ValidatorList
            {
                new IsNullOrDefaultValidator<string>(postcode),
                new IsPostcodeValidator(postcode)
            });
        }
    }
}