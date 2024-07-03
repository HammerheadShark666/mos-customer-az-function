using FluentValidation;
using MediatR;
using Microservice.Customer.Function.Data.Context;
using Microservice.Customer.Function.Data.Contexts;
using Microservice.Customer.Function.Data.Repository;
using Microservice.Customer.Function.Data.Repository.Interfaces;
using Microservice.Customer.Function.Helpers;
using Microservice.Customer.Function.MediatR.AddCustomer;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

var host = new HostBuilder()   
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration(c =>
    {
        c.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    })
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

        services.AddValidatorsFromAssemblyContaining<AddCustomerValidator>();
        services.AddMediatR(_ => _.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        services.AddAutoMapper(Assembly.GetAssembly(typeof(AddCustomerMapper)));
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddSingleton<DefaultData>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddMemoryCache();

        services.AddDbContextFactory<CustomerDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString(Constants.DatabaseConnectionString),
            options => options.EnableRetryOnFailure()));
    })
    .Build();

await host.RunAsync();