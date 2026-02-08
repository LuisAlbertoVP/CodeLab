using System.Collections.Concurrent;
using CodeLab.Application.Contracts.Telegram.DTOs;
using CodeLab.Application.Contracts.Telegram.Interfaces;

namespace CodeLab.Infrastructure.Telegram.Services;

public class TelegramUsuarioSesionService : ITelegramUsuarioSesionService
{
    private readonly ConcurrentDictionary<long, TelegramUsuarioSesion> _sessions = new();

    public TelegramUsuarioSesion GetOrCreate(long chatId)
        => _sessions.GetOrAdd(chatId, _ => new TelegramUsuarioSesion());

    public void Reset(long chatId)
        => _sessions.TryRemove(chatId, out _);
}