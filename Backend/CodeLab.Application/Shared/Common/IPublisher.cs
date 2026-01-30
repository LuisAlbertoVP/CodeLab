namespace CodeLab.Application.Shared.Common;

public interface IPublisher
{
    Task Publish<TNotification>(TNotification notification, CancellationToken ct = default)
        where TNotification : INotification;
}