namespace CodeLab.Application.Contracts.RabbitMq.Interfaces;

public interface IMailConsumerService
{
    Task InitializeAsync();
    
    Task StartAsync(Func<string, Task> task);
}