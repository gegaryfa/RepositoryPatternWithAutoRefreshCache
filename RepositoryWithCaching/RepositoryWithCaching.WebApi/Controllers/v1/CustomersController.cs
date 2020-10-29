using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using RepositoryWithCaching.Application.Features.Customers.Commands.CreateCustomer;
using RepositoryWithCaching.Application.Features.Customers.Commands.DeleteCustomerById;
using RepositoryWithCaching.Application.Features.Customers.Commands.UpdateCustomer;
using RepositoryWithCaching.Application.Features.Customers.Queries.GetAllCustomers;
using RepositoryWithCaching.Application.Features.Customers.Queries.GetCustomerById;
using RepositoryWithCaching.Application.Interfaces.Repositories;

namespace RepositoryWithCaching.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class CustomersController : BaseApiController
    {
        private readonly ICustomerRepository _repository;

        public CustomersController(ICustomerRepository repository)
        {
            this._repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllCustomersParameter props)
        {
            return Ok(await Mediator.Send(new GetAllCustomersQuery()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var customer = await Mediator.Send(new GetCustomerByIdQuery { Id = id });
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UpdateCustomerCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateCustomerCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteCustomerByIdCommand { Id = id }));
        }
    }
}