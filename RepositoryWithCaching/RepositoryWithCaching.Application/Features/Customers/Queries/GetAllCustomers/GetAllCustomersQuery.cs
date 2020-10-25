using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using RepositoryWithCaching.Application.Interfaces.Repositories;

namespace RepositoryWithCaching.Application.Features.Customers.Queries.GetAllCustomers
{
    public class GetAllCustomersQuery : IRequest<IEnumerable<GetAllCustomersViewModel>>
    {
    }

    public class GetAllProductsQueryHandler : IRequestHandler<GetAllCustomersQuery, IEnumerable<GetAllCustomersViewModel>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public GetAllProductsQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllCustomersViewModel>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _customerRepository.GetAllAsync();
            var customersViewModel = _mapper.Map<IEnumerable<GetAllCustomersViewModel>>(customers);
            return new List<GetAllCustomersViewModel>(customersViewModel);
        }
    }
}