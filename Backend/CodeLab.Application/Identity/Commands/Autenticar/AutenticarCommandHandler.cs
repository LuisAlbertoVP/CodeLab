using CodeLab.Application.Shared.Common;
using CodeLab.Application.Shared.Results;
using CodeLab.Infrastructure.Jwt.Contracts.Interfaces;
using CodeLab.Infrastructure.Logging.Contracts.Interfaces;
using CodeLab.Infrastructure.SqlServer.Contracts.Exceptions;
using CodeLab.Infrastructure.SqlServer.Contracts.Interfaces;

namespace CodeLab.Application.Identity.Commands.Autenticar;

public class AutenticarCommandHandler(
    IAuthRepository authRepository,
    IJwtService jwtService,
    ICodeLabLogger logger
) : IRequestHandler<AutenticarCommand, CodeLabResultado<LoginResultDTO>>
{
    public async Task<CodeLabResultado<LoginResultDTO>> Handle(AutenticarCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var usuarioAutenticado = await authRepository.IniciarSesion(request.Email, request.Clave);
            var token = jwtService.GenerateToken(usuarioAutenticado.Id, usuarioAutenticado.Roles);
            logger.LogInformation($"Usuario con ID '{usuarioAutenticado.Id}' autenticado correctamente.");
            var loginResult = new LoginResultDTO(
                Token: token,
                RefreshToken: usuarioAutenticado.RefreshToken,
                Expiration: usuarioAutenticado.FechaExpiracion
            );
            return CodeLabResultado<LoginResultDTO>.Exito(loginResult);
        }
        catch (AuthException ex)
        {
            logger.LogWarning($"Error de autenticación: {ex.Message}");
            return CodeLabResultado<LoginResultDTO>.Error(ex.Message);
        }
    }
}