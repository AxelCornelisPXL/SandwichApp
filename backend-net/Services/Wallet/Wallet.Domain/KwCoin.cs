namespace Wallet.Domain;

public class KwCoin
{
    public const decimal KW_TO_EUR = 1 * 5;
    public const decimal EUR_TO_KW = 1 / KW_TO_EUR;

    private decimal _count = 1;

    public KwCoin(decimal count)
    {
        _count = count;
    }

    public static implicit operator KwCoin(int euros)
    {
        return new KwCoin(EUR_TO_KW * euros);
    }

    public static KwCoin operator +(KwCoin first, KwCoin second)
    {
        return new KwCoin(first._count + second._count);
    }

    public static KwCoin operator -(KwCoin first, KwCoin second)
    {
        return new KwCoin(first._count - second._count);
    }
}