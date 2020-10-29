using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using RepositoryWithCaching.Domain.Entities;
using RepositoryWithCaching.Infrastructure.Persistence.Contexts;

namespace RepositoryWithCaching.Infrastructure.Persistence.DataGenerators
{
    public class DataGenerator
    {
        private static bool InDocker => Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

        public static async Task InitializeInMemoryDb(IServiceProvider serviceProvider)
        {
            await using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            var customers = GetSeedCustomers("SeedInMemCustomers");

            // Look for any customers.
            if (await context.Customers.AnyAsync())
            {
                return;   // Data was already seeded
            }

            await context.Customers.AddRangeAsync(customers);

            await context.SaveChangesAsync();
        }

        public static async Task InitializeRelationalDb(ApplicationDbContext orderContext, int? retry = 0)
        {
            if (retry == null)
            {
                return;
            }
            var retryForAvailability = retry.Value;

            try
            {
                //Need to run <add-migration Initial> on the infra project the first time
                await orderContext.Database.MigrateAsync();

                if (!await orderContext.Customers.AnyAsync())
                {
                    await orderContext.Customers.AddRangeAsync(GetSeedCustomers("SeedRelCustomers"));

                    await orderContext.SaveChangesAsync();
                }
            }
            catch (Exception exception)
            {
                // TODO: log it

                if (retryForAvailability < 3)
                {
                    retryForAvailability++;
                    await InitializeRelationalDb(orderContext, retryForAvailability);
                }
            }
        }

        private static IEnumerable<Customer> GetSeedCustomers(string filename)
        {
            var pathToFile = ConstructPathToFile(filename);

            using var r = new StreamReader(pathToFile);
            var json = r.ReadToEnd();
            return JsonConvert.DeserializeObject<List<Customer>>(json);
        }

        private static string ConstructPathToFile(string filename)
        {
            if (InDocker)
            {
                return $"data/sql/{filename}.json";
            }
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePathRelativeToAssembly = Path.Combine(assemblyPath, @$".\data/sql/{filename}.json");
            var normalizedPath = Path.GetFullPath(filePathRelativeToAssembly);
            return normalizedPath;
        }
    }
}