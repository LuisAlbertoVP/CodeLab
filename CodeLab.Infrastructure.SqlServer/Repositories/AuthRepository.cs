using System.Data;
using CodeLab.Domain.DTOs;
using CodeLab.Domain.Exceptions;
using CodeLab.Domain.Interfaces;
using CodeLab.Infrastructure.SqlServer.Data;
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

        var resultados = await connection.QueryAsync<UsuarioAutenticadoDto>(
            "dbo.QRY_IniciarSesion",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        var outputValue = parameters.Get<string>("@mensaje");
        if (!string.IsNullOrEmpty(outputValue))
        {
            throw new AuthException(outputValue);
        }

        return resultados.FirstOrDefault();
    }
}