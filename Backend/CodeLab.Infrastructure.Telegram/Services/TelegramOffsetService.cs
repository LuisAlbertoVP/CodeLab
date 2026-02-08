using CodeLab.Application.Contracts.Database.Interfaces;
using CodeLab.Application.Contracts.Telegram.Interfaces;
using CodeLab.Domain.Entities;
using CodeLab.Domain.Interfaces;

namespace CodeLab.Infrastructure.Telegram.Services;

public class TelegramOffsetService(
    IRepository<Parametros> repositoryParametros,
    IUnitOfWork unitOfWork
) : ITelegramOffsetService
{
    public async Task SaveOffsetAsync(int offset, CancellationToken ct)
    {
        var parametro = await repositoryParametros.FirstOrDefaultAsync(p => p.Nombre == "Telegram:LastOffset");
        parametro.Valor = offset.ToString();
        await unitOfWork.SaveChangesAsync(ct);    
    }
}