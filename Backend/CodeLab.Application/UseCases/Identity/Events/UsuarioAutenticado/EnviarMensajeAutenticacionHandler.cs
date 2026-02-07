using CodeLab.Application.Contracts.Telegram.Interfaces;
using CodeLab.Application.Shared.Common;
using CodeLab.Domain.Events;

namespace CodeLab.Application.UseCases.Identity.Events.UsuarioAutenticado;

public class EnviarMensajeAutenticacionHandler(ITelegramService telegramService) : INotificationHandler<DomainEventNotification<UsuarioAutenticadoEvent>>
{
    public async Task Handle(DomainEventNotification<UsuarioAutenticadoEvent> notification, CancellationToken ct)
    {
        await telegramService.EnviarMensaje("Se ha registrado un nuevo inicio de sesion.", ct);
    }
}