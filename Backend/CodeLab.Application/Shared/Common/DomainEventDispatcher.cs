using System.Collections.Concurrent;
using System.Linq.Expressions;
using CodeLab.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CodeLab.Application.Shared.Common;

public class DomainEventDispatcher(IServiceScopeFactory scopeFactory) : IDomainEventDispatcher
{
    private readonly ConcurrentDictionary<Type, Func<IPublisher, IDomainEvent, CancellationToken, Task>> _publishers = new();
    
    public async Task DispatchAsync(IReadOnlyCollection<IDomainEvent> domainEvents, CancellationToken ct)
    {
        const int maxConcurrency = 3;
        using var semaphore = new SemaphoreSlim(maxConcurrency);

        var tasks = domainEvents.Select(async (domainEvent) =>
        {
            await semaphore.WaitAsync(ct);
            try
            {
                using var scope = scopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await DispatchEvent(mediator, domainEvent, ct);
            }
            finally
            {
                semaphore.Release();
            }
        });

        await Task.WhenAll(tasks);
    }

    private async Task DispatchEvent(IMediator mediator, IDomainEvent domainEvent, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var eventType = domainEvent.GetType();
        var publisher = _publishers.GetOrAdd(eventType, CreatePublisher);

        await publisher(mediator, domainEvent, ct);
    }

    private static Func<IPublisher, IDomainEvent, CancellationToken, Task> CreatePublisher(Type eventType)
    {
        var publishMethod = typeof(IPublisher)
            .GetMethod(nameof(IPublisher.Publish))!
            .MakeGenericMethod(typeof(DomainEventNotification<>)
            .MakeGenericType(eventType));

        var mediatorParam = Expression.Parameter(typeof(IPublisher), "mediator");
        var domainEventParam = Expression.Parameter(typeof(IDomainEvent), "domainEvent");
        var ctParam = Expression.Parameter(typeof(CancellationToken), "ct");

        var notificationType = typeof(DomainEventNotification<>)
            .MakeGenericType(eventType);

        var notificationCtor = notificationType.GetConstructor([eventType])!;

        var newNotification = Expression.New(
            notificationCtor,
            Expression.Convert(domainEventParam, eventType));

        var call = Expression.Call(
            mediatorParam,
            publishMethod,
            newNotification,
            ctParam);

        return Expression
            .Lambda<Func<IPublisher, IDomainEvent, CancellationToken, Task>>(
                call,
                mediatorParam,
                domainEventParam,
                ctParam)
            .Compile();
    }
}