using Core.Models;
namespace ViewModels
{
    public class LeagueViewModel
    {
        private League _league;
        public LeagueViewModel(League league)
        {
            _league = league;
        }

        public string LeagueName => _league.LeagueName ?? "League Name";
        public string CourseName => _league.Course.CourseName;
        private int _leagueTeams => _league.Teams.Count;
        public string NumTeams => $"{_leagueTeams} Teams";
    }
}
