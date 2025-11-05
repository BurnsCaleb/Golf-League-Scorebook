

namespace Core.Models;


public class Hole
{
    public int HoleId { get; set; }
    public int HoleNum { get; set; }
    public int Par { get; set; }
    public int Distance { get; set; }
    public int Handicap { get; set; }
    public int CourseId { get; set; }

    public virtual Course Course { get; set; }
    public virtual ICollection<HoleScore> HoleScores { get; set; } = new List<HoleScore>();

    public override string ToString()
    {
        return $"Hole {HoleNum}";
    }
}