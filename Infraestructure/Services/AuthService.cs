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
    private readonly string _adminUser = config["Admin:Username"]
        ?? throw new InvalidOperationException("Admin:Username debe estar configurado.");
    private readonly string _adminPass = config["Admin:Password"]
        ?? throw new InvalidOperationException("Admin:Password debe estar configurado.");

    public TokenResponseDto? Login(LoginDto dto)
    {
        if (dto.Username != _adminUser || dto.Password != _adminPass)
            return null;

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                config["Jwt:Key"]
                    ?? throw new InvalidOperationException("Jwt:Key debe estar configurado.")
            )
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
