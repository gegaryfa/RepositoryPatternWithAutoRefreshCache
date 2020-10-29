using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using RepositoryWithCaching.Application.Configurations;
using RepositoryWithCaching.Application.Interfaces;

namespace RepositoryWithCaching.Infrastructure.Shared.Services
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly CacheConfiguration _cacheConfig;
        private readonly MemoryCacheEntryOptions _cacheOptions;

        public MemoryCacheService(IMemoryCache memoryCache, IOptions<CacheConfiguration> cacheConfig)
        {
            _memoryCache = memoryCache;
            _cacheConfig = cacheConfig.Value;
            if (_cacheConfig != null)
            {
                _cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(_cacheConfig.AbsoluteExpirationInHours),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromMinutes(_cacheConfig.SlidingExpirationInMinutes)
                };
            }
        }

        public async Task<T> Get<T>(string cacheKey, Func<Task<T>> factory)
        {
            var cachedResult = _memoryCache.Get<T>(cacheKey);

            if (cachedResult == null)
            {
                var result = await factory();
                _memoryCache.Set(cacheKey, result);
                return result;
            }
            return cachedResult;
        }

        public Task Set<T>(string cacheKey, T value)
        {
            _memoryCache.Set(cacheKey, value, _cacheOptions);
            return Task.CompletedTask;
        }

        public Task Remove(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
            return Task.CompletedTask;
        }
    }
}