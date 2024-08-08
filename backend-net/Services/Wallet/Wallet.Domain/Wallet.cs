using System.ComponentModel.DataAnnotations;
using Domain;

namespace Wallet.Domain;

public class Wallet : Entity
{
    [Key] public Owner Owner { get; private set; }
    public KwCoin Coins { get; private set; }

    private readonly List<Transaction> _transactions;
    public IReadOnlyList<Transaction> Transactions => _transactions;

    private Wallet(string employeeNumber)
    {
        Coins = 5;
        _transactions = new List<Transaction>();
        Owner = new Owner(employeeNumber);
    }

    public static Wallet CreateNewWallet(string employeenumber)
    {
        return new Wallet(employeenumber);
    }

    public void Deposit(int euros)
    {
        var trans = new Transaction(euros, Transaction.Type.Deposit);
        _transactions.Add(trans);
        Coins += trans._Ammount;
    }

    public void Pay(KwCoin coins)
    {
        var trans = new Transaction(coins, Transaction.Type.Payment);
        _transactions.Add(trans);
        Coins -= trans._Ammount;
    }
    
    protected override IEnumerable<object> GetIdComponents()
    {
        yield return Owner.ToString();
    }
}