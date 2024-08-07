using Test;

namespace HumanResources.Domain.Tests.Builders;

internal class EmployeeBuilder : BuilderBase<Employee>
{
    public EmployeeBuilder()
    {
        ConstructItem();
        var startDate = Random.NextDateTimeInFuture();
        SetProperty(e => e.Number, new EmployeeNumber(startDate, Random.Next(1, 1000)));
        SetProperty(e => e.FirstName, Random.NextString());
        SetProperty(e => e.LastName, Random.NextString());
        SetProperty(e => e.StartDate, startDate);
        SetProperty(e => e.EndDate, startDate.AddDays(Random.Next(10, 101)));
    }

    public EmployeeBuilder WithEndDate(DateTime? endDate)
    {
        SetProperty(e => e.EndDate, endDate);
        return this;
    }

    public EmployeeBuilder WithStartDate(DateTime startDate)
    {
        SetProperty(e => e.StartDate, startDate);
        return this;
    }
}