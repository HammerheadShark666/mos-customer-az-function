using Microservice.Customer.Function.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Customer.Function.Data.Contexts;

public class CustomerDbContext : DbContext
{ 
    public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options) { }
 
    public DbSet<Domain.Customer> Customer { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 

		modelBuilder.Entity<Domain.Customer>().HasData(DefaultData.GetCustomerDefaultData());  
    }
}

//EntityFrameworkCore\Add-Migration create-db add-favorite-table
//EntityFrameworkCore\update-database   

//EntityFramework6\Add-Migration
//EntityFramework6\update-database

//dotnet ef migrations add description-column-to-gallery --project PhotographySite
//dotnet ef database update --project PhotographySite

//azurite --silent --location c:\azurite --debug c:\azurite\debug.log