namespace DevOps.AppLogic;

using Domain;

public interface ITeamRepository
{
    Task<IReadOnlyList<Team>> GetAllAsync();
    Task<Team?> GetByIdAsync(Guid teamId);
}