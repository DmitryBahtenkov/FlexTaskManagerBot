using System.Text.Json;
using FTM.Domain.Services;

namespace FTM.Infrastructure.Redis;

public class CacheService : ICacheService
{
    public async Task<TValue?> GetOrCreate<TValue>(string key, Func<Task<TValue>> factory, TimeSpan? duration = null)
    {
        var cache = RedisConnection.RedisCache;
        var value = await cache.StringGetAsync(key);

        if (value.HasValue)
        {
            return JsonSerializer.Deserialize<TValue>(value.ToString());
        }

        return await CreateEntry(key, factory, duration);
    }

    public async Task<TValue?> CreateEntry<TValue>(string key, Func<Task<TValue>> factory, TimeSpan? duration = null)
    {
        var cache = RedisConnection.RedisCache;
        var newValue = await factory();
        var result = await cache.StringSetAsync(key, JsonSerializer.Serialize(newValue), expiry: duration);
        if (result)
        {
            return newValue;
        }

        throw new Exception("Не удалось добавить запись в кэш");
    }

    public async Task<TValue?> GetEntry<TValue>(string key)
    {
        var cache = RedisConnection.RedisCache;
        var value = await cache.StringGetAsync(key);

        return value.HasValue ? JsonSerializer.Deserialize<TValue>(value.ToString()) : default;
    }
}