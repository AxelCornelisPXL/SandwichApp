using HumanResources.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HumanResources.Infrastructure;

internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Number);
        builder.Property(e => e.Number).HasMaxLength(11).HasConversion(input => input.ToString(), output => new string(output));
        builder.Property(e => e.LastName).IsRequired();
        builder.Property(e => e.FirstName).IsRequired();
    }
}