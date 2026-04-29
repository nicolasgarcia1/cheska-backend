using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Auth;
using Infrastructure.Services;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var result = authService.Login(dto);
        if (result is null) return Unauthorized(new { message = "Credenciales inválidas" });
        return Ok(result);
    }
}