using Domain;

namespace Wallet.Domain;

public class Owner : ValueObject<Owner>
{
    private readonly string EmployeeNumber;

    private Owner()
    {
        //ef core
    }

    public Owner(string employeeNumber)
    {
        EmployeeNumber = employeeNumber;
    }

    public override string ToString()
    {
        return EmployeeNumber;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return EmployeeNumber;
    }

    public static implicit operator Owner(string employeeNumber)
    {
        return new Owner(employeeNumber);
    }

    public static implicit operator string(Owner employeeNumber)
    {
        return employeeNumber.ToString();
    }
}