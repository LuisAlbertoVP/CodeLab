using CodeLab.Application.Identity.Events.UsuarioAutenticadoExito;
using CodeLab.Application.Identity.Events.UsuarioNoAutenticado;
using CodeLab.Application.Identity.Interfaces;
using CodeLab.Application.Shared.Common;
using CodeLab.Application.Shared.Results;
using CodeLab.Infrastructure.SqlServer.Contracts.Exceptions;
using CodeLab.Infrastructure.SqlServer.Contracts.Interfaces;

namespace CodeLab.Application.Identity.Commands.Autenticar;

public class AutenticarCommandHandler(
    IAuthRepository authRepository,
    ITokenService tokenService,
    IMediator mediator
) : IRequestHandler<AutenticarCommand, CodeLabResultado<LoginResultDTO>>
{
    public async Task<CodeLabResultado<LoginResultDTO>> Handle(AutenticarCommand request, CancellationToken ct)
    {
        try
        {
            var usuario = await authRepository.ObtenerUsuarioValido(request.Email, request.Clave) ??
                throw new Exception("Credenciales incorrectas");

            var refreshToken = tokenService.GenerarRefreshToken();
            usuario.RefreshTokens = [refreshToken];

            await authRepository.GuardarUsuario(usuario);

            var roles = await authRepository.ObtenerRolesUsuario(usuario.Id);
            var token = tokenService.GenerarToken(usuario.Id, roles);
            
            var usuarioAutenticadoExito = new UsuarioAutenticadoExitoEvent(usuario.Id);
            await mediator.Publish(usuarioAutenticadoExito, ct);

            var loginResult = new LoginResultDTO(
                Token: token,
                RefreshToken: refreshToken.Token,
                Expiration: refreshToken.FechaExpiracion
            );
            return CodeLabResultado<LoginResultDTO>.Exito(loginResult);
        }
        catch (AuthException ex)
        {
            var usuarioNoAutenticado = new UsuarioNoAutenticadoEvent(request.Email, ex);
            await mediator.Publish(usuarioNoAutenticado, ct);

            return CodeLabResultado<LoginResultDTO>.Error(ex.Message);
        }
    }
}