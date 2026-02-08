namespace CodeLab.Application.Contracts.Telegram.Interfaces;

public interface ITelegramService
{
    Task EnviarMensaje(long chatId, string mensaje, CancellationToken ct);
}