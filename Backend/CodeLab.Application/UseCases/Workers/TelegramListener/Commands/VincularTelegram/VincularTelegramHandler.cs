using CodeLab.Application.Shared.Common;
using CodeLab.Application.UseCases.Workers.TelegramListener.DTOs;
using CodeLab.Application.UseCases.Workers.TelegramListener.Interfaces;

namespace CodeLab.Application.UseCases.Workers.TelegramListener.Commands.VincularTelegram;

public class VincularTelegramHandler(ITelegramHandlerService telegramHandlerService) : IRequestHandler<VincularTelegramCommand, TelegramUsuarioSesionResponse>
{
    public Task<TelegramUsuarioSesionResponse> Handle(VincularTelegramCommand request, CancellationToken ct)
    {
        return telegramHandlerService.HandleMessageAsync(request.ChatId, request.Message);
    }
}