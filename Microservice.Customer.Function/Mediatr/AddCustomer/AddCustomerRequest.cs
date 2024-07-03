using MediatR;

namespace Microservice.Customer.Function.MediatR.AddCustomer;

public record AddCustomerRequest(Guid Id, string Email, string Surname, string FirstName) : IRequest<AddCustomerResponse>;