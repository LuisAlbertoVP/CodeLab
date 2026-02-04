using CodeLab.Application.Shared.Common;
using CodeLab.Domain.Events;
using CodeLab.Infrastructure.Logging.Contracts.Interfaces;

namespace CodeLab.Application.Identity.Events.UsuarioAutenticadoExito;

public class LogUsuarioAutenticadoHandler(ICodeLabLogger logger) : INotificationHandler<DomainEventNotification<UsuarioAutenticadoEvent>>
{
    public Task Handle(DomainEventNotification<UsuarioAutenticadoEvent> notification, CancellationToken ct)
    {
        logger.LogInformation($"Usuario con ID '{notification.DomainEvent.Usuario.Id}' autenticado correctamente.");
        return Task.CompletedTask;
    }
}