using CodeLab.Domain.Interfaces;

namespace CodeLab.Application.Shared.Common;

public class DomainEventDispatcher(IMediator mediator) : IDomainEventDispatcher
{
    public async Task DispatchAsync(IDomainEvent domainEvent, CancellationToken ct)
    {
        var notificationType = typeof(DomainEventNotification<>)
            .MakeGenericType(domainEvent.GetType());

        var notification = Activator.CreateInstance(
            notificationType, domainEvent);

        await mediator.Publish((INotification)notification!, ct);
    }
}