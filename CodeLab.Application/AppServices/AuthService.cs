using CodeLab.Application.Interfaces;
using CodeLab.Application.Results;
using CodeLab.Domain.Exceptions;
using CodeLab.Domain.Interfaces;
using CodeLab.Infrastructure.Jwt.Contracts.Interfaces;
using CodeLab.Infrastructure.Logging.Contracts.Interfaces;

namespace CodeLab.Application.AppServices;

public class AuthService(
    IAuthRepository authRepository,
    IJwtService jwtService,
    ICodeLabLogger logger
) : IAuthService
{
    public async Task<CodeLabResultado<string>> IniciarSesion(string email, string clave)
    {
        try
        {
            var usuarioAutenticado = await authRepository.IniciarSesion(email, clave);
            var token = jwtService.GenerateToken(usuarioAutenticado.Id, email);
            logger.LogInformation($"Usuario {email} autenticado correctamente.");
            return CodeLabResultado<string>.Exito(token);
        }
        catch (AuthException ex)
        {
            logger.LogWarning($"Error de autenticación: {ex.Message}");
            return CodeLabResultado<string>.Error(ex.Message);
        }
    }
}