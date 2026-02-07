using CodeLab.Domain.Entities;
using CodeLab.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CodeLab.Infrastructure.SqlServer.Providers;

public class CodeLabInterceptor(IDomainEventDispatcher domainEventDispatcher) : SaveChangesInterceptor
{
    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken ct = default)
    {
        var context = eventData.Context!;
        
        var domainEvents = new List<IDomainEvent>();
        foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
        {
            var entity = entry.Entity;

            if (entity.DomainEvents.Count == 0)
                continue;

            domainEvents.AddRange(entity.DomainEvents);
            entity.ClearDomainEvents();
        }

        if (domainEvents.Any())
        {
            await domainEventDispatcher.DispatchAsync(domainEvents, ct);
        }

        return result;
    }
}