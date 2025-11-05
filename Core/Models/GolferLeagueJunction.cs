namespace Core.Models;

public class GolferLeagueJunction
{
    public int GolferId { get; set; }
    public int LeagueId { get; set; }

    public virtual Golfer Golfer { get; set; }
    public virtual League League { get; set; }
}