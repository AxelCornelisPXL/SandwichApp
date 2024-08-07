using DevOps.AppLogic;
using DevOps.Domain;

namespace DevOps.Infrastructure;

public class DeveloperRepository : IDeveloperRepository
{
    private DevOpsContext _ctx;

    public DeveloperRepository(DevOpsContext ctx)
    {
        _ctx = ctx;
    }

    public Task<IReadOnlyList<Developer>> FindDevelopersWithoutATeamAsync()
    {
        return Task.FromResult<IReadOnlyList<Developer>>(_ctx.Developers.ToList().AsReadOnly());
    }

    public async Task CommitTrackedChangesAsync()
    {
        await _ctx.SaveChangesAsync();
    }

    public Task<Developer?> GetByIdAsync(string eventNumber)
    {
        return Task.FromResult(_ctx.Developers?.Find(eventNumber));
    }

    public async Task AddAsync(Developer developer)
    {
        await _ctx.AddAsync(developer);
    }
}