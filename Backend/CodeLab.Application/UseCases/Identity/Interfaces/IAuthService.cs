using CodeLab.Application.Shared.Results;

namespace CodeLab.Application.UseCases.Identity.Interfaces;

public interface IAuthService
{
    Task<CodeLabResultado<LoginResultDTO>> Autenticar(string email, string clave);
}