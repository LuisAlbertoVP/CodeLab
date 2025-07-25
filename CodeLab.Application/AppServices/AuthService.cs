using System;
using CodeLab.Application.Interfaces;
using CodeLab.Domain.Interfaces;
using CodeLab.Infrastructure.Jwt.Contracts.Interfaces;

namespace CodeLab.Application.AppServices;

public class AuthService(
    IAuthRepository authRepository,
    IJwtService jwtService
) : IAuthService
{
    public async Task<string> IniciarSesion(string email, string clave)
    {
        var usuarioAutenticado = await authRepository.IniciarSesion(email, clave);
        return jwtService.GenerateToken(usuarioAutenticado.Id, email);
    }
}