using System.ComponentModel;
using DevOps.Domain;

namespace DevOps.AppLogic;

internal class TeamService : ITeamService
{
    private readonly IDeveloperRepository _repo;

    public TeamService(IDeveloperRepository developerRepository)
    {
        _repo = developerRepository;
    }

    public async Task AssembleDevelopersAsyncFor(Team team, int requiredNumberOfDevelopers)
    {
        var list = await _repo.FindDevelopersWithoutATeamAsync();
        var i = 0;
        foreach (var d in list)
            if (i++ < requiredNumberOfDevelopers)
                team.Join(d);
        await _repo.CommitTrackedChangesAsync();
    }
}