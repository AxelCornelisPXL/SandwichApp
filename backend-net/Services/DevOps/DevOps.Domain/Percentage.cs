using Domain;
using System.Globalization;

namespace DevOps.Domain;

public class Percentage : ValueObject<Percentage>
{
    private readonly double _value;

    public Percentage(double value)
    {
        Contracts.Require(value is >= 0.0d and <= 1.0d, "Value has to be within bounds [0.0-1.0]");
        _value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return _value;
    }

    public static implicit operator Percentage(string value)
    {
        return new Percentage(double.Parse(value));
    }

    public static implicit operator string(Percentage number)
    {
        return Format(number._value);
    }

    public static implicit operator double(Percentage number)
    {
        return number._value;
    }

    public static implicit operator Percentage(double value)
    {
        return new Percentage(value);
    }

    public override string ToString()
    {
        return Format(_value);
    }

    private static string Format(double value)
    {
        //bypass my english pc's number formatting to conform to the dutch number system convention
        var cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        cultureInfo.NumberFormat.NumberDecimalSeparator = ",";

        return string.Format(cultureInfo, "{0:#,##0.##}%", value * 100d);
    }

    public double get()
    {
        return _value;
    }
}