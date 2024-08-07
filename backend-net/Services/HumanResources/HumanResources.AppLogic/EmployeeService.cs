using Domain;
using HumanResources.Domain;
using IntegrationEvents.Employee;
using MassTransit;

namespace HumanResources.AppLogic;

internal class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repo;
    private readonly IEmployeeFactory _factory;
    private readonly IPublishEndpoint _bus;

    public EmployeeService(IEmployeeRepository repo, IEmployeeFactory factory, IPublishEndpoint bus)
    {
        _repo = repo;
        _factory = factory;
        _bus = bus;
    }

    public async Task<IEmployee> HireNewAsync(string lastName, string firstName, DateTime startDate)
    {
        var starters = await _repo.GetNumberOfStartersOnAsync(startDate);
        var employee = _factory.CreateNew(lastName, firstName, startDate, starters + 1);
        await _repo.AddAsync(employee);

        var @event = new EmployeeHiredIntegrationEvent { Number = employee.Number, LastName = employee.LastName, FirstName = employee.FirstName };
        await _bus.Publish(@event);

        return employee;
    }

    public async Task DismissAsync(EmployeeNumber employeeNumber, bool withNotice)
    {
        var employee = await _repo.GetByNumberAsync(employeeNumber);
        Contracts.Require(employee != null, $"Cannot find an employee with number '{employeeNumber}'");
        employee?.Dismiss(withNotice);
        await _repo.CommitTrackedChangesAsync();
    }
}