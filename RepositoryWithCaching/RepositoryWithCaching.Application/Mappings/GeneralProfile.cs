using AutoMapper;

using RepositoryWithCaching.Application.Features.Customers.Queries.GetAllCustomers;
using RepositoryWithCaching.Domain.Entities;

namespace RepositoryWithCaching.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Customer, GetAllCustomersViewModel>().ReverseMap();
        }
    }
}