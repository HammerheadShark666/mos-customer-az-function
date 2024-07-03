using FluentValidation;
using Microservice.Customer.Function.Data.Repository.Interfaces;

namespace Microservice.Customer.Function.MediatR.AddCustomer;

public class AddCustomerValidator : AbstractValidator<AddCustomerRequest>
{
    private readonly ICustomerRepository _customerRepository;

    public AddCustomerValidator(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;

        RuleFor(registerUserRequest => registerUserRequest).MustAsync(async (registerUserRequest, cancellation) => {
            return await CustomerIdExists(registerUserRequest.Id);
        }).WithMessage("Customer with this id already exists");

        RuleFor(registerUserRequest => registerUserRequest.Email)
                .NotEmpty().WithMessage("Email is required.")
                .Length(8, 150).WithMessage("Email length between 8 and 150.")
                .EmailAddress().WithMessage("Invalid Email."); 

        RuleFor(registerUserRequest => registerUserRequest).MustAsync(async (registerUserRequest, cancellation) => {
            return await EmailExists(registerUserRequest);
        }).WithMessage("Customer with this email already exists");

        RuleFor(updateCustomerRequest => updateCustomerRequest.Surname)
                .NotEmpty().WithMessage("Surname is required.")
                .Length(1, 30).WithMessage("Surname length between 1 and 30.");

        RuleFor(updateCustomerRequest => updateCustomerRequest.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .Length(1, 30).WithMessage("First name length between 1 and 30.");
    }

    protected async Task<bool> EmailExists(AddCustomerRequest registeredUserRequest)
    { 
        return !await _customerRepository.CustomerExistsAsync(registeredUserRequest.Email);
    }

    protected async Task<bool> CustomerIdExists(Guid customerId)
    {
        return !await _customerRepository.CustomerExistsAsync(customerId);
    }    
}