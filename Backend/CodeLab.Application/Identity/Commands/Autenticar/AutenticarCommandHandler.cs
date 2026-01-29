using CodeLab.Application.Identity.Events.UsuarioAutenticadoExito;
using CodeLab.Application.Identity.Events.UsuarioNoAutenticado;
using CodeLab.Application.Shared.Common;
using CodeLab.Application.Shared.Results;
using CodeLab.Infrastructure.Jwt.Contracts.Interfaces;
using CodeLab.Infrastructure.SqlServer.Contracts.Exceptions;
using CodeLab.Infrastructure.SqlServer.Contracts.Interfaces;

namespace CodeLab.Application.Identity.Commands.Autenticar;

public class AutenticarCommandHandler(
    IAuthRepository authRepository,
    IJwtService jwtService,
    IMediator mediator
) : IRequestHandler<AutenticarCommand, CodeLabResultado<LoginResultDTO>>
{
    public async Task<CodeLabResultado<LoginResultDTO>> Handle(AutenticarCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var usuario = await authRepository.IniciarSesion(request.Email, request.Clave);
            var token = jwtService.GenerateToken(usuario.Id, usuario.Roles);
            
            var usuarioAutenticadoExito = new UsuarioAutenticadoExitoEvent(usuario.Id);
            await mediator.Publish(usuarioAutenticadoExito, cancellationToken);

            var loginResult = new LoginResultDTO(
                Token: token,
                RefreshToken: usuario.RefreshToken,
                Expiration: usuario.FechaExpiracion
            );
            return CodeLabResultado<LoginResultDTO>.Exito(loginResult);
        }
        catch (AuthException ex)
        {
            var usuarioNoAutenticado = new UsuarioNoAutenticadoEvent(request.Email, ex);
            await mediator.Publish(usuarioNoAutenticado, cancellationToken);

            return CodeLabResultado<LoginResultDTO>.Error(ex.Message);
        }
    }
}