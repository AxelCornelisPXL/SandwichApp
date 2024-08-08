namespace Wallet.Infrastructure;

public class WalletDbRepository : IWalletRepository
{
    private readonly WalletContext _ctx;

    public WalletDbRepository(WalletContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<Domain.Wallet?> getByEmployee(string employeenumber)
    {
        var ret = await _ctx.Wallets.FindAsync();
        return ret;
    }
}