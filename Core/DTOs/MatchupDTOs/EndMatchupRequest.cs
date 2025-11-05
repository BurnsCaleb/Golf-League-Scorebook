using Core.DTOs.HoleDTOs;
using Core.Models;

namespace Core.DTOs.MatchupDTOs
{
    public class EndMatchupRequest
    {
        public required Matchup Matchup { get; set; }
        public required List<GolferHoleScore> ScoreData { get; set; }
        public required List<Golfer> Golfers { get; set; }
        public required List<Round> Rounds { get; set; }
    }
}
