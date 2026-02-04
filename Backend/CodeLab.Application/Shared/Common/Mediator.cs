using Microsoft.Extensions.DependencyInjection;

namespace CodeLab.Application.Shared.Common;

public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    public async Task<TResult> Send<TRequest, TResult>(TRequest request, CancellationToken ct = default)
        where TRequest : IRequest<TResult>
    {
        var handler = serviceProvider.GetService<IRequestHandler<TRequest, TResult>>() ??
            throw new InvalidOperationException($"No handler registered for {typeof(TRequest).Name}");
            
        var behaviors = serviceProvider.GetServices<IPipelineBehavior<TRequest, TResult>>().Reverse();

        Func<Task<TResult>> handlerDelegate = () => handler.Handle(request, ct);
        foreach (var behavior in behaviors)
        {
            var next = handlerDelegate;
            handlerDelegate = () => behavior.Handle(request, next, ct);
        }

        return await handlerDelegate();
    }

    public async Task Publish<TNotification>(TNotification notification, CancellationToken ct = default)
        where TNotification : INotification
    {
        var handlers = serviceProvider.GetServices<INotificationHandler<TNotification>>() ??
            throw new InvalidOperationException($"No handler registered for {typeof(TNotification).Name}");

        foreach (var handler in handlers)
        {
            await handler.Handle(notification, ct);
        }
    }
}