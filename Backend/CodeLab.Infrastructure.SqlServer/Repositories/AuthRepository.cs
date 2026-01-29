using System.Data;
using CodeLab.Infrastructure.SqlServer.Contracts.DTOs;
using CodeLab.Infrastructure.SqlServer.Contracts.Exceptions;
using CodeLab.Infrastructure.SqlServer.Contracts.Interfaces;
using CodeLab.Infrastructure.SqlServer.Providers;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace CodeLab.Infrastructure.SqlServer.Repositories;

public class AuthRepository(CodeLabContext context) : IAuthRepository
{
    public async Task<UsuarioAutenticadoDto> IniciarSesion(string email, string clave)
    {
        using var connection = context.Database.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@email", email, DbType.String, ParameterDirection.Input);
        parameters.Add("@clave", clave, DbType.String, ParameterDirection.Input);
        parameters.Add("@mensaje", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

        var multi = await connection.QueryMultipleAsync(
            "dbo.QRY_IniciarSesion",
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