using CodeLab.Domain.Interfaces;

namespace CodeLab.Infrastructure.SqlServer.Providers;

public class UnitOfWork(CodeLabContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return context.SaveChangesAsync(ct);
    }
}