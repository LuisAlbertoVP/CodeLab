namespace CodeLab.Application.Interfaces.Jwt;

public interface IJwtService
{
    string GenerateToken(int id, List<string> roles);
}