using CodeLab.Application.Shared.Common;
using CodeLab.Application.Shared.Results;

namespace CodeLab.Application.Identity.Commands.RefrescarToken;

public record RefrescarTokenCommand(string RefreshToken) : IRequest<CodeLabResultado<LoginResultDTO>>;