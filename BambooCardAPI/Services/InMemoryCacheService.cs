using Microsoft.Extensions.Caching.Memory;

namespace BambooCardAPI.Services
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public InMemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public void Set<T>(string key, T value, TimeSpan expirationTime)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(expirationTime);

            _memoryCache.Set(key, value, cacheEntryOptions);
        }

        public bool Remove(string key)
        {
            try
            {
                _memoryCache.Remove(key);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
