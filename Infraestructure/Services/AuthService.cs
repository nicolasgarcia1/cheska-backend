using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Application.DTOs.Auth;
using Application.Interfaces.Services;

namespace Infrastructure.Services;

public interface IAuthService
{
    TokenResponseDto? Login(LoginDto dto);
}

public class AuthService(IConfiguration config) : IAuthService
{
    // Credenciales hardcodeadas para 1 admin (simple y efectivo)
    private readonly string _adminUser = config["Admin:Username"] ?? "admin";
    private readonly string _adminPass = config["Admin:Password"] ?? "admin123";

    public TokenResponseDto? Login(LoginDto dto)
    {
        if (dto.Username != _adminUser || dto.Password != _adminPass)
            return null;

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["Jwt:Key"] ?? "super-secret-key-minimum-32-chars!!")
        );
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddHours(8);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: [new Claim(ClaimTypes.Role, "Admin"), new Claim(ClaimTypes.Name, dto.Username)],
            expires: expires,
            signingCredentials: creds
        );

        return new TokenResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expires
        };
    }
}