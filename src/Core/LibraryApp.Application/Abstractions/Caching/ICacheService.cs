namespace LibraryApp.Application.Abstractions.Caching;

public interface ICacheService
{
    /// <summary>
    /// Get object of type T from cache.
    /// </summary>
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        where T : class;

    /// <summary>
    /// Get object of type T from cache. If object does not exist in cache, it`s taken from query.
    /// </summary>
    Task<T?> GetAsync<T>(string key, Func<Task<T>> query, CancellationToken cancellationToken = default)
        where T : class;

    /// <summary>
    /// Get object of type T from cache. If object does not exist in cache, it`s taken from query and sets to cache.
    /// </summary>
    Task<T?> GetAndSetAsync<T>(string key, Func<Task<T>> query, CancellationToken cancellationToken = default)
        where T : class;

    /// <summary>
    /// Add object to cache.
    /// </summary>
    Task SetAsync<T>(string key, T cacheObject, CancellationToken cancellationToken = default)
        where T : class;

    /// <summary>
    /// Remove object from cache.
    /// </summary>
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
}
