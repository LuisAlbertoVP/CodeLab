using System;

namespace CodeLab.Infrastructure.RabbitMq.Contracts.Interfaces;

public interface IMailPublisherService
{
    Task InitializeAsync();

    Task PublishAsync(string message);
}