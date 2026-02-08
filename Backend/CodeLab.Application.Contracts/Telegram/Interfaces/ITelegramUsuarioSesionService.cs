using CodeLab.Application.Contracts.Telegram.DTOs;

namespace CodeLab.Application.Contracts.Telegram.Interfaces;

public interface ITelegramUsuarioSesionService
{
    TelegramUsuarioSesion GetOrCreate(long chatId);
    void Reset(long chatId);
}