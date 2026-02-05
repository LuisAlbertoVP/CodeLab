using System;

namespace CodeLab.Application.Interfaces.RabbitMq;

public interface IMailConsumerService
{
    Task InitializeAsync();
    
    Task StartAsync(Func<string, Task> task);
}