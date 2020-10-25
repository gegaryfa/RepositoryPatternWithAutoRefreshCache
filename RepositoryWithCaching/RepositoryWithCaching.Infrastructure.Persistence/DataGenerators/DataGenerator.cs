using System;
using System.Collections.Generic;
using System.IO;
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
            using var r = new StreamReader($"data/sql/{filename}.json");
            var json = r.ReadToEnd();
            return JsonConvert.DeserializeObject<List<Customer>>(json);
        }
    }
}