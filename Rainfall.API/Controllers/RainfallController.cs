using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rainfall.Common.Model.Middleware.ExceptionHandling;
using Rainfall.Model.Rainfall;
using Rainfall.Service.Interface.Rainfall;
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

        [HttpGet]
        [Route("api/[controller]/id/{stationId}/readings")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetReadings(string stationId, int count=10)
        {
            var param = new GetReadingsParam() { stationId=stationId, count=count };
            return Ok(await _rainfallService.GetReadings(param));
        }
    }
}
