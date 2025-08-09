using CodeLab.Infrastructure.SqlServer.Contracts.DTOs;

namespace CodeLab.Infrastructure.SqlServer.Contracts.Interfaces;

public interface IAuthRepository
{
    Task<UsuarioAutenticadoDto> IniciarSesion(string email, string clave);

    Task<UsuarioAutenticadoDto> RefrescarToken(string token);
}