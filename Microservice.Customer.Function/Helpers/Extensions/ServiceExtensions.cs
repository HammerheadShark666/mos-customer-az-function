﻿using FluentValidation;
using MediatR;
using Microservice.Customer.Function.Data.Context;
using Microservice.Customer.Function.Data.Repository;
using Microservice.Customer.Function.Data.Repository.Interfaces;
using Microservice.Customer.Function.Helpers.Exceptions;
using Microservice.Customer.Function.Helpers.Providers;
using Microservice.Customer.Function.MediatR.AddCustomer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Microservice.Customer.Function.Helpers.Extensions;
public static class ServiceExtensions
{
    public static void ConfigureDependencyInjection(IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetAssembly(typeof(AddCustomerMapper)));
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddSingleton<DefaultData>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }

    public static void ConfigureApplicationInsights(IServiceCollection services)
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    }

    public static void ConfigureMediatr(IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AddCustomerValidator>();
        services.AddMediatR(_ => _.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
    }

    public static void ConfigureSqlServer(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        if (environment.IsProduction())
        {
            var connectionString = configuration.GetConnectionString(Constants.AzureDatabaseConnectionString)
                    ?? throw new DatabaseConnectionStringNotFound("Production database connection string not found.");

            AddDbContextFactory(services, SqlAuthenticationMethod.ActiveDirectoryManagedIdentity, new ProductionAzureSQLProvider(), connectionString);
        }
        else if (environment.IsDevelopment())
        {
            var connectionString = configuration.GetConnectionString(Constants.LocalDatabaseConnectionString)
                    ?? throw new DatabaseConnectionStringNotFound("Development database connection string not found.");

            AddDbContextFactory(services, SqlAuthenticationMethod.ActiveDirectoryServicePrincipal, new DevelopmentAzureSQLProvider(), connectionString);
        }
    }

    public static void ConfigureMemoryCache(IServiceCollection services)
    {
        services.AddMemoryCache();
    }

    public static void ConfigureLogging(IServiceCollection services)
    {
        services.AddLogging(logging =>
        {
            logging.AddConsole();
        });
    }

    private static void AddDbContextFactory(IServiceCollection services, SqlAuthenticationMethod sqlAuthenticationMethod, SqlAuthenticationProvider sqlAuthenticationProvider, string connectionString)
    {
        services.AddDbContextFactory<CustomerDbContext>(options =>
        {
            SqlAuthenticationProvider.SetProvider(
                    sqlAuthenticationMethod,
                    sqlAuthenticationProvider);
            var sqlConnection = new SqlConnection(connectionString);
            options.UseSqlServer(sqlConnection);
        });
    }
}
