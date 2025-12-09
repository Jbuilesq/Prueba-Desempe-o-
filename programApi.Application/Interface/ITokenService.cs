using System.Security.Claims;

namespace programApi.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    Task<string> GenerateRefreshTokenAsync(int userId);      // ← int
    Task<bool> ValidateRefreshTokenAsync(string token, int userId); // ← int
}