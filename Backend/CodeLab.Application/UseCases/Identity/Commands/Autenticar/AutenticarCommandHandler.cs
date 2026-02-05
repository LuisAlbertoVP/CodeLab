using CodeLab.Application.Shared.Common;
using CodeLab.Application.Shared.Results;
using CodeLab.Application.UseCases.Identity.Interfaces;

namespace CodeLab.Application.UseCases.Identity.Commands.Autenticar;

public class AutenticarCommandHandler(
    IAuthService authService
) : IRequestHandler<AutenticarCommand, CodeLabResultado<LoginResultDTO>>
{
    public Task<CodeLabResultado<LoginResultDTO>> Handle(AutenticarCommand request, CancellationToken ct)
    {
        return authService.Autenticar(request.Email, request.Clave);
    }
}