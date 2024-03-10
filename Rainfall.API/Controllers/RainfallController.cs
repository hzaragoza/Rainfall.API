using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Rainfall.API.Controllers
{
    public class RainfallController : ControllerBase
    {
        [HttpGet]
        [Route("api/[controller]/id/{stationId}/readings")]
        public async Task<IActionResult> GetReadings(int stationId, int count=10)
        {
            return Ok(true);
        }
    }
}
