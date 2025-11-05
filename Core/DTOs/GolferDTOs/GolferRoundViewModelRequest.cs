using Core.Models;

namespace Core.DTOs.GolferDTOs
{
    public class GolferRoundViewModelRequest
    {
        public required GolferMatchupJunction GolferMatchupJunction { get; set; }
        public required Golfer Golfer { get; set; }
        public required int GrossScore { get; set; }
        public required int NetScore { get; set; }
        public required string MatchupName { get; set; }
    }
}
