using System;

using Hangfire;
using Hangfire.MemoryStorage;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RepositoryWithCaching.Application.Configurations;
using RepositoryWithCaching.Application.Enums;
using RepositoryWithCaching.Application.Interfaces;
using RepositoryWithCaching.Infrastructure.Shared.Services;

namespace RepositoryWithCaching.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<CacheConfiguration>(config.GetSection("CacheConfiguration"));

            // For In-Memory Caching
            services.AddMemoryCache();
            services.AddTransient<MemoryCacheService>();

            // For Redis Caching
            services.AddTransient<RedisCacheService>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = config.GetConnectionString("Redis");
            });

            services.AddTransient<Func<CacheTechnology, ICacheService>>(serviceProvider => key =>
            {
                switch (key)
                {
                    case CacheTechnology.Memory:
                        return serviceProvider.GetService<MemoryCacheService>();

                    case CacheTechnology.Redis:
                        return serviceProvider.GetService<RedisCacheService>();

                    default:
                        return serviceProvider.GetService<MemoryCacheService>();
                }
            });

            ConfigureHangFire(services, config);
        }

        private static void ConfigureHangFire(IServiceCollection services, IConfiguration config)
        {
            if (config.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddHangfire(c => c.UseMemoryStorage());
            }
            else
            {
                services.AddHangfire(x => x.UseSqlServerStorage(config.GetConnectionString("DefaultConnection")));
            }
            services.AddHangfireServer();
        }
    }
}