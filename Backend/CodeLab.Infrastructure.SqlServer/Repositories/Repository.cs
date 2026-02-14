using System.Linq.Expressions;
using CodeLab.Domain.Interfaces;
using CodeLab.Infrastructure.SqlServer.Providers;
using Microsoft.EntityFrameworkCore;

namespace CodeLab.Infrastructure.SqlServer.Repositories;

public class Repository<T>(CodeLabContext context) : IRepository<T> where T : class
{
    public async Task<bool> AnyAsync(Expression<Func<T?, bool>> predicate)
    {
        return await context.Set<T>().AnyAsync(predicate);
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await context.Set<T>().FindAsync(id);
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T?, bool>> predicate)
    {
        return await context.Set<T>().FirstOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await context.Set<T>().ToListAsync();
    } 

    public async Task AddAsync(T entity)
    {
        await context.Set<T>().AddAsync(entity);
    }

    public async Task UpdateAsync(T entity)
    {
        context.Set<T>().Update(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            context.Set<T>().Remove(entity);
        }
    }
}