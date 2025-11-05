using Core.Models;

namespace ViewModels
{
    
    public class RoundViewModel
    {
        private Round _round;

        public RoundViewModel(Round round)
        {
            _round = round;
        }

        public DateOnly DatePlayed => _round.DatePlayed;
        public string League => _round.League.LeagueName;
        public string Matchup => _round.Matchup.MatchupName;
        public int GrossScore => _round.GrossTotal;
        public int NetScore => _round.NetTotal;
    }
}
