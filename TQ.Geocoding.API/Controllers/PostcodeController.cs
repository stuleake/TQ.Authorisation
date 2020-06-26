using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TQ.Geocoding.Service.Interface.Search;

namespace TQ.Geocoding.API.Controllers
{
    [Route("postcodes")]
    [ApiController]
    public class PostcodeController : ControllerBase
    {
        private readonly ILogger<PostcodeController> _logger;
        private readonly ILocationPostcodeCoordinateSearch locationPostcodeCoordinateSearch;

        /// <summary>
        /// Creates a new instance of the <see cref="PostcodeController"/>
        /// </summary>
        /// <param name="logger">the logger to use</param>
        /// <param name="locationSearch">the postcode coordinate search component to use</param>
        public PostcodeController(ILogger<PostcodeController> logger, ILocationPostcodeCoordinateSearch locationSearch)
        {
            this._logger = logger;
            this.locationPostcodeCoordinateSearch = locationSearch;
        }

        [HttpGet("postcodecoordinates")]
        public async Task<IActionResult> GetPostcodeCoordinatesByIdAsync([FromQuery]string id)
        {
            var dto = await this.locationPostcodeCoordinateSearch.GetByIdAsync(id);

            return Ok(dto);
        }
    }
}