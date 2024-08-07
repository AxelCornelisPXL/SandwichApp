using DevOps.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevOps.Infrastructure;

public class DeveloperConfiguration : IEntityTypeConfiguration<Developer>
{
    public void Configure(EntityTypeBuilder<Developer> builder)
    {
        builder.HasKey(dev => dev.Id);
        builder.Property(dev => dev.FirstName).IsRequired();
        builder.Property(dev => dev.LastName).IsRequired();
        builder.Property(dev => dev.Rating).HasConversion(percentage => percentage.get(), percentage => new Percentage(percentage));
    }
}