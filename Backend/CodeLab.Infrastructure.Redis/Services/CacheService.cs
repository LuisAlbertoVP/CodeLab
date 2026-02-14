using System.Text.Json;
using CodeLab.Application.Contracts.Caching.Interfaces;
using StackExchange.Redis;

namespace CodeLab.Infrastructure.Redis.Services;

public class CacheService(IConnectionMultiplexer redis) : ICacheService
{
    private readonly IDatabase _database = redis.GetDatabase();

    public void Remove(string key)
    {
        _database.KeyDelete(key);
    }

    public void Set<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow)
    {
        var json = JsonSerializer.Serialize(value);

        _database.StringSet(
            key,
            json,
            absoluteExpirationRelativeToNow ?? TimeSpan.FromMinutes(5)
        );
    }

    public bool TryGetValue<T>(string key, out T value)
    {
        var redisValue = _database.StringGet(key);

        if (redisValue.IsNullOrEmpty)
        {
            value = default!;
            return false;
        }

        try
        {
            value = JsonSerializer.Deserialize<T>(redisValue!)!;
            return true;
        }
        catch
        {
            value = default!;
            return false;
        }
    }
}