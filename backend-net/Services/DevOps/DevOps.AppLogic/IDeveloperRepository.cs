namespace DevOps.AppLogic;

using Domain;

public interface IDeveloperRepository
{
    Task<IReadOnlyList<Developer>> FindDevelopersWithoutATeamAsync();
    Task CommitTrackedChangesAsync();
    Task<Developer?> GetByIdAsync(string eventNumber);
    Task AddAsync(Developer developer);
}