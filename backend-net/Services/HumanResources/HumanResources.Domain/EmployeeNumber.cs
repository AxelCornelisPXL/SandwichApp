using Domain;

namespace HumanResources.Domain;

public class EmployeeNumber : ValueObject<EmployeeNumber>
{
    public int Year { get; private set; }
    public int Month { get; private set; }
    public int Day { get; private set; }
    public int Sequence { get; private set; }

    private EmployeeNumber()
    {
        //ef core
    }

    public EmployeeNumber(DateTime startDate, int sequence)
    {
        Contracts.Require(sequence >= 1, "The sequence in the employee number must be a positive number");

        Year = startDate.Year;
        Month = startDate.Month;
        Day = startDate.Day;
        Sequence = sequence;
    }

    public EmployeeNumber(string sequence)
    {
        Contracts.Require(!string.IsNullOrEmpty(sequence), "An employee number cannot be empty");
        Contracts.Require(sequence.Length == 11, "An employee number must have exactly 11 characters");
        Contracts.Require(sequence.All(c => char.IsDigit(c)), "An employee number can only contain digits");

        Year = int.Parse(sequence.Substring(0, 4));
        Contracts.Require(Year > 0, "The first 4 digits of an employee number must be a valid year");

        Month = int.Parse(sequence.Substring(4, 2));
        Contracts.Require(Month >= 1 && Month <= 12, "Digits 5 and 6 of an employee number must be a valid month");

        Day = int.Parse(sequence.Substring(6, 2));
        Contracts.Require(Day >= 1 && Day <= 31, "Digits 7 and 8 of an employee number must be a valid day");

        Sequence = int.Parse(sequence.Substring(8, 3));
        Contracts.Require(Sequence >= 1, "The sequence in the employee number must be a positive number");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Year;
        yield return Day;
        yield return Month;
        yield return Sequence;
    }

    public override string ToString()
    {
        return $"{Year:0000}{Month:00}{Day:00}{Sequence:000}";
    }

    public static implicit operator EmployeeNumber(string sequence)
    {
        return new EmployeeNumber(sequence);
    }

    public static implicit operator string(EmployeeNumber number)
    {
        return number.ToString();
    }
}