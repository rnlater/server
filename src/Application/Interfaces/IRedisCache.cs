namespace Application.Interfaces;
public interface IRedisCache
{
    /// <summary>
    ///     
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expiry"></param>
    /// <returns></returns>
    Task SetStringAsync(string key, string value, TimeSpan? expiry = null);

    /// <summary>
    /// Get the value of the specified key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<string?> GetStringAsync(string key);

    /// <summary>
    /// Set the value of the specified key
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expiry"></param>
    /// <returns></returns>
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);

    /// <summary>
    /// Get the value of the specified key
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<T?> GetAsync<T>(string key);
}