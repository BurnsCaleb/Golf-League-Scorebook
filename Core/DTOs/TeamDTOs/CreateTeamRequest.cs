using Core.Models;

namespace Core.DTOs.TeamDTOs
{
    public class CreateTeamRequest
    {
        public int? TeamId { get; set; }
        public required string TeamName { get; set; }
        public int LeagueId { get; set; }
        public required List<Golfer> Golfers { get; set; }
    }
}
