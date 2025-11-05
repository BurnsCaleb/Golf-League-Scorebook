namespace Core.Models
{
    public class Substitute
    {
        public int GolferId { get; set; }
        public int TeamId { get; set; }
        public int MatchupId { get; set; }
        public int OriginalGolferId { get; set; }

        public virtual Golfer Golfer { get; set; }
        public virtual Golfer OriginalGolfer { get; set; }
        public virtual Team Team { get; set; }
        public virtual Matchup Matchup { get; set; }
    }
}
