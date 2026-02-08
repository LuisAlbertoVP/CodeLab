using CodeLab.Application.Contracts.Providers.Interfaces;
using CodeLab.Application.Contracts.Telegram.Interfaces;
using Telegram.Bot;

namespace CodeLab.Worker.TelegramListener;

public class Worker(
    IConfigTelegramProvider configTelegramProvider,
    IServiceScopeFactory serviceScopeFactory
) : BackgroundService
{    
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var bot = new TelegramBotClient(configTelegramProvider.Token, cancellationToken: ct);
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

                    var offsetService = scope.ServiceProvider.GetRequiredService<ITelegramOffsetService>();
                    await offsetService.SaveOffsetAsync(offset.Value, ct);

                    var handlerService = scope.ServiceProvider.GetRequiredService<ITelegramHandlerService>();

                    var chatId = update.Message.Chat.Id;
                    var text = update.Message.Text?.Trim() ?? string.Empty;

                    var response = await handlerService.HandleMessageAsync(chatId, text);
                    await bot.SendMessage(chatId, response.Mensaje, cancellationToken: ct);
                }
                catch (Exception ex)
                {
                }
                if (ct.IsCancellationRequested) break;
            }
        }
    }
}