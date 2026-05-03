namespace RawWorld.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using RawWorld.API.DTOs;
using RawWorld.API.Services;

[ApiController, Route("api/auth")]
public class AuthController(IAuthService svc) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var res = await svc.RegisterAsync(dto);
        return res == null ? Conflict("Email already exists") : Ok(res);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var res = await svc.LoginAsync(dto);
        return res == null ? Unauthorized("Invalid credentials") : Ok(res);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshDto dto)
    {
        var res = await svc.RefreshAsync(dto.RefreshToken);
        return res == null ? Unauthorized("Invalid or expired token") : Ok(res);
    }
}