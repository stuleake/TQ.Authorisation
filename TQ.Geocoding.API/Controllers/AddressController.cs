using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TQ.Geocoding.Dto.Address;
using TQ.Geocoding.Dtos.Address;
using TQ.Geocoding.Requests;
using TQ.Geocoding.Service.Interface.Audit;
using TQ.Geocoding.Service.Interface.Search;

namespace TQ.Geocoding.API.Controllers
{
    [Route("addresses")]
    public class AddressController : ControllerBase
    {
        private readonly ILogger<AddressController> _logger;
        private readonly IAddressUprnSearch addressUprnSearch;
        private readonly IAddressEastingNorthingSearch addressEastingNorthingSearch;
        private readonly IAddressLongitudeLatitudeSearch addressLongitudeLatitudeSearch;
        private readonly IAddressPostcodeSearch addressPostcodeSearch;
        private readonly IAddressTextSearch addressTextSearch;
        private readonly IReadOnlyAddressAuditLogger readOnlyAddressAuditLogger;

        /// <summary>
        /// Creates a new instance of the <see cref="AddressController"/>
        /// </summary>
        /// <param name="logger">the logger to use</param>
        /// <param name="addressUrpnSearch">the address uprn search to use</param>
        /// <param name="addressEastingNorthingSearch">the address easting northing search to use</param>
        /// <param name="addressLongitudeLatitudeSearch">the address longitude latitude search to use</param>
        /// <param name="addressPostcodeSearch">the address postcode search to use</param>
        /// <param name="addressTextSearch">the address text search to use</param>
        /// <param name="readOnlyAddressAuditLogger">the address audit logger to use</param>
        public AddressController(ILogger<AddressController> logger,
                                IAddressUprnSearch addressUrpnSearch,
                                IAddressEastingNorthingSearch addressEastingNorthingSearch,
                                IAddressLongitudeLatitudeSearch addressLongitudeLatitudeSearch,
                                IAddressPostcodeSearch addressPostcodeSearch,
                                IAddressTextSearch addressTextSearch,
                                IReadOnlyAddressAuditLogger readOnlyAddressAuditLogger)
        {
            _logger = logger;
            this.addressUprnSearch = addressUrpnSearch;
            this.addressEastingNorthingSearch = addressEastingNorthingSearch;
            this.addressLongitudeLatitudeSearch = addressLongitudeLatitudeSearch;
            this.addressPostcodeSearch = addressPostcodeSearch;
            this.addressTextSearch = addressTextSearch;
            this.readOnlyAddressAuditLogger = readOnlyAddressAuditLogger;
        }

        #region By Postcode

