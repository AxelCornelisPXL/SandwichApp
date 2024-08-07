using Kwops.Mobile.Models;

namespace Kwops.Mobile.ViewModels;

public class TeamDetailViewModel : BaseViewModel
{
    private Team _team;

    public Team Team
    {
        get => _team;
        set => SetProperty(ref _team, value);
    }

    public TeamDetailViewModel()
    {
        MessagingCenter.Subscribe<TeamsViewModel, Team>(this, "TeamSelected",
            (teamViewModel, selectedTeam) => { Team = selectedTeam; });
    }
}