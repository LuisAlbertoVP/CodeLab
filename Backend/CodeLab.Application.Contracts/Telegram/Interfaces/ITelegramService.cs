namespace CodeLab.Application.Contracts.Telegram.Interfaces;

public interface ITelegramService
{
    Task EnviarMensaje(string mensaje, CancellationToken ct);
}