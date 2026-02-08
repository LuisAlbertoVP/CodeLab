using CodeLab.Application.Contracts.Telegram.DTOs;

namespace CodeLab.Application.Contracts.Telegram.Interfaces;

public interface ITelegramHandlerService
{
    Task<TelegramUsuarioSesionResponse> HandleMessageAsync(long chatId, string message);
}