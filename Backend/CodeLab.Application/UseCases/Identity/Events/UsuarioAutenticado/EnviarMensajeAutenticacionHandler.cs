using CodeLab.Application.Contracts.Database.Interfaces;
using CodeLab.Application.Contracts.Telegram.Interfaces;
using CodeLab.Application.Shared.Common;
using CodeLab.Domain.Entities;
using CodeLab.Domain.Events;

namespace CodeLab.Application.UseCases.Identity.Events.UsuarioAutenticado;

public class EnviarMensajeAutenticacionHandler(
    IRepository<UsuarioCanal> usuarioCanalRepository,
    ITelegramService telegramService
) : INotificationHandler<DomainEventNotification<UsuarioAutenticadoEvent>>
{
    public async Task Handle(DomainEventNotification<UsuarioAutenticadoEvent> notification, CancellationToken ct)
    {
        var usuarioCanal = await usuarioCanalRepository.FirstOrDefaultAsync(
            uc => uc.IdUsuario == notification.DomainEvent.Usuario.Id
        );
        await telegramService.EnviarMensaje(long.Parse(usuarioCanal.Destino), "Se ha registrado un nuevo inicio de sesion.", ct);
    }
}