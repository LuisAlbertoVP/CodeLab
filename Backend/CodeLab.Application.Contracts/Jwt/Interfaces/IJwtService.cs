namespace CodeLab.Application.Contracts.Jwt.Interfaces;

public interface IJwtService
{
    string GenerateToken(int id, List<string> roles);
}