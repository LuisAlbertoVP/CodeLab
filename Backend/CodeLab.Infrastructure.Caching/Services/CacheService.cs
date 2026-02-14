using CodeLab.Application.Contracts.Caching.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace CodeLab.Infrastructure.Caching.Services;

public class CacheService(IMemoryCache memoryCache) : ICacheService
{
    public void Remove(string key)
    {
        memoryCache.Remove(key);
    }

    public void Set<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow)
    {
        memoryCache.Set(key, value, absoluteExpirationRelativeToNow ?? TimeSpan.FromMinutes(5));
    }

    public bool TryGetValue<T>(string key, out T value)
    {
        return memoryCache.TryGetValue(key, out value);
    }
}