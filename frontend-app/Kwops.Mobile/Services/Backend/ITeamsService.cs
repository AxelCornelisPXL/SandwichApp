using Kwops.Mobile.Models;

namespace Kwops.Mobile.Services.Backend;

public interface ITeamsService
{
    Task<IReadOnlyList<Team>> GetAllTeamsAsync();
}