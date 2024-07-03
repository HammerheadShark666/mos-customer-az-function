using AutoMapper;

namespace Microservice.Customer.Function.MediatR.AddCustomer;

public class AddCustomerMapper : Profile
{
    public AddCustomerMapper()
    {
        base.CreateMap<AddCustomerRequest, Microservice.Customer.Function.Domain.Customer>();
    }
}