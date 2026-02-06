using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CodeLab.Application.Contracts.Jwt.Interfaces;
using CodeLab.Application.Contracts.Providers.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace CodeLab.Infrastructure.Jwt.Services;

public class JwtService(IConfigJwtProvider configJwtProvider) : IJwtService
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

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configJwtProvider.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configJwtProvider.Issuer,
            audience: configJwtProvider.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(configJwtProvider.ExpiryMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}