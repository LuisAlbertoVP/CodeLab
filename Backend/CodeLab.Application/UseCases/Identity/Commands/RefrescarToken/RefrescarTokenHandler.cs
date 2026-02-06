using CodeLab.Application.Contracts.Database.Interfaces;
using CodeLab.Application.Contracts.Jwt.Interfaces;
using CodeLab.Application.Contracts.Logging.Interfaces;
using CodeLab.Application.Shared.Common;
using CodeLab.Application.Shared.Results;

namespace CodeLab.Application.UseCases.Identity.Commands.RefrescarToken;

public class RefrescarTokenHandler(
    IAuthRepository authRepository,
    IJwtService jwtService,
    ICodeLabLogger logger
) : IRequestHandler<RefrescarTokenCommand, CodeLabResultado<LoginResultDTO>>
{
    public async Task<CodeLabResultado<LoginResultDTO>> Handle(RefrescarTokenCommand request, CancellationToken ct)
    {
        try
        {
            var usuarioAutenticado = await authRepository.RefrescarToken(request.RefreshToken);
            var token = jwtService.GenerateToken(usuarioAutenticado.Id, usuarioAutenticado.Roles);
            logger.LogInformation($"Refresh token exitoso para el usuario con ID '{usuarioAutenticado.Id}'.");
            var loginResult = new LoginResultDTO(
                Token: token,
                RefreshToken: usuarioAutenticado.RefreshToken,
                Expiration: usuarioAutenticado.FechaExpiracion
            );
            return CodeLabResultado<LoginResultDTO>.Exito(loginResult);
        }
        catch (Exception ex)
        {
            logger.LogWarning($"Fallo al refrescar token: {ex.Message}");
            return CodeLabResultado<LoginResultDTO>.Error(ex.Message);
        }
    }
}