using System.Text.Json;
using Application.Interfaces;
using StackExchange.Redis;

namespace Application.Services;
public class RedisCache : IRedisCache
{
    private readonly IConnectionMultiplexer _redis;

    public RedisCache(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task SetStringAsync(string key, string value, TimeSpan? expiry = null)
    {
        var db = _redis.GetDatabase();
        await db.StringSetAsync(key, value, expiry);
    }

    public async Task<string?> GetStringAsync(string key)
    {
        var db = _redis.GetDatabase();
        var result = await db.StringGetAsync(key);
        return result.IsNullOrEmpty ? null : result.ToString();
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var db = _redis.GetDatabase();
        var json = JsonSerializer.Serialize(value);
        await db.StringSetAsync(key, json, expiry);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var db = _redis.GetDatabase();
        var result = await db.StringGetAsync(key);
        if (result.IsNullOrEmpty)
        {
            return default;
        }
        return JsonSerializer.Deserialize<T>(result.ToString());
    }
}
