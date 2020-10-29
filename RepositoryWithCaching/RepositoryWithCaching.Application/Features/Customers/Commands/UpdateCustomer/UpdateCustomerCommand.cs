using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using RepositoryWithCaching.Application.Exceptions;
using RepositoryWithCaching.Application.Interfaces.Repositories;

namespace RepositoryWithCaching.Application.Features.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, int>
    {
        private readonly ICustomerRepository _customerRepository;

        public UpdateCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<int> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(command.Id);

            if (customer == null)
            {
                throw new ApiException($"Customer Not Found."); // Todo: Re-evaluate how to handle or what exception to throw
            }
            else
            {
                customer.Contact = command.Contact;
                customer.DateOfBirth = command.DateOfBirth;
                customer.Email = command.Email;
                customer.FirstName = command.FirstName;
                customer.LastName = command.LastName;
                await _customerRepository.UpdateAsync(customer);

                return customer.Id;
            }
        }
    }
}