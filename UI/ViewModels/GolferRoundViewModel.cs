using Core.DTOs.GolferDTOs;
using Core.Models;

namespace ViewModels
{
    public class GolferRoundViewModel
    {
        private Golfer Golfer { get; set; }
        private GolferMatchupJunction Junction { get; set; }
        private GolferRoundViewModelRequest Request { get; set; }

        public GolferRoundViewModel(GolferRoundViewModelRequest request)
        {
            Junction = request.GolferMatchupJunction;
            Golfer = request.Golfer;
            Request = request;
        }

        public string GolferName => Golfer.FullName;
        public int GrossScore => Request.GrossScore;
        public int NetScore => Request.NetScore;
        public string MatchupName => Request.MatchupName;
        public int PointsAwarded => Junction.PointsAwarded;
    }
}
