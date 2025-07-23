using System;
using CodeLab.Domain.Interfaces;
using CodeLab.Infrastructure.SqlServer.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeLab.Infrastructure.SqlServer.Repositories;

public class AuthRepository(CodeLabContext context) : IAuthRepository
{
    public Task<bool> ValidarUsuario(string email, string clave)
    {
        return context.Usuarios.AnyAsync(u => u.Email == email && u.Clave == clave);
    }
}