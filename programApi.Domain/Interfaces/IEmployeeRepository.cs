using programApi.Domain.Entities;

namespace programApi.Domain.Interfaces;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<Employee?> GetByUserIdAsync(int userId);
    Task<IEnumerable<Employee>> GetAllWithDepartmentAsync();
}