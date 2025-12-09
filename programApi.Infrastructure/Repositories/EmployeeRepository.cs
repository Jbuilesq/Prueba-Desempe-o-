using Microsoft.EntityFrameworkCore;
using programApi.Domain.Entities;
using programApi.Domain.Interfaces;
using programApi.Infrastructure.Data;

namespace programApi.Infrastructure.Repositories;

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    private readonly AppDbContext _ctx;
    public EmployeeRepository(AppDbContext ctx) : base(ctx) => _ctx = ctx;

    public async Task<Employee?> GetByUserIdAsync(int userId) =>
        await _ctx.Employees.Include(e => e.Department).SingleOrDefaultAsync(e => e.Id == userId);

    public async Task<IEnumerable<Employee>> GetAllWithDepartmentAsync() =>
        await _ctx.Employees.Include(e => e.Department).ToListAsync();
}