using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using RepositoryWithCaching.Application;
using RepositoryWithCaching.Infrastructure.Persistence;
using RepositoryWithCaching.Infrastructure.Shared;
using RepositoryWithCaching.WebApi.Extensions;

namespace RepositoryWithCaching.WebApi
{
    public class Startup
    {
        public IConfiguration Config { get; }

        public Startup(IConfiguration configuration)
        {
            Config = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationLayer(Config);
            services.AddPersistenceInfrastructure(Config);
            services.AddSharedInfrastructure(Config);
            services.AddSwaggerExtension();
            services.AddControllers();
            services.AddApiVersioningExtension();
            services.AddHealthChecks();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseSwaggerExtension();
            app.UseHealthChecks("/health");

            app.AddSharedInfrastructureApplicationPipeline();

            app.UseEndpoints(endpoints =>
             {
                 endpoints.MapControllers();
             });
        }
    }
}