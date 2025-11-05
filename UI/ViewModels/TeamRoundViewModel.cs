using Core.Models;

namespace ViewModels
{
    public class TeamRoundViewModel
    {
        TeamMatchupJunction _junction { get; set; }

        public TeamRoundViewModel(TeamMatchupJunction junction)
        {
            _junction = junction;
        }

        public DateOnly DatePlayed => _junction.Matchup.MatchupDate;

        public bool HasPlayed => _junction.Matchup.HasPlayed;

        public string TeamName => _junction.Team.TeamName;

        public string MatchupName => _junction.Matchup.MatchupName;

        public int PointsAwarded => _junction.PointsAwarded;
    }
}
