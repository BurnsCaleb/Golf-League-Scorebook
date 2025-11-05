

namespace Core.Models;

public class Round
{
    public int RoundId { get; set; }
    public string Name { get; set; }
    public int NetTotal { get; set; }
    public int GrossTotal { get; set; }
    public DateOnly DatePlayed { get; set; }
    public int GolferId { get; set; }
    public int TeamId { get; set; }
    public bool ActiveSubstitute { get; set; } = false;
    public int CourseId { get; set; }
    public int LeagueId { get; set; }
    public int MatchupId { get; set; }
    public double ScoreDifferential { get; set; }

    public virtual Team Team { get; set; }
    public virtual Course Course { get; set; }
    public virtual League League { get; set; }
    public virtual Matchup Matchup { get; set; }
    public virtual Golfer Golfer { get; set; }
    public virtual ICollection<HoleScore> HoleScores { get; set; } = new List<HoleScore>();

    public override string ToString()
    {
        return Name;
    }
}