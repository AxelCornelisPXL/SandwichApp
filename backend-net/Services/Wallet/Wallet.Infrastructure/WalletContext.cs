using Microsoft.EntityFrameworkCore;

namespace Wallet.Infrastructure;

public class WalletContext : DbContext
{
    public DbSet<Domain.Wallet>? Wallets { get; set; }

    public WalletContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new WalletConfiguration().Configure((modelBuilder.Entity<Domain.Wallet>()));
    }
}