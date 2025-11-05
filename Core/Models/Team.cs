

namespace Core.Models;

public class Team
{
    public int TeamId { get; set; }
    public string TeamName { get; set; }
    public DateOnly LastPlayed { get; set; }
    public int LeagueId { get; set; }

    public virtual ICollection<GolferTeamJunction> GolferTeamJunctions { get; set; } = new List<GolferTeamJunction>();
    public virtual League League { get; set; }
    public virtual ICollection<TeamMatchupJunction> TeamMatchupJunctions { get; set; } = new List<TeamMatchupJunction>();
    public virtual ICollection<Round> Rounds { get; set; } = new List<Round>();
    public virtual ICollection<Substitute> Substitutes { get; set; } = new List<Substitute>();

    public override string ToString()
    {
        return TeamName;
    }
}