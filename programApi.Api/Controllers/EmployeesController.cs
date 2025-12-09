using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace programApi.Api.Controllers;

[ApiController]
[Route("api/employees")]
[Authorize(Roles = "User")]   // <-- solo empleados
public class EmployeesController : ControllerBase
{
    [HttpGet("me")]
    public IActionResult Me() => Ok(new { message = "Mis datos" });
}