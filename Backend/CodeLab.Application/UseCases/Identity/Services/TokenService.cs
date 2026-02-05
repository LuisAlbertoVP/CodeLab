using System.Security.Cryptography;
using System.Text;
using CodeLab.Application.Interfaces.Jwt;
using CodeLab.Application.UseCases.Identity.Interfaces;
using CodeLab.Domain.Entities;

namespace CodeLab.Application.UseCases.Identity.Services;

public class TokenService(IJwtService jwtService) : ITokenService
{
    public RefreshToken GenerarRefreshToken()
    {
        Guid rawToken = Guid.NewGuid();
        byte[] rawBytes = Encoding.UTF8.GetBytes(rawToken.ToString());
        byte[] hashBytes;
        using (SHA256 sha256 = SHA256.Create())
        {
            hashBytes = sha256.ComputeHash(rawBytes);
        }
        string tokenString = Convert.ToHexString(hashBytes).ToLower();
        DateTime fechaExpiracion = DateTime.Now.AddDays(3);
        var refreshToken = new RefreshToken
        {
            Id = rawToken,
            Token = tokenString,
            FechaExpiracion = fechaExpiracion,
            FechaCreacion = DateTime.UtcNow
        };
        return refreshToken;
    }

    public string GenerarToken(int idUsuario, List<string> roles)
    {
        return jwtService.GenerateToken(idUsuario, roles);
    }
}
