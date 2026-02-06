using CodeLab.Application.Contracts.RabbitMq.Interfaces;

namespace CodeLab.Service.Mail;

public class Worker(IMailConsumerService mailConsumerService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await mailConsumerService.InitializeAsync();
        await mailConsumerService.StartAsync(async message =>
        {
            Console.WriteLine($"Received message: {message}");
            await Task.CompletedTask;
        });
        await Task.Delay(-1, cancellationToken: stoppingToken);
    }
}