using Core.Models;

namespace Core.DTOs.HoleScoreDTOs
{
    public class JumpToHoleResult
    {
        public required Golfer Golfer { get; set; }
        public required Hole Hole { get; set; }
    }
}
