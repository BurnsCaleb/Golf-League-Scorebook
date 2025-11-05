

namespace Core.Models;

public class LeagueSetting
{
    public int LeagueSettingsId { get; set; }
    public string LeagueSettingsName { get; set; }
    public string PlayDate { get; set; }
    public int ScoringRuleId { get; set; }
    public int TeamSize { get; set; }

    public virtual ICollection<League> Leagues { get; set; } = new List<League>();
    public virtual ScoringRule ScoringRule { get; set; }

    public override string ToString()
    {
        return LeagueSettingsName;
    }
}