using System;

namespace CodeLab.Infrastructure.Jwt.Contracts.Interfaces;

public interface IJwtService
{
    string GenerateToken(int id, string email);
}