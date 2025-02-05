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
        try
        {
            await db.StringSetAsync(key, value, expiry);
        }
        catch { }
    }

    public async Task<string?> GetStringAsync(string key)
    {
        var db = _redis.GetDatabase();
        try
        {
            var result = await db.StringGetAsync(key);
            return result.IsNullOrEmpty ? null : result.ToString();
        }
        catch
        {
            await db.KeyDeleteAsync(key);
            return null;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var db = _redis.GetDatabase();
        try
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            };
            var json = JsonSerializer.Serialize(value, options);
            await db.StringSetAsync(key, json, expiry);
        }
        catch { }
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var db = _redis.GetDatabase();
        var result = await db.StringGetAsync(key);
        if (result.IsNullOrEmpty)
        {
            return default;
        }
        try
        {
            return JsonSerializer.Deserialize<T>(result.ToString());
        }
        catch
        {
            await db.KeyDeleteAsync(key);
            return default;
        }
    }
}
