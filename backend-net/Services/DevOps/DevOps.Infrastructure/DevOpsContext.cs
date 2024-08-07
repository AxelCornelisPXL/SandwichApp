using DevOps.Domain;
using Microsoft.EntityFrameworkCore;

namespace DevOps.Infrastructure;

public class DevOpsContext : DbContext
{
    public DbSet<Developer>? Developers { get; set; }
    public DbSet<Team>? Teams { get; set; }

    public DevOpsContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new DeveloperConfiguration().Configure(modelBuilder.Entity<Developer>());
        new TeamConfiguration().Configure(modelBuilder.Entity<Team>());
    }
}