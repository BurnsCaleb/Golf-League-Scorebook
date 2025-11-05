using Core.Interfaces.Service;
using Core.Models;

namespace ViewModels
{
    public class TeamViewModel
    {
        private Team _team { get; set; }
        private readonly IGolferTeamJunctionService _golferTeamJunctionService;
        private readonly ILeagueService _leagueService;

        public TeamViewModel(Team team, IGolferTeamJunctionService golferTeamJunctionService, ILeagueService leagueService)
        {
            _team = team;
            _golferTeamJunctionService = golferTeamJunctionService;
            _leagueService = leagueService;

            TeamViewModel_Loaded();
        }

        private async void TeamViewModel_Loaded()
        {
            GolferNamesDisplay = await GetGolferNames();
            LeagueName = await GetLeagueName();
        }

        public string GolferNamesDisplay {  get; set; } = string.Empty;
        public string LeagueName {  get; set; } = string.Empty;

        private async Task<string> GetGolferNames()
        {
            // Grabs all golfers on a team and then their names and then joins them together using &.
            var golfers = await _golferTeamJunctionService.GetAllGolfersByTeam(_team.TeamId);
            return string.Join(" & ", golfers.Select(g => g.FullName));
        }

        private async Task<string> GetLeagueName()
        {
            var league = await _leagueService.GetById(_team.LeagueId);
            return league.LeagueName;
        }
    }
}
