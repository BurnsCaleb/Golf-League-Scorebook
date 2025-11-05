namespace Core.DTOs.HoleDTOs
{
    public class CreateHoleRequest
    {
        public int? HoleId { get; set; }
        public int HoleNum { get; set; }
        public int Par { get; set; }
        public int Distance { get; set; }
        public int Handicap { get; set; }

        public bool KnownWarning { get; set; } = false;
    }
}
