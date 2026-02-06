namespace CodeLab.Application.Contracts.RabbitMq.Interfaces;

public interface IMailPublisherService
{
    Task InitializeAsync();

    Task PublishAsync(string message);
}