using CodeLab.Application.Contracts.Database.DTOs;
using CodeLab.Domain.Entities;

namespace CodeLab.Application.Contracts.Database.Interfaces;

public interface IAuthRepository
{
    Task<Usuarios?> ObtenerUsuarioPorEmail(string email);
    
    Task<UsuarioAutenticadoDto> RefrescarToken(string token);
}