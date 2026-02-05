using System.Data;
using CodeLab.Application.DTOs.Database;
using CodeLab.Application.Interfaces.Database;
using CodeLab.Domain.Entities;
using CodeLab.Infrastructure.SqlServer.Providers;
using Dapper;
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

        return usuario;
    }
}