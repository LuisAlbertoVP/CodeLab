using CodeLab.Application.Contracts.Database.Interfaces;
using CodeLab.Domain.Entities;
using CodeLab.Infrastructure.SqlServer.Providers;
using Microsoft.EntityFrameworkCore;

namespace CodeLab.Infrastructure.SqlServer.Repositories;

public class AuthRepository(CodeLabContext context) : IAuthRepository
{
    public Task<Usuarios?> ObtenerUsuarioPorEmail(string email)
    {
        return context.Usuarios
            .Include(u => u.UsuarioRol)
            .ThenInclude(ur => ur.Rol)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public Task<RefreshToken?> ObtenerRefreshToken(string token)
    {
        return context.RefreshToken
        .Include(rt => rt.Usuario)
        .ThenInclude(u => u.UsuarioRol)
        .ThenInclude(ur => ur.Rol)
        .FirstOrDefaultAsync(r => r.Token == token);
    }
}