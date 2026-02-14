using CodeLab.Application.Contracts.Database.Interfaces;
using CodeLab.Application.Contracts.Jwt.Interfaces;
using CodeLab.Application.Shared.Common;
using CodeLab.Application.Shared.Results;

namespace CodeLab.Application.UseCases.Identity.Commands.RefrescarToken;

public class RefrescarTokenHandler(
    IAuthRepository authRepository,
    IJwtService jwtService
) : IRequestHandler<RefrescarTokenCommand, CodeLabResultado<LoginResultDTO>>
{
    public async Task<CodeLabResultado<LoginResultDTO>> Handle(RefrescarTokenCommand request, CancellationToken ct)
    {
        var refreshToken = await authRepository.ObtenerRefreshToken(request.RefreshToken);

        if (refreshToken == null || refreshToken.FechaExpiracion <= DateTime.UtcNow)
            return CodeLabResultado<LoginResultDTO>.Error("Tu sesión ha expirado, por favor vuelve a iniciar sesión.");

        DateTime fechaExpiracion = DateTime.UtcNow.AddDays(3);
        refreshToken.FechaExpiracion = fechaExpiracion;

        var roles = refreshToken.Usuario.UsuarioRol
            .Where(ur => ur.Rol != null)
            .Select(ur => ur.Rol.Codigo)
            .ToList();
        var token = jwtService.GenerateToken(refreshToken.IdUsuario, roles);
        
        var loginResult = new LoginResultDTO(
            Token: token,
            RefreshToken: refreshToken.Token,
            Expiration: refreshToken.FechaExpiracion
        );
        
        return CodeLabResultado<LoginResultDTO>.Exito(loginResult);
    }
}