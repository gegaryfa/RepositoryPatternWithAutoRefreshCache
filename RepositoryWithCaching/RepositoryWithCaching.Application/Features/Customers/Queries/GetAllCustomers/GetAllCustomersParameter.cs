using RepositoryWithCaching.Application.Parameters;

namespace RepositoryWithCaching.Application.Features.Customers.Queries.GetAllCustomers
{
    public class GetAllCustomersParameter : RequestParameter
    {
        public string ToAddProp { get; set; }
    }
}