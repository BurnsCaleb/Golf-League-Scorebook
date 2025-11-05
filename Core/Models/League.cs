
namespace Core.Models;

public class League
{
    public int LeagueId { get; set; }
    public string LeagueName { get; set; }
    public int LeagueSettingsId { get; set; }
    public int CourseId { get; set; }
    public DateOnly DateAccessed { get; set; }

    public virtual Course Course { get; set; }
    public virtual ICollection<GolferLeagueJunction> GolferLeagueJunctions { get; set; } = new List<GolferLeagueJunction>();
    public virtual ICollection<Season> Seasons { get; set; } = new List<Season>();
    public virtual LeagueSetting LeagueSettings { get; set; }
    public virtual ICollection<Matchup> Matchups { get; set; } = new List<Matchup>();
    public virtual ICollection<Round> Rounds { get; set; } = new List<Round>();
    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();

    public override string ToString()
    {
        return LeagueName;
    }
}