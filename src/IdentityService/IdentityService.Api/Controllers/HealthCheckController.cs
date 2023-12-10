using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Controllers;

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