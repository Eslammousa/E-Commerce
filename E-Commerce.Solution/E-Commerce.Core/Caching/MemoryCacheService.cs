
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Core.Caching
{
    public class MemoryCacheService : ICacheService
    {

        private readonly IMemoryCache _cache;
        private readonly ILogger<MemoryCacheService> _logger;
        private readonly HashSet<string> _keys = new();
        private readonly object _lock = new();

        public MemoryCacheService(IMemoryCache cache, ILogger<MemoryCacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }
        public T? Get<T>(string key)
        {
            if (_cache.TryGetValue(key, out T? value))
            {
                _logger.LogInformation("Cache HIT: {Key}", key);
                return value;
            }

            _logger.LogInformation("Cache MISS: {Key}", key);
            return default;
        }

        public void Set<T>(string key, T value, TimeSpan? expiration = null)
        {
            var options = new MemoryCacheEntryOptions();

            if (expiration.HasValue) 
                options.AbsoluteExpirationRelativeToNow = expiration;
            else
                options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10); // default

            // Sliding expiration لو الداتا بتتطلب منها كتير
            options.SlidingExpiration = TimeSpan.FromMinutes(2);

            _cache.Set(key, value, options);

            lock (_lock)
            {
                _keys.Add(key);
            }
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
            lock (_lock) { _keys.Remove(key); }
        }

        public void RemoveByPrefix(string prefix)
        {
            lock (_lock)
            {
                var matchedKeys = _keys.Where(k => k.StartsWith(prefix)).ToList();
                foreach (var key in matchedKeys)
                {
                    _cache.Remove(key);
                    _keys.Remove(key);
                }
            }
        }
    }
}