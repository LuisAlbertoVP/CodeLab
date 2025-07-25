using System;
using CodeLab.Domain.DTOs;

namespace CodeLab.Domain.Interfaces;

public interface IAuthRepository
{
    Task<UsuarioAutenticadoDto> IniciarSesion(string email, string clave);
}