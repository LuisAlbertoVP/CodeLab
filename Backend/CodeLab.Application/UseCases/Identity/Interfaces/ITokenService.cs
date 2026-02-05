using CodeLab.Domain.Entities;

namespace CodeLab.Application.UseCases.Identity.Interfaces;

public interface ITokenService
{
    RefreshToken GenerarRefreshToken();
    string GenerarToken(int idUsuario, List<string> roles);
}