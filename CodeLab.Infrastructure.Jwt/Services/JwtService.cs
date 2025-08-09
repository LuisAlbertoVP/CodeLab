using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CodeLab.Infrastructure.Jwt.Contracts.Interfaces;
using CodeLab.Infrastructure.Jwt.Contracts.Settings;
using Microsoft.IdentityModel.Tokens;

namespace CodeLab.Infrastructure.Jwt.Services;

public class JwtService(JwtSettings jwtSettings) : IJwtService
{
    public string GenerateToken(int id, List<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

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