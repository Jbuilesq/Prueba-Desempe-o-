using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using programApi.Application.Interfaces;
using System.Security.Claims;
using programApi.Application.DTOs;
using programApi.Application.Interface;

namespace programApi.Api.Controllers;

[ApiController]
[Route("api/employees")]
[Authorize(Roles = "User")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _empService;
    public EmployeesController(IEmployeeService empService) => _empService = empService;

    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var emp = await _empService.GetByUserIdAsync(userId);
        return emp is null ? NotFound() : Ok(emp);
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe([FromBody] EmployeeUpdateDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var emp = await _empService.GetByUserIdAsync(userId);
        if (emp is null) return NotFound();
        // Solo campos que puede editar él mismo
        emp.Phone = dto.Phone;
        emp.Address = dto.Address;
        emp.ProfessionalProfile = dto.ProfessionalProfile;
        await _empService.UpdateAsync(emp);
        return NoContent();
    }
}