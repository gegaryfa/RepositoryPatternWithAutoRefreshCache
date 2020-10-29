using System;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using RepositoryWithCaching.Application.Configurations;
using RepositoryWithCaching.Application.Interfaces;

namespace RepositoryWithCaching.Infrastructure.Shared.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly CacheConfiguration _cacheConfig;
        public readonly DistributedCacheEntryOptions _cacheOption;

        public RedisCacheService(IDistributedCache distributedCache,
            IOptions<CacheConfiguration> cacheConfig)
        {
            _distributedCache = distributedCache;

            _cacheConfig = cacheConfig.Value;
            _cacheOption = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddHours(_cacheConfig.AbsoluteExpirationInHours))
                .SetSlidingExpiration(TimeSpan.FromMinutes(_cacheConfig.SlidingExpirationInMinutes));
        }

        public async Task Remove(string cacheKey)
        {
            await _distributedCache.RemoveAsync(cacheKey);
        }

        public async Task Set<T>(string cacheKey, T value)
        {
            var serializedValue = JsonConvert.SerializeObject(value);
            var valueInBytes = Encoding.UTF8.GetBytes(serializedValue);
            await _distributedCache.SetAsync(cacheKey, valueInBytes, _cacheOption);
        }

        public async Task<T> Get<T>(string cacheKey, Func<Task<T>> factory)
        {
            var cachedValue = await _distributedCache.GetAsync(cacheKey);
            if (cachedValue == null)
            {
                var result = await factory();
                await this.Set(cacheKey, result);
                return result;
            }

            var serializedResult = Encoding.UTF8.GetString(cachedValue);
            var value = JsonConvert.DeserializeObject<T>(serializedResult);
            return value;
        }
    }
}