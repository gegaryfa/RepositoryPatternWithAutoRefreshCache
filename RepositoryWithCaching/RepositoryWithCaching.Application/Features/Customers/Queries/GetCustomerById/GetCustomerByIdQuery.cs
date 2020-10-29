using System.Threading;
using System.Threading.Tasks;

using MediatR;

using RepositoryWithCaching.Application.Interfaces.Repositories;
using RepositoryWithCaching.Domain.Entities;

namespace RepositoryWithCaching.Application.Features.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQuery : IRequest<Customer>
    {
        public int Id { get; set; }
    }

    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Customer>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
        {
            this._customerRepository = customerRepository;
        }

        public async Task<Customer> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(query.Id);

            return customer;
        }
    }
}