        /// <summary>
        /// Returns a collection of SimpleAddressDto's that match the specified postcode
        /// </summary>
        /// <param name="postcode">the postcode to search for</param>
        /// <returns>a <see cref="Task{IActionResult}"/>that contains a <see cref="IEnumerable{SimpleAddressDto}"/></returns>
        [HttpGet("simple/by-postcode/english")]
        [ProducesResponseType(typeof(IEnumerable<SimpleAddressDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSimpleAddressByPostCodeAsync([FromQuery]string postcode)
        {
            var dtos = await this.addressPostcodeSearch.GetSimpleAddressesByPostcodeAsync(HttpUtility.UrlDecode(postcode));
            
            return Ok(dtos);
        }

        /// <summary>
        /// Returns a collection of SimpleWelshAddressDto's that match the specified postcode
        /// </summary>
        /// <param name="postcode">the postcode to search for</param>
        /// <returns>a <see cref="Task{IActionResult}"/>that contains a <see cref="IEnumerable{SimpleWelshAddressDto}"/></returns>
        [HttpGet("simple/by-postcode/welsh")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWelshAddressDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSimpleWelshAddressByPostCodeAsync([FromQuery]string postcode)
        {
            var dtos = await this.addressPostcodeSearch.GetSimpleWelshAddressesByPostcodeAsync(HttpUtility.UrlDecode(postcode));
            
            return Ok(dtos);
        }

        /// <summary>
        /// Returns a collection of FullAddressDto's that match the specified postcode
        /// </summary>
        /// <param name="postcode">the postcode to search for</param>
        /// <returns>a <see cref="Task{IActionResult}"/>that contains a <see cref="IEnumerable{FullAddressDto}"/></returns>
        [HttpGet("full/by-postcode/english")]
        [ProducesResponseType(typeof(IEnumerable<FullAddressDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFullAddressByPostCodeAsync([FromQuery]string postcode)
        {
            var dtos = await this.addressPostcodeSearch.GetFullAddressesByPostcodeAsync(HttpUtility.UrlDecode(postcode));
            
            return Ok(dtos);
        }

        /// <summary>
        /// Returns a collection of FullWelshAddressDto's that match the specified postcode
        /// </summary>
        /// <param name="postcode">the postcode to search for</param>
        /// <returns>a <see cref="Task{IActionResult}"/>that contains a <see cref="IEnumerable{FullWelshAddressDto}"/></returns>
        [HttpGet("full/by-postcode/welsh")]
        [ProducesResponseType(typeof(IEnumerable<FullWelshAddressDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFullWelshAddressByPostCodeAsync([FromQuery]string postcode)
        {
            var dtos = await this.addressPostcodeSearch.GetFullWelshAddressesByPostcodeAsync(HttpUtility.UrlDecode(postcode));
            
            return Ok(dtos);
        }

        #endregion

        #region By Text

        /// <summary>
        /// Returns a collection of SimpleAddressDto's that match the search text in the request
        /// </summary>
        /// <param name="request">the request that contains the text to search for</param>
        /// <returns>a <see cref="Task{IActionResult}"/>that contains a <see cref="IEnumerable{SimpleAddressDto}"/></returns>
        [HttpPost("simple/by-text/english")]
        [ProducesResponseType(typeof(IEnumerable<SimpleAddressDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSimpleAddressesByTextAsync([FromBody]GetAddressByTextRequest request)
        {
            var dtos = await addressTextSearch.GetSimpleAddressByTextAsync(request);
            
            return Ok(dtos);
        }

        /// <summary>
        /// Returns a collection of FullAddressDto's that match the search text in the request
        /// </summary>
        /// <param name="request">the request that contains the text to search for</param>
        /// <returns>a <see cref="Task{IActionResult}"/>that contains a <see cref="IEnumerable{FullAddressDto}"/></returns>
        [HttpPost("full/by-text/english")]
        [ProducesResponseType(typeof(IEnumerable<FullAddressDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFullAddressesByTextAsync([FromBody]GetAddressByTextRequest request)
        {
            var dtos = await addressTextSearch.GetFullAddressByTextAsync(request);
            
            return Ok(dtos);
        }

        /// <summary>
        /// Returns a collection of SimpleWelshAddressDto's that match the search text in the request
        /// </summary>
        /// <param name="request">the request that contains the text to search for</param>
        /// <returns>a <see cref="Task{IActionResult}"/>that contains a <see cref="IEnumerable{SimpleWelshAddressDto}"/></returns>
        [HttpPost("simple/by-text/welsh")]
        [ProducesResponseType(typeof(IEnumerable<SimpleAddressDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSimpleWelshAddressesByTextAsync([FromBody]GetAddressByTextRequest request)
        {
            var dtos = await addressTextSearch.GetSimpleWelshAddressByTextAsync(request);
            
            return Ok(dtos);
        }

        /// <summary>
        /// Returns a collection of FullWelshAddressDto's that match the search text in the request
        /// </summary>
        /// <param name="request">the request that contains the text to search for</param>
        /// <returns>a <see cref="Task{IActionResult}"/>that contains a <see cref="IEnumerable{FullWelshAddressDto}"/></returns>
        [HttpPost("full/by-text/welsh")]
        [ProducesResponseType(typeof(IEnumerable<FullWelshAddressDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFullWelshAddressesByTextAsync([FromBody]GetAddressByTextRequest request)
        {
            var dtos = await addressTextSearch.GetFullWelshAddressByTextAsync(request);
            
            return Ok(dtos);
        }

        #endregion

        #region By UPRN

        /// <summary>
        /// Returns a FullAddressDto that matches the specified uprn
        /// </summary>
        /// <param name="uprn">the uprn to search for</param>
        /// <returns>a <see cref="Task{IActionResult}"/>that contains a <see cref="FullAddressDto"/></returns>
        [HttpGet("full/by-uprn/english")]
        [ProducesResponseType(typeof(FullAddressDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFullAddressByUprnAsync([FromQuery]long uprn)
        {
            var dto = await addressUprnSearch.GetFullAddressByUprnAsync(uprn);
            
            return Ok(dto);
        }

        /// <summary>
        /// Returns a SimpleAddressDto that matches the specified uprn
        /// </summary>
        /// <param name="uprn">the uprn to search for</param>
        /// <returns>a <see cref="Task{IActionResult}"/>that contains a <see cref="SimpleAddressDto"/></returns>
        [HttpGet("simple/by-uprn/english")]
        [ProducesResponseType(typeof(SimpleAddressDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSimpleAddressByUprnAsync([FromQuery]long uprn)
        {
            var dto = await addressUprnSearch.GetSimpleAddressByUprnAsync(uprn);
            
            return Ok(dto);
        }

        /// <summary>
        /// Returns a SimpleWelshAddressDto that matches the specified uprn
        /// </summary>
        /// <param name="uprn">the uprn to search for</param>
        /// <returns>a <see cref="Task{IActionResult}"/>that contains a <see cref="SimpleWelshAddressDto"/></returns>
        [HttpGet("simple/by-uprn/welsh")]
        [ProducesResponseType(typeof(SimpleWelshAddressDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSimpleWelshAddressByUprnAsync([FromQuery]long uprn)
        {
            var dto = await addressUprnSearch.GetSimpleWelshAddressByUprnAsync(uprn);
            
            return Ok(dto);
        }

        /// <summary>
        /// Returns a FullWelshAddressDto that match the specified uprn
        /// </summary>
        /// <param name="uprn">the uprn to search for</param>
        /// <returns>a <see cref="Task{IActionResult}"/>that contains a <see cref="FullWelshAddressDto"/></returns>
        [HttpGet("simple/by-uprn/full")]
        [ProducesResponseType(typeof(FullWelshAddressDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFullWelshAddressByUprnAsync([FromQuery]long uprn)
        {
            var dto = await addressUprnSearch.GetFullWelshAddressByUprnAsync(uprn);
            
            return Ok(dto);
        }

        #endregion

        #region By Coordinates (Easting \ Northing)
        /// <summary>
        /// Returns a collection of SimpleAddressDto's that match the easting\northing coordinates in the request
        /// </summary>
        /// <param name="request">the request that contains the coordinates to search for</param>
        /// <returns>a <see cref="Task{IActionResult}"/>that contains a <see cref="IEnumerable{SimpleAddressDto}"/></returns>
        [HttpPost("simple/by-easting-northing/english")]
        [ProducesResponseType(typeof(IEnumerable<SimpleAddressDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAddressesByEastingNorthing([FromBody]GetAddressByEastingNorthingRequest request)
        {
            var dtos = await this.addressEastingNorthingSearch.GetAddressesByEastingNorthingAsync(request);
            
            return Ok(dtos);
        }

        /// <summary>
        /// Returns a collection of SimpleWelshAddressDto's that match the easting\northing coordinates in the request
        /// </summary>
        /// <param name="request">the request that contains the coordinates to search for</param>
        /// <returns>a <see cref="Task{IActionResult}"/>that contains a <see cref="IEnumerable{SimpleWelshAddressDto}"/></returns>
        [HttpPost("simple/by-easting-northing/welsh")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWelshAddressDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWelshAddressesByEastingNorthing([FromBody]GetAddressByEastingNorthingRequest request)
        {
            var dtos = await this.addressEastingNorthingSearch.GetWelshAddressesByEastingNorthingAsync(request);
            
            return Ok(dtos);
        }
        #endregion

        #region By Coordinates (Longitude \ Latitude)
        /// <summary>
        /// Returns a collection of SimpleAddressDto's that match the Longitude\Latitude coordinates in the request
        /// </summary>
        /// <param name="request">the request that contains the coordinates to search for</param>
        /// <returns>a <see cref="Task{IActionResult}"/>that contains a <see cref="IEnumerable{SimpleAddressDto}"/></returns>
        [HttpPost("simple/by-longitude-latitude/english")]
        [ProducesResponseType(typeof(IEnumerable<SimpleAddressDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAddressesByLongitudeLatitude([FromBody]GetAddressByLongitudeLatitudeRequest request)
        {
            var dtos = await this.addressLongitudeLatitudeSearch.GetAddressesByLongitudeLatitudeAsync(request);
            
            return Ok(dtos);
        }

        /// <summary>
        /// Returns a collection of SimpleWelshAddressDto's that match the Longitude\Latitude coordinates in the request
        /// </summary>
        /// <param name="request">the request that contains the coordinates to search for</param>
        /// <returns>a <see cref="Task{IActionResult}"/>that contains a <see cref="IEnumerable{SimpleWelshAddressDto}"/></returns>
        [HttpPost("simple/by-longitude-latitude/welsh")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWelshAddressDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWelshAddressesByLongitudeLatitude([FromBody]GetAddressByLongitudeLatitudeRequest request)
        {
            var dtos = await this.addressLongitudeLatitudeSearch.GetWelshAddressesByLongitudeLatitudeAsync(request);
            
            return Ok(dtos);
        }
        #endregion

        #region Audit Logging

        /// <summary>
        /// Returns a collection of <see cref="IEnumerable{AddressSearchAuditLogDto}"/>
        /// </summary>
        /// <returns>a <see cref="Task{IActionResult}"/>that contains a <see cref="IEnumerable{AddressSearchAuditLogDto}"/></returns>
        [HttpGet("auditlog")]
        [ProducesResponseType(typeof(IEnumerable<AddressSearchAuditLogDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuditLog()
        {
            var dtos = await this.readOnlyAddressAuditLogger.GetAllAsync();

            return Ok(dtos);
        }

        /// <summary>
        /// Returns a collection of <see cref="IEnumerable{AddressSearchAuditLogDto}"/> that match the specified dates
        /// </summary>
        /// <param name="startDate">the start date to search for</param>
        /// <param name="endDate">the end date to search for</param>
        /// <returns>a <see cref="Task{IActionResult}"/>that contains a <see cref="IEnumerable{AddressSearchAuditLogDto}"/></returns>
        [HttpGet("auditlog/by-dates")]
        [ProducesResponseType(typeof(IEnumerable<AddressSearchAuditLogDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuditLogByDates(DateTime startDate, DateTime endDate)
        {
            var dtos = await this.readOnlyAddressAuditLogger.GetAllByDateRangeAsync(startDate, endDate);

            return Ok(dtos);
        }

        /// <summary>
        /// Returns a collection of <see cref="IEnumerable{AddressSearchAuditLogDto}"/> that match the specified result counts
        /// </summary>
        /// <param name="startCount">the start count to search for</param>
        /// <param name="endCount">the end count to search for</param>
        /// <returns>a <see cref="Task{IActionResult}"/>that contains a <see cref="IEnumerable{AddressSearchAuditLogDto}"/></returns>
        [HttpGet("auditlog/by-results-count")]
        [ProducesResponseType(typeof(IEnumerable<AddressSearchAuditLogDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuditLogByResults(int startCount, int endCount)
        {
            var dtos = await this.readOnlyAddressAuditLogger.GetAllByResultCountRangeAsync(startCount, endCount);

            return Ok(dtos);
        }

        /// <summary>
        /// Returns a collection of <see cref="IEnumerable{AddressSearchAuditLogDto}"/> that match the specified search text
        /// </summary>
        /// <param name="searchText">the text to search for</param>
        /// <returns>a <see cref="Task{IActionResult}"/>that contains a <see cref="IEnumerable{AddressSearchAuditLogDto}"/></returns>
        [HttpGet("auditlog/by-Search")]
        [ProducesResponseType(typeof(IEnumerable<AddressSearchAuditLogDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuditLogBySearch(string searchText)
        {
            var dtos = await this.readOnlyAddressAuditLogger.GetAllBySearchTextAsync(searchText);

            return Ok(dtos);
        }
        #endregion
    }
}