using System;

namespace CodeLab.Application.Interfaces;

public interface IAuthService
{
    Task<string> IniciarSesion(string email, string clave);
}
