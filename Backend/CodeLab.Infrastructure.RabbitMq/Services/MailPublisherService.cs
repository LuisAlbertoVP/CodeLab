using System.Text;
using CodeLab.Application.Contracts.RabbitMq.Interfaces;
using RabbitMQ.Client;

namespace CodeLab.Infrastructure.RabbitMq.Services;

public class MailPublisherService : IMailPublisherService, IAsyncDisposable
{
    private IConnection _connection;
    private IChannel _channel;

    public async Task InitializeAsync()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        await _channel.QueueDeclareAsync(queue: "mail", durable: false, exclusive: false, autoDelete: false,
            arguments: null); 
    }

    public async Task PublishAsync(string message)
    {
        await _channel.QueueDeclareAsync(queue: "mail", durable: false, exclusive: false, autoDelete: false,
            arguments: null);

        var body = Encoding.UTF8.GetBytes(message);

        await _channel.BasicPublishAsync(exchange: string.Empty, routingKey: "mail", body: body);
    }
    
    public async ValueTask DisposeAsync()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }
}