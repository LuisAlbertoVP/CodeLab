using CodeLab.Application.Shared.Common;
using CodeLab.Domain.Entities;
using CodeLab.Domain.Interfaces;

namespace CodeLab.Application.UseCases.Workers.TelegramListener.Commands.GuardarOffset;

public class GuardarOffsetHandler(IRepository<Parametros> repositoryParametros) : IRequestHandler<GuardarOffsetCommand, Unit>
{
    public async Task<Unit> Handle(GuardarOffsetCommand request, CancellationToken ct)
    {
        var parametro = await repositoryParametros.FirstOrDefaultAsync(p => p.Nombre == "Telegram:LastOffset");
        parametro.Valor = request.Offset.ToString();
        return Unit.Value;
    }
}