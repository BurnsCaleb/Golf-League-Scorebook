namespace Core.DTOs.ScorerDTOs
{
    public class PointsAwarded
    {
        public int id { get; set; }  // Golfer id or Team id
        public double points { get; set; } = 0;

        public bool isGolfer { get; set; }
    }
}
