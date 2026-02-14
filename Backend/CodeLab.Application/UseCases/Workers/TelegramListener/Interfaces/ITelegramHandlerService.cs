using CodeLab.Application.UseCases.Workers.TelegramListener.DTOs;

namespace CodeLab.Application.UseCases.Workers.TelegramListener.Interfaces;

public interface ITelegramHandlerService
{
    Task<TelegramUsuarioSesionResponse> HandleMessageAsync(long chatId, string message);
}