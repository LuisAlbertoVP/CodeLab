using CodeLab.Application.Contracts.Providers.Interfaces;
using CodeLab.Application.Shared.Common;
using CodeLab.Application.UseCases.Workers.TelegramListener.Commands.GuardarOffset;
using CodeLab.Application.UseCases.Workers.TelegramListener.Commands.VincularTelegram;
using CodeLab.Application.UseCases.Workers.TelegramListener.DTOs;
using Telegram.Bot;

namespace CodeLab.Worker.TelegramListener;

public class Worker(IServiceScopeFactory serviceScopeFactory) : BackgroundService
{    
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var bot = CreateTelegramBot(ct);
        int? offset = null;
        
        while (!ct.IsCancellationRequested)
        {
            var updates = await bot.GetUpdates(offset, timeout: 2, cancellationToken: ct);
            foreach (var update in updates)
            {
                offset = update.Id + 1;
                try
                {
                    using var scope = serviceScopeFactory.CreateScope();

                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    var guardarOffset = new GuardarOffsetCommand(offset.Value);
                    await mediator.Send<GuardarOffsetCommand, Unit>(guardarOffset, ct);

                    var chatId = update.Message.Chat.Id;
                    var text = update.Message.Text?.Trim() ?? string.Empty;
                    
                    var vincularTelegram = new VincularTelegramCommand(chatId, text);
                    var response = await mediator.Send<VincularTelegramCommand, TelegramUsuarioSesionResponse>(vincularTelegram, ct);

                    await bot.SendMessage(chatId, response.Mensaje, cancellationToken: ct);
                }
                catch (Exception ex)
                {
                }
                if (ct.IsCancellationRequested) break;
            }
        }
    }

    private TelegramBotClient CreateTelegramBot(CancellationToken ct)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var configTelegramProvider = scope.ServiceProvider.GetRequiredService<IConfigTelegramProvider>();
        return new TelegramBotClient(configTelegramProvider.Token, cancellationToken: ct);
    }
}