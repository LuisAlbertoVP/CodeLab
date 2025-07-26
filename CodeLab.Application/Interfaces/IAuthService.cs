using CodeLab.Application.Results;

namespace CodeLab.Application.Interfaces;

public interface IAuthService
{
    Task<CodeLabResultado<string>> IniciarSesion(string email, string clave);
}