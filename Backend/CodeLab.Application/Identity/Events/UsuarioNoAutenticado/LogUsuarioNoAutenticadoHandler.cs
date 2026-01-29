using CodeLab.Application.Shared.Common;
using CodeLab.Infrastructure.Logging.Contracts.Interfaces;

namespace CodeLab.Application.Identity.Events.UsuarioNoAutenticado;

public class LogUsuarioNoAutenticadoHandler(ICodeLabLogger logger) : INotificationHandler<UsuarioNoAutenticadoEvent>
{
    public Task Handle(UsuarioNoAutenticadoEvent notification, CancellationToken cancellationToken)
    {
        logger.LogError($"Error de autenticación {notification.Usuario}.", notification.Exception);
        return Task.CompletedTask;
    }
}