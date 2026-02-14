using CodeLab.Application.Shared.Common;

namespace CodeLab.Application.UseCases.Workers.TelegramListener.Commands.GuardarOffset;

public record GuardarOffsetCommand(int Offset) : ICommand<Unit>;