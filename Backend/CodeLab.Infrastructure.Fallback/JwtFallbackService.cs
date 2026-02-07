using CodeLab.Application.Contracts.Jwt.Interfaces;

namespace CodeLab.Infrastructure.Fallback;

public class JwtFallbackService : IJwtService
{
    public string GenerateToken(int id, List<string> roles)
    {
        throw new NotImplementedException();
    }
}