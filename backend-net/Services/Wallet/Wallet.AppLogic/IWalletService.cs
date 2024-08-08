namespace Wallet.AppLogic;

public interface IWalletService
{
    Task<Domain.Wallet> CreateNewWallet(string employeenumber);
}