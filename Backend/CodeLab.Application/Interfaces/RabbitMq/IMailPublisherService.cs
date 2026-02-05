using System;

namespace CodeLab.Application.Interfaces.RabbitMq;

public interface IMailPublisherService
{
    Task InitializeAsync();

    Task PublishAsync(string message);
}