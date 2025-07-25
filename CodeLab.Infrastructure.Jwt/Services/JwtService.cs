using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CodeLab.Infrastructure.Jwt.Contracts.DTOs;
using CodeLab.Infrastructure.Jwt.Contracts.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace CodeLab.Infrastructure.Jwt.Services;

public class JwtService(JwtSettingsDto jwtSettings) : IJwtService
{
    public string GenerateToken(int id, string email)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtSettings.ExpiryMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}