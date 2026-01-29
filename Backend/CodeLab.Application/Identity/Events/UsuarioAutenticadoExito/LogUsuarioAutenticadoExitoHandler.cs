using CodeLab.Application.Shared.Common;
using CodeLab.Infrastructure.Logging.Contracts.Interfaces;

namespace CodeLab.Application.Identity.Events.UsuarioAutenticadoExito;

public class LogUsuarioAutenticadoHandler(ICodeLabLogger logger) : INotificationHandler<UsuarioAutenticadoExitoEvent>
{
    public Task Handle(UsuarioAutenticadoExitoEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Usuario con ID '{notification.IdUsuario}' autenticado correctamente.");
        return Task.CompletedTask;
    }
}