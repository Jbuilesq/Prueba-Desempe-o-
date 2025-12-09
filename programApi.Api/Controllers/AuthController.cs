using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using programApi.Application.DTOs;
using programApi.Application.Interfaces;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace programApi.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser<int>> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _config;

    public AuthController(UserManager<IdentityUser<int>> userManager,
                          ITokenService tokenService,
                          IConfiguration config)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest dto)
    {
        var user = new IdentityUser<int> { UserName = dto.Email, Email = dto.Email };
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        // Aquí después crearemos el Employee
        return Ok(new { userId = user.Id });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            return Unauthorized("Invalid credentials");

        var claims = await _userManager.GetClaimsAsync(user);
        var accessToken = _tokenService.GenerateAccessToken(claims);
        var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id);

        return Ok(new AuthResponse(accessToken, refreshToken));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] AuthResponse dto)
    {
        var principal = GetPrincipalFromExpiredToken(dto.AccessToken);
        if (principal == null) return Unauthorized();

        var userId = int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        if (!await _tokenService.ValidateRefreshTokenAsync(dto.RefreshToken, userId))
            return Unauthorized();

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
        var newRefreshToken = await _tokenService.GenerateRefreshTokenAsync(userId);

        return Ok(new AuthResponse(newAccessToken, newRefreshToken));
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JwtSettings:Secret"]!)),
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
            return principal;
        }
        catch { return null; }
    }
}