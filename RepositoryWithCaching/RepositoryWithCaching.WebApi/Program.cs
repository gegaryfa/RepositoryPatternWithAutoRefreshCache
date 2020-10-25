using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using RepositoryWithCaching.Infrastructure.Persistence.Contexts;
using RepositoryWithCaching.Infrastructure.Persistence.DataGenerators;

using Serilog;

namespace RepositoryWithCaching.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //Read Configuration from appSettings
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            //InitializeInMemoryDb Logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            var host = CreateHostBuilder(args).Build();

            if (config.GetValue<bool>("UseInMemoryDatabase"))
            {
                using var scope = host.Services.CreateScope();

                var services = scope.ServiceProvider;

                // Call the DataGenerator to create sample data
                await DataGenerator.InitializeInMemoryDb(services);
            }
            else
            {
                await CreateAndSeedDatabase(host);
            }

            await host.RunAsync();
        }

        private static async Task CreateAndSeedDatabase(IHost host)
        {
            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;

            try
            {
                var orderContext = services.GetRequiredService<ApplicationDbContext>();
                await DataGenerator.InitializeRelationalDb(orderContext);
            }
            catch (Exception exception)
            {
                // TODO: Log it.
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog() //Uses Serilog instead of default .NET Logger
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}