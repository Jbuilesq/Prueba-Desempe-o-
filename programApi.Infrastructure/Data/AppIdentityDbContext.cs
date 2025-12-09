using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using programApi.Domain.Entities;

namespace programApi.Infrastructure.Data;

public class AppIdentityDbContext           
    : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
{
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
        : base(options) { }

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<RefreshToken>(b =>
        {
            b.HasKey(r => r.Id);
            b.Property(r => r.Token).IsRequired().HasMaxLength(256);
            b.HasOne<IdentityUser<int>>()   // sin propiedad
                .WithMany()
                .HasForeignKey("UserId")       // sombra
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
    
}