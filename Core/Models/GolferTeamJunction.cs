
namespace Core.Models;

public class GolferTeamJunction
{ 
    public int GolferId { get; set; }
    public int TeamId { get; set; }

    public virtual Golfer Golfer { get; set; }
    public virtual Team Team { get; set; }
}