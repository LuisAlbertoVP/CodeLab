using CodeLab.Application.Contracts.Logging.Interfaces;
using CodeLab.Application.Shared.Common;

namespace CodeLab.Application.UseCases.Identity.Events.UsuarioNoAutenticado;

public class LogUsuarioNoAutenticadoHandler(ICodeLabLogger logger) : INotificationHandler<UsuarioNoAutenticadoEvent>
{
    public Task Handle(UsuarioNoAutenticadoEvent notification, CancellationToken ct)
    {
        logger.LogError($"Error de autenticación {notification.Usuario}.", notification.Exception);
        return Task.CompletedTask;
    }
}