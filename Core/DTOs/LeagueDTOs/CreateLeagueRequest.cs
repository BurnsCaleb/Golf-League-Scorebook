namespace Core.DTOs.LeagueDTOs
{
    public class CreateLeagueRequest
    {
        public int? LeagueId { get; set; }
        public required string LeagueName { get; set; }
        public int LeagueSettingsId { get; set; }
        public int CourseId { get; set; }
    }
}
