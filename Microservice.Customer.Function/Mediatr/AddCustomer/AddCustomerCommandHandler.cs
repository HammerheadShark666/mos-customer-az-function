using AutoMapper;
using MediatR;
using Microservice.Customer.Function.Data.Repository.Interfaces;

namespace Microservice.Customer.Function.MediatR.AddCustomer;

public class AddCustomerCommandHandler(ICustomerRepository customerRepository,
                                       IMapper mapper) : IRequestHandler<AddCustomerRequest, AddCustomerResponse>
{
    private ICustomerRepository _customerRepository { get; set; } = customerRepository;
    private IMapper _mapper { get; set; } = mapper;

    private readonly string CustomerAddedMessage = "Customer Added.";

    public async Task<AddCustomerResponse> Handle(AddCustomerRequest addCustomerRequest, CancellationToken cancellationToken)
    {
        var customer = _mapper.Map<Domain.Customer>(addCustomerRequest);
        await _customerRepository.AddAsync(customer);

        return new AddCustomerResponse(CustomerAddedMessage);
    }
}