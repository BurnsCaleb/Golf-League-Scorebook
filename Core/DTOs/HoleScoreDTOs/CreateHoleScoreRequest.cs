using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.HoleScoreDTOs
{
    public class CreateHoleScoreRequest
    {
        public HoleScore? HoleScore { get; set; }
        public required int GrossScore { get; set; }
        public required int NetScore { get; set; }
        public required int GolferId { get; set; }
        public required int RoundId { get; set; }
        public required int HoleId { get; set; }
        public required DateOnly DatePlayed { get; set; }
    }
}
