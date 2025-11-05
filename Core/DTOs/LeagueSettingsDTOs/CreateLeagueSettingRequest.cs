namespace Core.DTOs.LeagueSettingsDTOs
{
    public class CreateLeagueSettingRequest
    {
        public int? LeagueSettingsId { get; set; }
        public required string LeagueSettingsName { get; set; }
        public required string PlayDate { get; set; }
        public int ScoringRuleId { get; set; }
        public int TeamSize { get; set; }
    }
}
