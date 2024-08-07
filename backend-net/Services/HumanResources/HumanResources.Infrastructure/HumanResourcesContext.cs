using HumanResources.Domain;
using Microsoft.EntityFrameworkCore;

namespace HumanResources.Infrastructure;

internal class HumanResourcesContext : DbContext
{
    public HumanResourcesContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new EmployeeConfiguration().Configure(modelBuilder.Entity<Employee>());
    }
}