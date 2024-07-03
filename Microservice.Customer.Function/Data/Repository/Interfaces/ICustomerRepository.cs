namespace Microservice.Customer.Function.Data.Repository.Interfaces;

public interface ICustomerRepository
{
    Task<Domain.Customer> AddAsync(Domain.Customer customer);    
    Task<bool> CustomerExistsAsync(string email);
    Task<bool> CustomerExistsAsync(Guid id);
}