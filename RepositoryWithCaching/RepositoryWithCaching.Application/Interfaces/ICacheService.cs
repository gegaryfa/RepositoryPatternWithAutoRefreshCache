using System;
using System.Threading.Tasks;

namespace RepositoryWithCaching.Application.Interfaces
{
    public interface ICacheService
    {
        Task<T> Get<T>(string cacheKey, Func<Task<T>> factory);

        Task Set<T>(string cacheKey, T value);

        Task Remove(string cacheKey);
    }
}