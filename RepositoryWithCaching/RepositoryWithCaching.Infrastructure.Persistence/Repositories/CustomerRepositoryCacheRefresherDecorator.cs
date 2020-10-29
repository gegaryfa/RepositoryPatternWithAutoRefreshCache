using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Hangfire;

using Microsoft.EntityFrameworkCore;

using RepositoryWithCaching.Application.Enums;
using RepositoryWithCaching.Application.Interfaces;
using RepositoryWithCaching.Application.Interfaces.Repositories;
using RepositoryWithCaching.Domain.Entities;
using RepositoryWithCaching.Infrastructure.Persistence.Contexts;

namespace RepositoryWithCaching.Infrastructure.Persistence.Repositories
{
    public class CustomerRepositoryCacheRefresherDecorator : ICustomerRepository, ICacheRefresher
    {
        private readonly ICustomerRepository _customerRepository;

        private const CacheTechnology CacheTech = Application.Enums.CacheTechnology.Memory;
        private readonly string _cacheKey = $"{typeof(CustomerRepository)}";
        private readonly Func<CacheTechnology, ICacheService> _cacheService;

        private readonly ApplicationDbContext _dbContext;

        public CustomerRepositoryCacheRefresherDecorator(ICustomerRepository customerRepository, Func<CacheTechnology, ICacheService> cacheService, ApplicationDbContext dbContext)
        {
            this._customerRepository = customerRepository;
            this._cacheService = cacheService;
            this._dbContext = dbContext;
        }

        /// <summary>
        /// We will not be caching the Result of GetByID because it is usually a very fast performing query.
        /// We will need caching only when the user requests for all Data or modifies any.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Customer> GetByIdAsync(int id)
        {
            return await this._customerRepository.GetByIdAsync(id);
        }

        public async Task<IReadOnlyList<Customer>> GetAllAsync()
        {
            var cachedList = await _cacheService(CacheTech)
                .Get(_cacheKey, async () => await this._customerRepository.GetAllAsync());

            return cachedList;
        }

        public async Task<Customer> AddAsync(Customer entity)
        {
            await this._customerRepository.AddAsync(entity);
            BackgroundJob.Enqueue(() => RefreshCache());

            return entity;
        }

        public async Task UpdateAsync(Customer entity)
        {
            await this._customerRepository.UpdateAsync(entity);
            BackgroundJob.Enqueue(() => RefreshCache());
        }

        public async Task DeleteAsync(Customer entity)
        {
            await this._customerRepository.DeleteAsync(entity);
            BackgroundJob.Enqueue(() => RefreshCache());
        }

        public async Task RefreshCache()
        {
            await _cacheService(CacheTech).Remove(_cacheKey);
            var cachedList = await _dbContext.Set<Customer>().ToListAsync();
            await _cacheService(CacheTech).Set(_cacheKey, cachedList);
        }
    }
}