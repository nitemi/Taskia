using System;
using Microsoft.AspNetCore.Mvc;

namespace Taskia.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult GetHealth()
    {
        return Ok(new
        {
            status = "Healthy",
            timestamp = DateTime.UtcNow,
            service = "Taskia.Api",
            version = "1.0.0"
        });
    }
}
