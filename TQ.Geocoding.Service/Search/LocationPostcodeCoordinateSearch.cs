using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TQ.Geocoding.Dto.Dtos;
using TQ.Geocoding.Models;
using TQ.Geocoding.Repository.Location;
using TQ.Geocoding.Service.Extensions;
using TQ.Geocoding.Service.Interface.Converters;
using TQ.Geocoding.Service.Interface.Helpers;
using TQ.Geocoding.Service.Interface.Search;

namespace TQ.Geocoding.Service.Search
{
    public class LocationPostcodeCoordinateSearch : ILocationPostcodeCoordinateSearch
    {
        private readonly ILocationReadOnlyRepository<PostcodeCoordinates> readOnlyRepository;
        private readonly IPostcodeCoordinateConverter postcodeCoordinateConverter;
        private readonly ILocationAuditLoggerHelper auditLoggerHelper;

        /// <summary>
        /// Creates a new <see cref="LocationPostcodeCoordinateSearch"/>
        /// </summary>
        /// <param name="readOnlyRepository">the read-only repository to use</param>
        /// <param name="postcodeCoordinateConverter">the postcode coordinate converter to use</param>
        /// <param name="auditLoggerHelper">the audit logger helper to use</param>
        public LocationPostcodeCoordinateSearch(ILocationReadOnlyRepository<PostcodeCoordinates> readOnlyRepository,
                                        IPostcodeCoordinateConverter postcodeCoordinateConverter,
                                        ILocationAuditLoggerHelper auditLoggerHelper)
        {
            this.readOnlyRepository = readOnlyRepository;
            this.postcodeCoordinateConverter = postcodeCoordinateConverter;
            this.auditLoggerHelper = auditLoggerHelper;
        }

        /// <inheritdoc/>
        public async Task<PostcodeCoordinateDto> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!id.IsPostCode())
            {
                throw new ArgumentException($"{nameof(id)} is not a valid postcode");
            }

            id = id.Replace(" ", string.Empty);

            var entity = await this.readOnlyRepository.FirstOrDefaultAsync(postcodecoordinate => postcodecoordinate.Id == id);

            await this.auditLoggerHelper.LogAsync(new ReadOnlyDictionary<string, object>(new Dictionary<string, object> { { nameof(id), id } }), entity == null ? 0 : 1);

            return this.postcodeCoordinateConverter.ToPostcodeCoordinateDto(entity);
        }
    }
}