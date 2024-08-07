using DevOps.AppLogic;
using DevOps.Domain;

namespace DevOps.Infrastructure;

public class TeamRepository : ITeamRepository
{
    private readonly DevOpsContext _ctx;

    public TeamRepository(DevOpsContext ctx)
    {
        _ctx = ctx;
    }

    public Task<IReadOnlyList<Team>> GetAllAsync()
    {
        return Task.FromResult<IReadOnlyList<Team>>(_ctx.Teams.ToList().AsReadOnly());
    }

    public async Task<Team?> GetByIdAsync(Guid teamId)
    {
        var ret = await _ctx.Teams.FindAsync();
        return ret;
    }
}