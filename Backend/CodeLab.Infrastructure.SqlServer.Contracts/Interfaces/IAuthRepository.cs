using CodeLab.Domain.Entities;
using CodeLab.Infrastructure.SqlServer.Contracts.DTOs;

namespace CodeLab.Infrastructure.SqlServer.Contracts.Interfaces;

public interface IAuthRepository
{
    Task<Usuarios?> ObtenerUsuarioPorEmail(string email);
    
    Task<UsuarioAutenticadoDto> RefrescarToken(string token);
}