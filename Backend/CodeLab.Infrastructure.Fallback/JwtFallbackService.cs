using CodeLab.Application.Contracts.Fallback.Attributes;
using CodeLab.Application.Contracts.Jwt.Interfaces;

namespace CodeLab.Infrastructure.Fallback;

[Fallback]
public class JwtFallbackService : IJwtService
{
    public string GenerateToken(int id, List<string> roles)
    {
        throw new NotImplementedException();
    }
}