using CodeLab.Application.Contracts.RabbitMq.Interfaces;
using CodeLab.Infrastructure.RabbitMq.Services;
using CodeLab.Service.Mail;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<IMailConsumerService, MailConsumerService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();