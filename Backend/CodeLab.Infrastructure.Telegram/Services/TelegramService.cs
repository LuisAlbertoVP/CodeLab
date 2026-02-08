using CodeLab.Application.Contracts.Providers.Interfaces;
using CodeLab.Application.Contracts.Telegram.Interfaces;
using Telegram.Bot;

namespace CodeLab.Infrastructure.Telegram.Services;

public class TelegramService(IConfigTelegramProvider configTelegramProvider) : ITelegramService
{
    public async Task EnviarMensaje(long chatId, string mensaje, CancellationToken ct)
    {
        var bot = new TelegramBotClient(configTelegramProvider.Token, cancellationToken: ct);
        await bot.SendMessage(chatId, mensaje, cancellationToken: ct);
    }
}