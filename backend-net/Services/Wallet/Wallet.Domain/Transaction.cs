namespace Wallet.Domain;

public class Transaction
{
    public readonly KwCoin _Ammount;
    public readonly Type _type;

    public enum Type
    {
        Deposit,
        Payment
    }

    public Transaction(KwCoin amount, Type type)
    {
        _Ammount = amount;
        _type = type;
    }
}