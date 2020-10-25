using System;

namespace RepositoryWithCaching.Application.Features.Customers.Queries.GetAllCustomers
{
    public class GetAllCustomersViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}