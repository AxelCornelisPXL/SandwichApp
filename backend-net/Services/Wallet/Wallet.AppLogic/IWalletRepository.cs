namespace Wallet.Infrastructure;

public interface IWalletRepository
{
    Task<Domain.Wallet?> getByEmployee(string employeenumber);
}