using Microsoft.AspNetCore.Mvc;
using RainfallApiProject.Models;
using RainfallApiProject.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace RainfallApiProject.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    public class RainfallController : ControllerBase
    {
        private readonly RainfallService _rainfallService;


        public RainfallController(RainfallService rainfallService)
        {
            _rainfallService = rainfallService; 
        }

        /// <summary>
        /// Get rainfall readings by station Id
        /// </summary>
        /// <remarks>Retrieve the latest readings for the specified stationId</remarks>
        /// <param name="stationId">The id of the reading station</param>
        /// <param name="count">The number of readings to return</param>
        /// <response code="200">A list of rainfall readings successfully retrieved</response>
        /// <response code="400">Invalid request</response>
        /// <response code="404">No readings found for the specified stationId</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [Route("/rainfall/id/{stationId}/readings")]
        [SwaggerOperation("GetRainfall")]
        [SwaggerResponse(statusCode: 200, type: typeof(RainfallReadingResponse), description: "A list of rainfall readings successfully retrieved")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "Invalid request")]
        [SwaggerResponse(statusCode: 404, type: typeof(Error), description: "No readings found for the specified stationId")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal server error")]
        public ActionResult<RainfallReadingResponse> GetRainfall(string stationId, [FromQuery][Range(1, 100)] int? count = 10)
        {
            
            try
            {
                return _rainfallService.GetRainfallMeasuresAsync().GetAwaiter().GetResult();
            }catch(Exception ex) {

                return StatusCode(500, default(Error));
            }
        }
    }
}
