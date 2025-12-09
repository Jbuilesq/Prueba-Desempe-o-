namespace programApi.Application.DTOs;

public record RegisterRequest(string Email, string Password, string Document, string FirstName, string LastName);
public record LoginRequest(string Email, string Password);
public record AuthResponse(string AccessToken, string RefreshToken);