using Microsoft.AspNetCore.Mvc;

namespace EventDriven.Domain.PoC.Api.Rest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet("")]
        [HttpHead("")]
        public IActionResult Ping()
        {
            return Ok();
        }
    }
}