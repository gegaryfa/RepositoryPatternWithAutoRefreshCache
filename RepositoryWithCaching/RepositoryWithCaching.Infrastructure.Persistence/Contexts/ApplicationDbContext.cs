using Microsoft.EntityFrameworkCore;

using RepositoryWithCaching.Domain.Entities;

namespace RepositoryWithCaching.Infrastructure.Persistence.Contexts
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Customer> Customers { get; set; }
    }
}