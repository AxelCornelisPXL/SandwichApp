using HumanResources.Domain;

namespace HumanResources.AppLogic;

public interface IEmployeeService
{
    Task<IEmployee> HireNewAsync(string lastName, string firstName, DateTime startDate);
    Task DismissAsync(EmployeeNumber employeeNumber, bool withNotice);
}