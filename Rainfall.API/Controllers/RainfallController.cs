using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rainfall.Common.Model.Middleware.ExceptionHandling;
using Rainfall.Model.Rainfall;
using Rainfall.Service.Interface.Rainfall;
using System.ComponentModel;
using System.Net.Mime;

namespace Rainfall.API.Controllers
{
    public class RainfallController : ControllerBase
    {
        private readonly IRainfallService _rainfallService;

        public RainfallController(IRainfallService rainfallService)
        {
            _rainfallService = rainfallService;
        }

        /// <summary>
        /// Get rainfall readings by station Id
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/id/{stationId}/readings")]
        //[Tags(new string[] { "get-rainfall", "get-rainfall2" })]
        [EndpointSummary("Get rainfall readings by station Id")]
        [Description("Retrieve the latest readings for the specified stationId")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(StationReadingResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReadings(string stationId, int count = 10)
        {
            var param = new GetReadingsParam() { stationId = stationId, count = count };
            return Ok(await _rainfallService.GetReadings(param));
        }
    }
}
