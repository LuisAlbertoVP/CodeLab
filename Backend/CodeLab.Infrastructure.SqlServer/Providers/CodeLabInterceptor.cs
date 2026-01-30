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
        
        var entities = context.ChangeTracker
            .Entries<BaseEntity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToList();

        foreach (var entity in entities)
        {
            var domainEvents = entity.DomainEvents.ToList();
            entity.ClearDomainEvents();
            
            foreach (var domainEvent in domainEvents)
            {
                await domainEventDispatcher.DispatchAsync(domainEvent, ct);
            }
        }

        return result;
    }
}