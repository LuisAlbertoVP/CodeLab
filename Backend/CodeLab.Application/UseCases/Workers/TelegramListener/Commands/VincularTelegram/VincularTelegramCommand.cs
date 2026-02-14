using CodeLab.Application.Shared.Common;
using CodeLab.Application.UseCases.Workers.TelegramListener.DTOs;

namespace CodeLab.Application.UseCases.Workers.TelegramListener.Commands.VincularTelegram;

public record VincularTelegramCommand(long ChatId, string Message) : ICommand<TelegramUsuarioSesionResponse>;