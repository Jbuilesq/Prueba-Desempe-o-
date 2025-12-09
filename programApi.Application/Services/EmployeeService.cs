using programApi.Application.Interface;
using programApi.Domain.Entities;
using programApi.Domain.Interfaces;
using programApi.Application.Interfaces;

namespace programApi.Application.Services;

public class EmployeeService : IEmployeeService
{
    public async Task<Employee> CreateAsync(Employee employee)
    {
        await _repo.AddAsync(employee);
        await _repo.SaveChangesAsync();
        return employee;
    }
    
    private readonly IEmployeeRepository _repo;
    public EmployeeService(IEmployeeRepository repo) => _repo = repo;

    public async Task<Employee?> GetByUserIdAsync(int userId) =>
        await _repo.GetByUserIdAsync(userId);

    public async Task<IEnumerable<Employee>> GetAllAsync() =>
        await _repo.GetAllWithDepartmentAsync();

    public async Task UpdateAsync(Employee employee) =>
        await _repo.UpdateAsync(employee);

    public async Task DeleteAsync(int id)
    {
        var emp = await _repo.FindByIdAsync(id);
        if (emp is null) throw new KeyNotFoundException();
        await _repo.DeleteAsync(emp);
        await _repo.SaveChangesAsync();
    }
}