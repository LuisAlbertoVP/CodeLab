using System.Text;
using CodeLab.Application.Contracts.RabbitMq.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CodeLab.Infrastructure.RabbitMq.Services;

public class MailConsumerService : IMailConsumerService, IAsyncDisposable
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

    public async Task StartAsync(Func<string, Task> task)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            await task(message);
        };

        await _channel.BasicConsumeAsync("mail", autoAck: true, consumer: consumer);
    }
    
    public async ValueTask DisposeAsync()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }
}