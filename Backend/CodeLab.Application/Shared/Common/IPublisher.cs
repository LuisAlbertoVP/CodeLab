namespace CodeLab.Application.Shared.Common;

public interface IPublisher
{
    Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification;
}