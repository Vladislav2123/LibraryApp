using Microsoft.Extensions.Caching.Distributed;
using LibraryApp.Application.Abstractions.Caching;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using LibraryApp.Application.Abstractions;

namespace LibraryApp.DAL.Caching;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly ILibraryDbContext _dbContext;

    public CacheService(IDistributedCache cache, ILibraryDbContext dbContext)
    {
        _cache = cache;
        _dbContext = dbContext;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        where T : class
    {
        string? cachedValue = await _cache.GetStringAsync(key, cancellationToken);

        if (cachedValue == null) return null;

        T cachedObject = JsonConvert.DeserializeObject<T>(cachedValue);

        T trackingEntity = _dbContext.FindTrackingObject(cachedObject) as T;
        if(trackingEntity != null) 
            _dbContext.Entry(trackingEntity).State = EntityState.Detached;

        _dbContext.Set<T>().Attach(cachedObject);

        return cachedObject;
    }

    public async Task<T?> GetAsync<T>(string key, Func<Task<T>> query, CancellationToken cancellationToken = default)
        where T : class
    {
        T? cachedObject = await GetAsync<T>(key, cancellationToken);
        if (cachedObject != null) return cachedObject;

        return await query();
    }

    public async Task<T?> GetAndSetAsync<T>(string key, Func<Task<T>> query, CancellationToken cancellationToken = default)
        where T : class
    {
        T? cachedObject = await GetAsync<T>(key, cancellationToken);
        if (cachedObject != null) return cachedObject;

        cachedObject = await query();
        await SetAsync(key, cachedObject, cancellationToken);
        return cachedObject;
    }

    public async Task SetAsync<T>(string key, T cacheObject, CancellationToken cancellationToken = default)
        where T : class
    {
        string? cacheValue = JsonConvert.SerializeObject(cacheObject);

        await _cache.SetStringAsync(key, cacheValue, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _cache.RemoveAsync(key, cancellationToken);
    }
}
