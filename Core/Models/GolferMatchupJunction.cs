namespace Core.Models;

public class GolferMatchupJunction
{
    public int PointsAwarded { get; set; } = 0;
    public int GolferId { get; set; }
    public int MatchupId { get; set; }

    public virtual Matchup Matchup { get; set; }
    public virtual Golfer Golfer { get; set; }
}