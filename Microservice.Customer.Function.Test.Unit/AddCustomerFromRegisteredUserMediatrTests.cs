using FluentValidation;
using MediatR;
using Microservice.Customer.Function.Data.Repository.Interfaces;
using Microservice.Customer.Function.Helpers;
using Microservice.Customer.Function.MediatR.AddCustomer;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Reflection;

namespace Microservice.Customer.Function.Test.Unit;

[TestFixture]
public class AddCustomerFromRegisteredUserMediatrTests
{
    private Mock<ICustomerRepository> customerRepositoryMock = new Mock<ICustomerRepository>();
    private ServiceCollection services = new ServiceCollection();
    private ServiceProvider serviceProvider;
    private IMediator mediator;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        services.AddValidatorsFromAssemblyContaining<AddCustomerValidator>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AddCustomerCommandHandler).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        services.AddScoped<ICustomerRepository>(sp => customerRepositoryMock.Object);
        services.AddAutoMapper(Assembly.GetAssembly(typeof(AddCustomerMapper)));

        serviceProvider = services.BuildServiceProvider();
        mediator = serviceProvider.GetRequiredService<IMediator>();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        services.Clear();
        serviceProvider.Dispose();
    }

    [Test]
    public async Task Customer_added_return_success_message()
    {
        var customer = new Domain.Customer() { Id = Guid.NewGuid(), Email = "ValidEmail@hotmail.com", Surname = "TestSurname", FirstName = "TestFirstName" };

        customerRepositoryMock
                .Setup(x => x.CustomerExistsAsync("ValidEmail@hotmail.com"))
                .Returns(Task.FromResult(false));

        customerRepositoryMock
                .Setup(x => x.AddAsync(customer))
                .Returns(Task.FromResult(customer));

        var addCustomerRequest = new AddCustomerRequest(Guid.NewGuid(), "ValidEmail@hotmail.com", "TestSurname", "TestFirstName");

        var actualResult = await mediator.Send(addCustomerRequest);
        var expectedResult = "Customer Added.";

        Assert.That(actualResult.message, Is.EqualTo(expectedResult));
    }

    [Test]
    public void Customer_not_added_id_exists_return_exception_fail_message()
    {
        var customerId = Guid.NewGuid();

        customerRepositoryMock
                .Setup(x => x.CustomerExistsAsync(customerId))
                .Returns(Task.FromResult(true));

        var addCustomerRequest = new AddCustomerRequest(customerId, "ValidEmail@hotmail.com", "TestSurname", "TestFirstName");

        var validationException = Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await mediator.Send(addCustomerRequest);
        });

        Assert.That(validationException.Errors.Count, Is.EqualTo(1));
        Assert.That(validationException.Errors.ElementAt(0).ErrorMessage, Is.EqualTo("Customer with this id already exists"));
    }

    [Test]
    public void Customer_not_added_email_exists_return_exception_fail_message()
    {   
        customerRepositoryMock
                .Setup(x => x.CustomerExistsAsync("InvalidEmail@hotmail.com"))
                .Returns(Task.FromResult(true));  

        var addCustomerRequest = new AddCustomerRequest(Guid.NewGuid(), "InvalidEmail@hotmail.com", "TestSurname", "TestFirstName");

        var validationException = Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await mediator.Send(addCustomerRequest);
        });
         
        Assert.That(validationException .Errors.Count, Is.EqualTo(1));
        Assert.That(validationException.Errors.ElementAt(0).ErrorMessage, Is.EqualTo("Customer with this email already exists")); 
    }


    [Test]
    public void Customer_not_added_invalid_email_return_exception_fail_message()
    {
        customerRepositoryMock
                .Setup(x => x.CustomerExistsAsync("InvalidEmail"))
                .Returns(Task.FromResult(false));

        var addCustomerRequest = new AddCustomerRequest(Guid.NewGuid(), "InvalidEmail", "TestSurname", "TestFirstName");

        var validationException = Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await mediator.Send(addCustomerRequest);
        });

        Assert.That(validationException.Errors.Count, Is.EqualTo(1));
        Assert.That(validationException.Errors.ElementAt(0).ErrorMessage, Is.EqualTo("Invalid Email."));
    }

    [Test]
    public void Customer_not_added_invalid_surname_firstname_return_exception_fail_message()
    {
        customerRepositoryMock
                .Setup(x => x.CustomerExistsAsync("ValidEmail@hotmail.com"))
                .Returns(Task.FromResult(false));

        var addCustomerRequest = new AddCustomerRequest(Guid.NewGuid(), "ValidEmail@hotmail.com", "TestSurnameTestSurnameTestSurnameTestSurnameTestSurnameTestSurname", "TestFirstNameTestFirstNameTestFirstNameTestFirstNameTestFirstName");

        var validationException = Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await mediator.Send(addCustomerRequest);
        });

        Assert.That(validationException.Errors.Count, Is.EqualTo(2));
        Assert.That(validationException.Errors.ElementAt(0).ErrorMessage, Is.EqualTo("Surname length between 1 and 30."));
        Assert.That(validationException.Errors.ElementAt(1).ErrorMessage, Is.EqualTo("First name length between 1 and 30."));
    }

    [Test]
    public void Customer_not_added_no_email_surname_firstname_return_exception_fail_message()
    {
        customerRepositoryMock
                .Setup(x => x.CustomerExistsAsync("ValidEmail@hotmail.com"))
                .Returns(Task.FromResult(false));

        var addCustomerRequest = new AddCustomerRequest(Guid.NewGuid(), "", "", "");

        var validationException = Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await mediator.Send(addCustomerRequest);
        });

        Assert.That(validationException.Errors.Count, Is.EqualTo(7));
        Assert.That(validationException.Errors.ElementAt(0).ErrorMessage, Is.EqualTo("Email is required."));
        Assert.That(validationException.Errors.ElementAt(1).ErrorMessage, Is.EqualTo("Email length between 8 and 150."));
        Assert.That(validationException.Errors.ElementAt(2).ErrorMessage, Is.EqualTo("Invalid Email."));    
        Assert.That(validationException.Errors.ElementAt(3).ErrorMessage, Is.EqualTo("Surname is required."));
        Assert.That(validationException.Errors.ElementAt(4).ErrorMessage, Is.EqualTo("Surname length between 1 and 30."));
        Assert.That(validationException.Errors.ElementAt(5).ErrorMessage, Is.EqualTo("First name is required."));
        Assert.That(validationException.Errors.ElementAt(6).ErrorMessage, Is.EqualTo("First name length between 1 and 30."));
    } 
}