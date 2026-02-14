namespace CodeLab.Application.Contracts.Caching.Interfaces;

public interface ICacheService
{
    void Remove(string key);

    void Set<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow);

    bool TryGetValue<T>(string key, out T value);
}