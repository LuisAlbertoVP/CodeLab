namespace CodeLab.Application.Shared.Common;

public interface INotificationHandler<TNotification> where TNotification : INotification
{
    Task Handle(TNotification notification, CancellationToken cancellationToken);
}