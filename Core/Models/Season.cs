namespace Core.Models
{
    public class Season
    {
        public int SeasonId { get; set; }
        public string SeasonName { get; set; }
        public int LeagueId { get; set; }

        public virtual League League { get; set; }
        public virtual ICollection<Matchup> Matchups { get; set; } = new List<Matchup>();

        public override string ToString()
        {
            return SeasonName;
        }
    }
}
