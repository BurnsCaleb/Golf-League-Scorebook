

namespace Core.Models;
public class ScoringRule
{
    public int ScoringRuleId { get; set; }
    public string RuleName { get; set; }
    public string Description { get; set; }

    public virtual ICollection<LeagueSetting> LeagueSettings { get; set; } = new List<LeagueSetting>();

    public override string ToString()
    {
        return RuleName;
    }

}
