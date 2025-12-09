using Microsoft.EntityFrameworkCore;
using programApi.Domain.Interfaces;
using programApi.Infrastructure.Data;

namespace programApi.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _ctx;
    public Repository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<T>> GetAllAsync() => await _ctx.Set<T>().ToListAsync();
    public async Task<T?> FindByIdAsync(int id) => await _ctx.Set<T>().FindAsync(id);
    public async Task<T> AddAsync(T entity) { await _ctx.Set<T>().AddAsync(entity); return entity; }
    public Task<T> UpdateAsync(T entity) { _ctx.Set<T>().Update(entity); return Task.FromResult(entity); }
    public Task DeleteAsync(T entity) { _ctx.Set<T>().Remove(entity); return Task.CompletedTask; }
    public Task SaveChangesAsync() => _ctx.SaveChangesAsync();
}