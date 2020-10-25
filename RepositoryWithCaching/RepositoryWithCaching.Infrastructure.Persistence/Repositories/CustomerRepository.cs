using Microsoft.EntityFrameworkCore;

using RepositoryWithCaching.Application.Interfaces.Repositories;
using RepositoryWithCaching.Domain.Entities;
using RepositoryWithCaching.Infrastructure.Persistence.Contexts;

namespace RepositoryWithCaching.Infrastructure.Persistence.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly DbSet<Customer> _customer;

        public CustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _customer = dbContext.Set<Customer>();
        }
    }
}