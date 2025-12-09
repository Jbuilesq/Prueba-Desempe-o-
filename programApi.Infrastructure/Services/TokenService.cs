using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using programApi.Application.Interfaces;
using programApi.Domain.Entities;
using programApi.Infrastructure.Data;

namespace programApi.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly UserManager<IdentityUser<int>> _userManager;
    private readonly AppIdentityDbContext _ctx;

    public TokenService(IConfiguration config,
                        UserManager<IdentityUser<int>> userManager,
                        AppIdentityDbContext ctx)
    {
        _config = config;
        _userManager = userManager;
        _ctx = ctx;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _config["JwtSettings:Secret"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                int.Parse(_config["JwtSettings:ExpiryMinutes"] ?? "60")),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> GenerateRefreshTokenAsync(int userId)
    {
        var token = Guid.NewGuid().ToString();
        var rt = new RefreshToken { Token = token, Expires = DateTime.UtcNow.AddDays(7) };
        _ctx.RefreshTokens.Add(rt);
        _ctx.Entry(rt).Property("UserId").CurrentValue = userId; // sombra
        await _ctx.SaveChangesAsync();
        return token;
    }

    public async Task<bool> ValidateRefreshTokenAsync(string token, int userId)
    {
        var rt = await _ctx.RefreshTokens
            .Where(r => EF.Property<int>(r, "UserId") == userId &&
                        r.Token == token)
            .SingleOrDefaultAsync();
        return rt != null && !rt.IsExpired;
    }
}