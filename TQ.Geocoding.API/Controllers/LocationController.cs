using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TQ.Geocoding.Dtos.Address;
using TQ.Geocoding.Service.Interface.Audit;
using TQ.Geocoding.Service.Interface.Search;

namespace TQ.Geocoding.API.Controllers
{
    [Route("locations")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILogger<LocationController> _logger;
        private readonly ILocationCoordinateSearch locationCoordinateSearch;
        private readonly IReadOnlyLocationAuditLogger readOnlyLocationAuditLogger;

        /// <summary>
        /// Creates a new instance of the <see cref="LocationController"/>
        /// </summary>
        /// <param name="logger">the logger to use</param>
        /// <param name="locationSearch">the location coordinate search component to use</param>
        /// <param name="readOnlyLocationAuditLogger">the location audit logger to use</param>
        public LocationController(ILogger<LocationController> logger,
            ILocationCoordinateSearch locationSearch,
            IReadOnlyLocationAuditLogger readOnlyLocationAuditLogger)
        {
            this._logger = logger;
            this.locationCoordinateSearch = locationSearch;
            this.readOnlyLocationAuditLogger = readOnlyLocationAuditLogger;
        }

        [HttpGet("locationcoordinates")]
        public async Task<IActionResult> GetLocationCoordinatesByIdAsync([FromQuery]string id)
        {
            var dto = await this.locationCoordinateSearch.GetByIdAsync(id);

            return Ok(dto);
        }

        #region Audit Logging
        /// <summary>
        /// Returns a collection of <see cref="IEnumerable{LocationSearchAuditLogDto}"/>
        /// </summary>
        /// <returns>a <see cref="Task{IActionResult}"/>that contains a <see cref="IEnumerable{LocationSearchAuditLogDto}"/></returns>
        [HttpGet("auditlog")]
        [ProducesResponseType(typeof(IEnumerable<LocationSearchAuditLogDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuditLog()
        {
            var dtos = await this.readOnlyLocationAuditLogger.GetAllAsync();

            return Ok(dtos);
        }

        /// <summary>
        /// Returns a collection of <see cref="IEnumerable{LocationSearchAuditLogDto}"/> that match the specified dates
        /// </summary>
        /// <param name="startDate">the start date to search for</param>
        /// <param name="endDate">the end date to search for</param>
        /// <returns>a <see cref="Task{IActionResult}"/>that contains a <see cref="IEnumerable{LocationSearchAuditLogDto}"/></returns>
        [HttpGet("auditlog/by-dates")]
        [ProducesResponseType(typeof(IEnumerable<LocationSearchAuditLogDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuditLogByDates(DateTime startDate, DateTime endDate)
        {
            var dtos = await this.readOnlyLocationAuditLogger.GetAllByDateRangeAsync(startDate, endDate);

            return Ok(dtos);
        }

        /// <summary>
        /// Returns a collection of <see cref="IEnumerable{LocationSearchAuditLogDto}"/> that match the specified result counts
        /// </summary>
        /// <param name="startCount">the start count to search for</param>
        /// <param name="endCount">the end count to search for</param>
        /// <returns>a <see cref="Task{IActionResult}"/>that contains a <see cref="IEnumerable{LocationSearchAuditLogDto}"/></returns>
        [HttpGet("auditlog/by-results-count")]
        [ProducesResponseType(typeof(IEnumerable<LocationSearchAuditLogDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuditLogByResults(int startCount, int endCount)
        {
            var dtos = await this.readOnlyLocationAuditLogger.GetAllByResultCountRangeAsync(startCount, endCount);

            return Ok(dtos);
        }

        /// <summary>
        /// Returns a collection of <see cref="IEnumerable{LocationSearchAuditLogDto}"/> that match the specified search text
        /// </summary>
        /// <param name="searchText">the text to search for</param>
        /// <returns>a <see cref="Task{IActionResult}"/>that contains a <see cref="IEnumerable{LocationSearchAuditLogDto}"/></returns>
        [HttpGet("auditlog/by-Search")]
        [ProducesResponseType(typeof(IEnumerable<LocationSearchAuditLogDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuditLogBySearch(string searchText)
        {
            var dtos = await this.readOnlyLocationAuditLogger.GetAllBySearchTextAsync(searchText);

            return Ok(dtos);
        }
        #endregion
    }
}