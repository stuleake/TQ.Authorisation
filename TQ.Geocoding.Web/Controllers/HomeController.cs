using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TQ.Geocoding.Dto.Dtos;
using TQ.Geocoding.Dto.Requests;
using TQ.Geocoding.Web.Models;

namespace TQ.Geocoding.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string apiBaseUrl;

        private static HttpClient client = new HttpClient();

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
            var appSettingsSection = _configuration.GetSection("AppSettings");
            apiBaseUrl = appSettingsSection["apiBaseUrl"];

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FindAnAddress()
        {
            return View();
        }

        public IActionResult Counts()
        {
            return View();
        }

        [Route("Home/Detail/{uprn}")]
        public async Task<IActionResult> Detail(string uprn)
        {
            using (HttpClient client = new HttpClient())
            {
                // Request headers
                var appSettingsSection = _configuration.GetSection("AppSettings");
                var subscriptionKey = appSettingsSection["unlimitedSubscriptionKey"];
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{subscriptionKey}");
                var apiBaseUrl = appSettingsSection["apiBaseUrl"];

                // Request parameters
                var uri = string.Format("{0}/api/Address/ByUprn/{1}/full", apiBaseUrl, uprn);

                HttpResponseMessage response = null;
                try
                {
                    response = await client.GetAsync(uri);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();



                    return View(JsonConvert.DeserializeObject<FullAddressDto>(responseBody));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [HttpGet("Home/FreeTextSearch/{preferWelsh}/{dtoType}")]
        public async Task<ContentResult> FreeTextSearch([FromQuery]string searchString, bool preferWelsh, string dtoType)
        {
            var request = new GetAddressByTextRequest { PageSize = 200, SkipPages = 10, SearchString = searchString };
            var statusCode = (int)System.Net.HttpStatusCode.OK;

            HttpResponseMessage response = await client.PostAsJsonAsync(this.BuildUri("ByText", preferWelsh, dtoType), request);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                statusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            }

            var content = await response.Content.ReadAsStringAsync();
            return new ContentResult {  Content = content, ContentType = "application/json", StatusCode = statusCode };
        }

        [HttpGet("Home/PostcodeSearch/{preferWelsh}/{dtoType}")]
        public async Task<ContentResult> PostcodeSearch([FromQuery]string postcode, bool preferWelsh, string dtoType)
        {
            var statusCode = (int)System.Net.HttpStatusCode.OK;
            var queryString = new Dictionary<string, string>()
            {
                { "postcode", HttpUtility.UrlEncode(postcode) }
            };

            var uri = $"{apiBaseUrl}/api/Address/ByPostcode/{dtoType}";
            if (preferWelsh)
            {
                uri += "/welsh";
            }

            uri = QueryHelpers.AddQueryString(uri, queryString);
            HttpResponseMessage response = await client.GetAsync(uri);
            try
            {
                response.EnsureSuccessStatusCode();
            } 
            catch(Exception)
            {
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                statusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            }

            var content = await response.Content.ReadAsStringAsync();
            return new ContentResult { Content = content, ContentType = "application/json", StatusCode = statusCode };
        }

        [HttpGet("Home/CoordinateSearch/{xcoordinate}/{ycoordinate}/{preferWelsh}/{radius}")]
        public async Task<string> CoordinateSearch(decimal xCoordinate, decimal yCoordinate, bool preferWelsh, int radius)
        {
            var uri = $"{apiBaseUrl}/api/Address/ByCoords/simple";
            if (preferWelsh)
            {
                uri += "/welsh";
            }

            var request = new GetAddressByEastingNorthingRequest { XCoordinateEasting = xCoordinate, YCoordinateNorthing = yCoordinate, Radius = radius };
            HttpResponseMessage response = await client.PostAsJsonAsync(uri, request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        [HttpGet("Home/CoordinateSearch/{xcoordinate}/{ycoordinate}/{preferWelsh}")]
        public async Task<string> CoordinateSearch(decimal xCoordinate, decimal yCoordinate, bool preferWelsh)
        {
            var uri = $"{apiBaseUrl}/api/Address/ByCoords/simple";
            if (preferWelsh)
            {
                uri += "/welsh";
            }

            var request = new GetAddressByEastingNorthingRequest { XCoordinateEasting = xCoordinate, YCoordinateNorthing = yCoordinate };
            HttpResponseMessage response = await client.PostAsJsonAsync(uri, request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        [HttpGet("Home/UprnSearch/{uprn}/{preferWelsh}/{dtoType}")]
        public async Task<string> UprnSearch(string uprn, bool preferWelsh, string dtoType)
        {
            var uri = $"{apiBaseUrl}/api/Address/ByUprn/{uprn}/{dtoType}";
            if (preferWelsh)
            {
                uri += "/welsh";
            }

            HttpResponseMessage response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        [HttpGet("Home/PostcodeCount/{postcodeString}")]
        public async Task<int> PostcodeCount(string postcodeString, [FromQuery]string searchString="")
        {
            using (HttpClient client = new HttpClient())
            {
                var queryString = HttpUtility.ParseQueryString(string.Empty);

                // Request headers
                var appSettingsSection = _configuration.GetSection("AppSettings");
                var subscriptionKey = appSettingsSection["unlimitedSubscriptionKey"];
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{subscriptionKey}");
                var apiBaseUrl = appSettingsSection["apiBaseUrl"];

                // Request parameters
                queryString["searchString"] = searchString ?? "";
                var encodedPostcodeString = Uri.EscapeUriString(postcodeString);

                var uri = string.Format("{0}/api/Postcode/Count/{1}?{2}", apiBaseUrl, encodedPostcodeString, queryString);

                HttpResponseMessage response = null;
                try
                {
                    response = await client.GetAsync(uri);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return Convert.ToInt32(responseBody);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string BuildUri(string searchType, bool preferWelsh, string dtoType)
        {
            var uri = $"{apiBaseUrl}/api/Address/{searchType}/{dtoType}";

            if (preferWelsh)
            {
                uri += "/welsh";
            }

            return uri;
        }
    }
}
