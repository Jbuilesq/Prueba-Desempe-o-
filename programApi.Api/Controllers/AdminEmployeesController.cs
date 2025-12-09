using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using programApi.Application.DTOs;
using programApi.Application.Interface;
using programApi.Application.Interfaces;
using programApi.Domain.Entities;

namespace programApi.Api.Controllers;

[ApiController]
[Route("api/admin/employees")]
[Authorize(Roles = "Admin")]
public class AdminEmployeesController : ControllerBase
{
    private readonly IEmployeeService _empService;
    public AdminEmployeesController(IEmployeeService empService) => _empService = empService;

    [HttpGet]
    public async Task<IActionResult> List() => Ok(await _empService.GetAllAsync());

    [HttpPut("{id}/complete")]
    public async Task<IActionResult> Complete(int id, [FromBody] EmployeeCompleteDto dto)
    {
        var emp = await _empService.GetByUserIdAsync(id); // o FindByIdAsync
        if (emp is null) return NotFound();
        // Mapear dto → emp
        emp.BirthDate = dto.BirthDate;
        emp.Address = dto.Address;
        emp.Position = dto.Position;
        emp.Salary = dto.Salary;
        emp.HireDate = dto.HireDate;
        emp.EducationLevel = dto.EducationLevel;
        emp.ProfessionalProfile = dto.ProfessionalProfile;
        emp.DepartmentId = dto.DepartmentId;
        emp.Status = "Active";
        await _empService.UpdateAsync(emp);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _empService.DeleteAsync(id);
        return NoContent();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EmployeeCreateDto dto)
    {
        var employee = new Employee
        {
            Document = dto.Document,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            BirthDate = dto.BirthDate,
            Address = dto.Address,
            Phone = dto.Phone,
            Email = dto.Email,
            Position = dto.Position,
            Salary = dto.Salary,
            HireDate = dto.HireDate,
            Status = dto.Status,
            EducationLevel = dto.EducationLevel,
            ProfessionalProfile = dto.ProfessionalProfile,
            DepartmentId = dto.DepartmentId,
            UserId = null // ← se asignará después si se enlaza a usuario
        };

        var created = await _empService.CreateAsync(employee);
        return CreatedAtAction(nameof(List), new { id = created.Id }, created);
    }
}