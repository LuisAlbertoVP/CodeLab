using CodeLab.Application.Contracts.Database.Interfaces;
using CodeLab.Application.Shared.Results;
using CodeLab.Application.UseCases.Identity.Interfaces;
using CodeLab.Domain.Entities;
using CodeLab.Domain.Interfaces;

namespace CodeLab.Application.UseCases.Identity.Services;

public class AuthService(
    IAuthRepository authRepository,
    IRepository<RefreshToken> refreshTokenRepository,
    ITokenService tokenService
) : IAuthService
{
    public async Task<CodeLabResultado<LoginResultDTO>> Autenticar(string email, string clave)
    {
        var usuario = await authRepository.ObtenerUsuarioPorEmail(email);
        
        if (usuario == null)
            return CodeLabResultado<LoginResultDTO>.Error("El correo ingresado no se encuentra registrado.");

        usuario.Autenticar(clave);

        var refreshToken = tokenService.GenerarRefreshToken();
        refreshToken.IdUsuario = usuario.Id;
        await refreshTokenRepository.AddAsync(refreshToken);

        var roles = usuario.UsuarioRol
            .Where(ur => ur.Rol != null)
            .Select(ur => ur.Rol.Codigo)
            .ToList();
        var token = tokenService.GenerarToken(usuario.Id, roles);

        var loginResult = new LoginResultDTO(
            Token: token,
            RefreshToken: refreshToken.Token,
            Expiration: refreshToken.FechaExpiracion
        );

        return CodeLabResultado<LoginResultDTO>.Exito(loginResult);
    }
}