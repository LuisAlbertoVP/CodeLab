using CodeLab.Application.Shared.Results;

namespace CodeLab.Application.Identity.Interfaces;

public interface IAuthService
{
    Task<CodeLabResultado<LoginResultDTO>> Autenticar(string email, string clave);
}