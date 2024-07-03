﻿using Microservice.Customer.Function.Data.Contexts;
using Microservice.Customer.Function.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Customer.Function.Data.Repository;

public class CustomerRepository(IDbContextFactory<CustomerDbContext> dbContextFactory) : ICustomerRepository
{    
    public IDbContextFactory<CustomerDbContext> _dbContextFactory { get; set; } = dbContextFactory;

    public async Task<Domain.Customer> AddAsync(Domain.Customer customer)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        await db.AddAsync(customer);
        db.SaveChanges(); 

        return customer;
    } 
 
    public async Task<bool> CustomerExistsAsync(string email)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        return await db.Customer.AnyAsync(x => x.Email.Equals(email));
    }
 
    public async Task<bool> CustomerExistsAsync(Guid id)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        return await db.Customer.AnyAsync(x => x.Id.Equals(id));
    }
}