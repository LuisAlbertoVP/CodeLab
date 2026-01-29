using System.Data;
using CodeLab.Domain.Entities;
using CodeLab.Infrastructure.SqlServer.Contracts.DTOs;
using CodeLab.Infrastructure.SqlServer.Contracts.Exceptions;
using CodeLab.Infrastructure.SqlServer.Contracts.Interfaces;
using CodeLab.Infrastructure.SqlServer.Providers;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace CodeLab.Infrastructure.SqlServer.Repositories;

public class AuthRepository(CodeLabContext context) : IAuthRepository
{
    public Task<Usuarios?> ObtenerUsuarioValido(string email, string clave)
    {
        return context.Usuarios.FirstOrDefaultAsync(u => u.Email == email && u.Clave == clave);
    }

    public Task<List<string?>> ObtenerRolesUsuario(long idUsuario)
    {
        return (from rol in context.Roles
                join usuariorol in context.UsuarioRol
                on rol.Id equals usuariorol.IdRol
                where usuariorol.IdUsuario == idUsuario
                select
                    rol.Codigo
                ).ToListAsync();
    }

    public async Task GuardarUsuario(Usuarios usuario)
    {
        context.Usuarios.Update(usuario);
        await context.SaveChangesAsync();
    }

    public async Task<UsuarioAutenticadoDto> RefrescarToken(string token)
    {
        using var connection = context.Database.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@token", token, DbType.String, ParameterDirection.Input);
        parameters.Add("@mensaje", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

        var multi = await connection.QueryMultipleAsync(
            "dbo.QRY_ValidarRefreshToken",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        var usuario = await multi.ReadFirstOrDefaultAsync<UsuarioAutenticadoDto>();
        if (usuario != null)
        {
            usuario.Roles = (await multi.ReadAsync<string>()).ToList();
        }

        var outputValue = parameters.Get<string>("@mensaje");
        if (!string.IsNullOrEmpty(outputValue))
        {
            throw new AuthException(outputValue);
        }

        return usuario;
    }
}