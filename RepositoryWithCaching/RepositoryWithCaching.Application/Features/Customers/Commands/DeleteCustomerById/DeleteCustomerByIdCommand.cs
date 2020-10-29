using System.Threading;
using System.Threading.Tasks;

using MediatR;

using RepositoryWithCaching.Application.Exceptions;
using RepositoryWithCaching.Application.Interfaces.Repositories;

namespace RepositoryWithCaching.Application.Features.Customers.Commands.DeleteCustomerById
{
    public class DeleteCustomerByIdCommand : IRequest<int>
    {
        public int Id { get; set; }
    }

    public class DeleteCustomerByIdCommandHandler : IRequestHandler<DeleteCustomerByIdCommand, int>
    {
        private readonly ICustomerRepository _customerRepository;

        public DeleteCustomerByIdCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<int> Handle(DeleteCustomerByIdCommand command, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(command.Id);
            if (customer == null)
            {
                throw new ApiException($"Customer Not Found."); // Todo: Re-evaluate how to handle or what exception to throw
            }

            await _customerRepository.DeleteAsync(customer);
            return customer.Id;
        }
    }
}