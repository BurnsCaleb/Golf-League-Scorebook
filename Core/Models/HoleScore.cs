

namespace Core.Models;

public class HoleScore
{
    public int GrossScore { get; set; }
    public int NetScore { get; set; }
    public DateOnly DatePlayed { get; set; }
    public int GolferId { get; set; }
    public int RoundId { get; set; }
    public int HoleId { get; set; }

    public virtual Golfer Golfer { get; set; }
    public virtual Hole Hole { get; set; }
    public virtual Round Round { get; set; }
}