using System;

namespace CodeLab.Infrastructure.RabbitMq.Contracts.Interfaces;

public interface IMailConsumerService
{
    Task InitializeAsync();
    
    Task StartAsync(Func<string, Task> task);
}