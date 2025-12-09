using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace programApi.Api.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]  
public class AdminController : ControllerBase
{
    [HttpGet("dashboard")]
    public IActionResult Dashboard() => Ok(new { message = "Admin Dashboard" });
}