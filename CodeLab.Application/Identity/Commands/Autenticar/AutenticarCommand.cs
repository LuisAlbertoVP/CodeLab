using CodeLab.Application.Shared.Common;
using CodeLab.Application.Shared.Results;

namespace CodeLab.Application.Identity.Commands.Autenticar;

public record AutenticarCommand(string Email, string Clave) : IRequest<CodeLabResultado<LoginResultDTO>>;