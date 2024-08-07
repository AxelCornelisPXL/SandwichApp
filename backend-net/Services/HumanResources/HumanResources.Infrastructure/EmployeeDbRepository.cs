using HumanResources.AppLogic;
using HumanResources.Domain;

namespace HumanResources.Infrastructure;

internal class EmployeeDbRepository : IEmployeeRepository
{
    private readonly HumanResourcesContext _ctx;

    public EmployeeDbRepository(HumanResourcesContext ctx)
    {
        _ctx = ctx;
    }

    public async Task AddAsync(IEmployee newEmployee)
    {
        await _ctx.AddAsync(newEmployee).AsTask();
    }

    public async Task<IEmployee?> GetByNumberAsync(EmployeeNumber number)
    {
        var emp = await _ctx.FindAsync<IEmployee>(number);
        return emp;
    }

    public Task<int> GetNumberOfStartersOnAsync(DateTime startDate)
    {
        return Task.Run(() => _ctx.Set<Employee>().Count());
    }

    public async Task CommitTrackedChangesAsync()
    {
        await _ctx.SaveChangesAsync();
    }
}