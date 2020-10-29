using System.Reflection;

using AutoMapper;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RepositoryWithCaching.Application.Configurations;

namespace RepositoryWithCaching.Application
{
    public static class ServiceExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration config)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.Configure<CacheConfiguration>(x => config.GetSection("key").Bind(x));
        }
    }
}