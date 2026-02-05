using CodeLab.Application.DTOs.Database;
using CodeLab.Domain.Entities;

namespace CodeLab.Application.Interfaces.Database;

public interface IAuthRepository
{
    Task<Usuarios?> ObtenerUsuarioPorEmail(string email);
    
    Task<UsuarioAutenticadoDto> RefrescarToken(string token);
}