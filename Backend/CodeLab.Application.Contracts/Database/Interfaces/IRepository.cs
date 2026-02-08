using System.Linq.Expressions;

namespace CodeLab.Application.Contracts.Database.Interfaces;

public interface IRepository<T> where T : class
{
    Task<bool> AnyAsync(Expression<Func<T?, bool>> predicate);
    Task<T?> GetByIdAsync(Guid id);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T?, bool>> predicate);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}