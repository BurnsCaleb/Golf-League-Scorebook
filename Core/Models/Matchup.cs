

namespace Core.Models;
public class Matchup
{
    public int MatchupId { get; set; }
    public string MatchupName { get; set; }
    public int LeagueId { get; set; }
    public DateOnly MatchupDate { get; set; }
    public int SeasonId { get; set; }
    public bool HasPlayed { get; set; } = false;
    public int Week {  get; set; }

    public virtual League League { get; set; }
    public virtual Season Season { get; set; }
    public virtual ICollection<Round> Rounds { get; set; } = new List<Round>();
    public virtual ICollection<TeamMatchupJunction> TeamMatchupJunctions { get; set; } = new List<TeamMatchupJunction>();
    public virtual ICollection<GolferMatchupJunction> GolferMatchupJunctions { get; set; } = new List<GolferMatchupJunction>();
    public virtual ICollection<Substitute> Substitutes { get; set; } = new List<Substitute>();

    public override string ToString()
    {
        return MatchupName;
    }
}