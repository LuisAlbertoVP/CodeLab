using CodeLab.Application.Contracts.Logging.Interfaces;
using CodeLab.Application.Shared.Common;
using CodeLab.Domain.Events;

namespace CodeLab.Application.UseCases.Identity.Events.UsuarioAutenticado;

public class LogUsuarioAutenticadoHandler(ICodeLabLogger logger) : INotificationHandler<DomainEventNotification<UsuarioAutenticadoEvent>>
{
    public Task Handle(DomainEventNotification<UsuarioAutenticadoEvent> notification, CancellationToken ct)
    {
        logger.LogInformation($"Usuario con ID '{notification.DomainEvent.Usuario.Id}' autenticado correctamente.");
        return Task.CompletedTask;
    }
}