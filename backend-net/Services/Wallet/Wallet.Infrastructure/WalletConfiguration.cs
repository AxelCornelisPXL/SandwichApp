using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Wallet.Infrastructure;

public class WalletConfiguration : IEntityTypeConfiguration<Domain.Wallet>
{
    public void Configure(EntityTypeBuilder<Domain.Wallet> builder)
    {
        builder.HasKey(wallet => wallet.Owner);
    }
}