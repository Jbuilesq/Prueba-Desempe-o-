using programApi.Domain.Entities;

namespace programApi.Application.Interface;

public interface IEmployeeService
{
    Task<Employee> CreateAsync(Employee employee);
    Task<Employee?> GetByUserIdAsync(int userId);
    Task<IEnumerable<Employee>> GetAllAsync();
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(int id);
}