using Microsoft.EntityFrameworkCore;
using programApi.Domain.Entities;
using System.Reflection;
using Microsoft.AspNetCore.Identity;

namespace programApi.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Employee> Employees { set; get; }
    public DbSet<Department> Departments { set; get; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);   // ← indispensable

        builder.Entity<Employee>()
            .HasOne<IdentityUser<int>>()
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}