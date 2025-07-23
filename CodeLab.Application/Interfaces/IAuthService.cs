using System;

namespace CodeLab.Application.Interfaces;

public interface IAuthService
{
    Task<bool> ValidarUsuario(string email, string clave);
}
