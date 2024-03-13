using Microsoft.AspNetCore.Mvc;
using Rainfall.Common.Model.Response;
using Rainfall.Model.Rainfall;
using Rainfall.Model.Rainfall.Response;
using Rainfall.Service.Interface.Rainfall;
using Swashbuckle.AspNetCore.Annotations;

namespace Rainfall.API.Controllers
{
    [SwaggerTag("Operations relating to rainfall")]
    public class RainfallController : ControllerBase
    {
        private readonly IRainfallService _rainfallService;

        public RainfallController(IRainfallService rainfallService)
        {
            _rainfallService = rainfallService;
        }

        [HttpGet]
        [Route("api/[controller]/id/{stationId}/readings")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get rainfall readings by station Id", Description = "Retrieve the latest readings for the specified stationId")]
        [SwaggerResponse(200, "A list of rainfall readings successfully retrieved", typeof(rainfallReadingResponse))]
        [SwaggerResponse(400, "Invalid request", typeof(error))]
        [SwaggerResponse(404, "No readings found for the specified stationId", typeof(error))]
        [SwaggerResponse(500, "Internal server error", typeof(error))]
        public async Task<IActionResult> GetReadings(string stationId, int count = 10)
        {
            var param = new GetReadingsParam() { stationId = stationId, count = count };
            return Ok(await _rainfallService.GetReadings(param));
        }
    }
}
