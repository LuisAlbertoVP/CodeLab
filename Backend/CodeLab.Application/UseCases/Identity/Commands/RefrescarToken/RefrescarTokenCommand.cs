using CodeLab.Application.Shared.Common;
using CodeLab.Application.Shared.Results;

namespace CodeLab.Application.UseCases.Identity.Commands.RefrescarToken;

public record RefrescarTokenCommand(string RefreshToken) : ICommand<CodeLabResultado<LoginResultDTO>>;