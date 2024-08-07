using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain;

namespace HumanResources.Domain;

public class Employee : Entity, IEmployee
{
    public void Dismiss(bool withNotice = true)
    {
        if (!withNotice)
        {
            EndDate = DateTime.Now;
            return;
        }

        Contracts.Require(EndDate == null, "This Employee has already been fired. Their end date is " + EndDate.ToString());

        if (DateTime.Now <= StartDate.AddMonths(3))
            EndDate = DateTime.Now.AddDays(7);
        else if (DateTime.Now <= StartDate.AddMonths(12))
            EndDate = DateTime.Now.AddDays(14);
        else
            EndDate = DateTime.Now.AddDays(28);
    }

    public EmployeeNumber Number { get; private set; }

    public string? LastName { get; private set; }
    public string? FirstName { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }

    private Employee()
    {
        Number = new EmployeeNumber(DateTime.MinValue, 1);
        LastName = string.Empty;
        FirstName = string.Empty;
    }

    protected override IEnumerable<object> GetIdComponents()
    {
        yield return FirstName;
        yield return LastName;
        yield return StartDate;
        yield return EndDate;
        yield return Number;
    }

    internal class Factory : IEmployeeFactory
    {
        public IEmployee CreateNew(string lastName, string firstName, DateTime startDate, int sequence)
        {
            Contracts.Require(startDate >= DateTime.Now.AddYears(-1), "The start date of an employee cannot be more than 1 year in the past");
            Contracts.Require(!string.IsNullOrEmpty(lastName), "The last name of an employee cannot be empty");
            Contracts.Require(lastName.Length >= 2, "The last name of an employee must at least have 2 characters");
            Contracts.Require(!string.IsNullOrEmpty(firstName), "The first name of an employee cannot be empty");
            Contracts.Require(firstName.Length >= 2, "The first name of an employee must at least have 2 characters");

            var employee = new Employee
            {
                Number = new EmployeeNumber(startDate, sequence),
                FirstName = firstName,
                LastName = lastName,
                StartDate = startDate,
                EndDate = null
            };
            return employee;
        }
    }
}