using System;
using CodeLab.Application.Interfaces;
using CodeLab.Domain.Interfaces;

namespace CodeLab.Application.AppServices;

public class AuthService(IAuthRepository authRepository) : IAuthService
{
    public Task<bool> ValidarUsuario(string email, string clave)
    {
        return authRepository.ValidarUsuario(email, clave);
    }
}