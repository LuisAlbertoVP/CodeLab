namespace CodeLab.Application.Contracts.Telegram.Interfaces;

public interface ITelegramOffsetService
{
    Task SaveOffsetAsync(int offset, CancellationToken ct);
}