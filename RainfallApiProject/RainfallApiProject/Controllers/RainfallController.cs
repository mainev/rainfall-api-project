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
        [Tags("Rainfall")]
        [SwaggerResponse(statusCode: 200, type: typeof(RainfallReadingResponse), description: "A list of rainfall readings successfully retrieved")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "Invalid request")]
        [SwaggerResponse(statusCode: 404, type: typeof(Error), description: "No readings found for the specified stationId")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal server error")]
        public async Task<ActionResult> GetRainfall([FromRoute] string stationId, [FromQuery][Range(1, 100)] decimal count = 10)
        {
            try
            {
                // 400
                if (String.IsNullOrEmpty(stationId) || String.IsNullOrWhiteSpace(stationId))
                {
                    var error = new Error
                    {
                        Message = "Invalid request",
                        Detail = new List<ErrorDetail> {
                            new() { PropertyName = "stationId", Message = "stationId is invalid"}
                        }
                    };

                    return StatusCode(StatusCodes.Status400BadRequest, error);
                }

                var rainfallReadings = await _rainfallService.GetRainfallMeasuresAsync((int)count, stationId);

                // 404
                if (!rainfallReadings.Readings.Any())
                    return StatusCode(StatusCodes.Status404NotFound, new Error { Message = "No readings found for the specified stationId" });
                
                // 200
                else
                    return StatusCode(StatusCodes.Status200OK, rainfallReadings);

            }
            catch (Exception ex)
            {
                // 500
                var error = new Error
                {
                    Message = ex.Message,
                    Detail = new List<ErrorDetail>
                    {
                        new ErrorDetail { Message = ex.InnerException?.Message }
                    }
                };

                return StatusCode(StatusCodes.Status500InternalServerError, error);
            }
        }
    }
}
