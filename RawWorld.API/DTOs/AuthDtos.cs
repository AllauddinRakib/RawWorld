namespace RawWorld.API.DTOs;

public record RegisterDto(string FullName, string Email, string Password);
public record LoginDto(string Email, string Password);
public record AuthResponseDto(string AccessToken, string RefreshToken, string Role);
public record RefreshDto(string RefreshToken);