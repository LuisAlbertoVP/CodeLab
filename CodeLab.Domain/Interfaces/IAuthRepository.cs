using System;

namespace CodeLab.Domain.Interfaces;

public interface IAuthRepository
{
    Task<bool> ValidarUsuario(string email, string clave);
}
