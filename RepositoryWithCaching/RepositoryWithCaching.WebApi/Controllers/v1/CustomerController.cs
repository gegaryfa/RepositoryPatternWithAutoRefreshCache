using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using RepositoryWithCaching.Application.Features.Customers.Queries.GetAllCustomers;
using RepositoryWithCaching.Application.Features.Customers.Queries.GetCustomerById;
using RepositoryWithCaching.Application.Interfaces.Repositories;
using RepositoryWithCaching.Domain.Entities;

namespace RepositoryWithCaching.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class CustomerController : BaseApiController
    {
        private readonly ICustomerRepository _repository;

        public CustomerController(ICustomerRepository repository)
        {
            this._repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllCustomersParameter props)
        {
            return Ok(await Mediator.Send(new GetAllCustomersQuery()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> Get(int id)
        {
            var customer = await Mediator.Send(new GetCustomerByIdQuery { Id = id });
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }
            await _repository.UpdateAsync(customer);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> Post(Customer customer)
        {
            await _repository.AddAsync(customer);
            return CreatedAtAction("Get", new { id = customer.Id }, customer);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> Delete(int id)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            await _repository.DeleteAsync(customer);
            return customer;
        }
    }
}