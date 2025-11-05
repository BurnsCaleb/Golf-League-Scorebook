namespace Core.Models
{
    public class Golfer
    {
        public int GolferId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Handicap { get; set; }
        public DateOnly DateLastPlayed { get; set; }
        public string FullName { get; set; }

        public virtual ICollection<GolferLeagueJunction> GolferLeagueJunctions { get; set; } = new List<GolferLeagueJunction>();
        public virtual ICollection<Round> Rounds { get; set; } = new List<Round>();
        public virtual ICollection<GolferTeamJunction> GolferTeamJunctions { get; set; } = new List<GolferTeamJunction>();
        public virtual ICollection<GolferMatchupJunction> GolferMatchupJunctions { get; set; } = new List<GolferMatchupJunction>();
        public virtual ICollection<HoleScore> HoleScores { get; set; } = new List<HoleScore>();

        public override string ToString()
        {
            return FullName;
        }
    }
}
