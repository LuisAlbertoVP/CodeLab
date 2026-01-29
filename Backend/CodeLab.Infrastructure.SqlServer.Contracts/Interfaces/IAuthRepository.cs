using CodeLab.Domain.Entities;
using CodeLab.Infrastructure.SqlServer.Contracts.DTOs;

namespace CodeLab.Infrastructure.SqlServer.Contracts.Interfaces;

public interface IAuthRepository
{
    Task<Usuarios?> ObtenerUsuarioValido(string email, string clave);

    Task<List<string?>> ObtenerRolesUsuario(long idUsuario);
    
    Task GuardarUsuario(Usuarios usuario);

    Task<UsuarioAutenticadoDto> RefrescarToken(string token);
}