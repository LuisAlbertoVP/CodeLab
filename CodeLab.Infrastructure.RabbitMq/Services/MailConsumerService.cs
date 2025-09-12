using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CodeLab.Infrastructure.RabbitMq.Services;

public class MailConsumerService
{
    public async Task StartAsync(Func<string, Task> task)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "mail", durable: false, exclusive: false, autoDelete: false,
            arguments: null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            await task(message);
        };

        await channel.BasicConsumeAsync("mail", autoAck: true, consumer: consumer);
    }
}