using CodeLab.Application.Shared.Common;
using CodeLab.Application.Shared.Results;

namespace CodeLab.Application.UseCases.Identity.Commands.Autenticar;

public record AutenticarCommand(string Email, string Clave) : ICommand<CodeLabResultado<LoginResultDTO>>;