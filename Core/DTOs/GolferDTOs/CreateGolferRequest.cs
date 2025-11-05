namespace Core.DTOs.GolferDTOs
{
    public class CreateGolferRequest
    {
        public int? GolferId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public double Handicap { get; set; }
    }
}
