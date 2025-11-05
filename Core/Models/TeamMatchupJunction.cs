

namespace Core.Models;

public class TeamMatchupJunction
{
    public int TeamMatchupId { get; set; }
    public int TeamId { get; set; }
    public int PointsAwarded { get; set; } = 0;
    public int MatchupId { get; set; }

    public virtual Matchup Matchup { get; set; }
    public virtual Team Team { get; set; }
}