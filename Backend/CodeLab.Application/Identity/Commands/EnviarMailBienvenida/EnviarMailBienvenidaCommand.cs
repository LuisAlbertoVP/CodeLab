using CodeLab.Application.Shared.Common;
using CodeLab.Application.Shared.Results;

namespace CodeLab.Application.Identity.Commands.EnviarMailBienvenida;

public class EnviarMailBienvenidaCommand : IRequest<CodeLabResultado<string>>
{
    public string Mensaje { get; set; }
}