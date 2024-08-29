using AutoMapper;
using MediatR;
using Microservice.Customer.Function.Data.Repository.Interfaces;

namespace Microservice.Customer.Function.MediatR.AddCustomer;

public class AddCustomerCommandHandler(ICustomerRepository customerRepository,
                                       IMapper mapper) : IRequestHandler<AddCustomerRequest, AddCustomerResponse>
{
    private readonly string CustomerAddedMessage = "Customer Added.";

    public async Task<AddCustomerResponse> Handle(AddCustomerRequest addCustomerRequest, CancellationToken cancellationToken)
    {
        var customer = mapper.Map<Domain.Customer>(addCustomerRequest);
        await customerRepository.AddAsync(customer);

        return new AddCustomerResponse(CustomerAddedMessage);
    }
}