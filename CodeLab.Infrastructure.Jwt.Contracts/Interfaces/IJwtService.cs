namespace CodeLab.Infrastructure.Jwt.Contracts.Interfaces;

public interface IJwtService
{
    string GenerateToken(int id, List<string> roles);
}