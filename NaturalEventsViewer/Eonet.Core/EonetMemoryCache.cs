using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Eonet.Core
{
    public class EonetMemoryCache<TItem>: IEonetMemoryCache<TItem>
    {
        private readonly int _slidingExpirationMinutes = 10;
        private readonly int _absoluteExpirationMinutes = 60;

        private readonly MemoryCacheEntryOptions _cacheEntryOptions;

        private IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        private ConcurrentDictionary<object, SemaphoreSlim> _locks = new ConcurrentDictionary<object, SemaphoreSlim>();

        public EonetMemoryCache(IMemoryCache cache, MemoryCacheEntryOptions cacheEntryOptions)
        {
            _cache = cache;
            _cacheEntryOptions = cacheEntryOptions;
        }

        public async Task<TItem> GetOrCreate(object key, Func<Task<TItem>> createItem)
        {
            TItem cacheEntry;

            if (!_cache.TryGetValue(key, out cacheEntry))// Look for cache key.
            {
                SemaphoreSlim mylock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

                await mylock.WaitAsync();
                try
                {
                    if (!_cache.TryGetValue(key, out cacheEntry))
                    {
                        var policy = new MemoryCacheEntryOptions
                        {
                            SlidingExpiration = TimeSpan.FromMinutes(_slidingExpirationMinutes), // TODO: Move to app.config
                            AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(_absoluteExpirationMinutes) // TODO: Move to app.config
                        };

                        // Key not in cache, so get data.
                        cacheEntry = await createItem();
                        _cache.Set(key, cacheEntry, _cacheEntryOptions);
                    }
                }
                finally
                {
                    mylock.Release();
                }
            }
            return cacheEntry;
        }
    }
}
