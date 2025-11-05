#nullable disable

using Core.Interfaces.Service;
using Core.Models;

namespace ViewModels
{
    public class GolferViewModel
    {
        private Golfer _golfer;
        private readonly IRoundService _roundService;

        public GolferViewModel(Golfer golfer, IRoundService roundService)
        {
            _golfer = golfer;
            _roundService = roundService;
        }

        private string firstName => _golfer.FirstName;
        private string lastName => _golfer.LastName;
        public string GolferName => $"{firstName} {lastName}";

        public string GolferHandicap => _golfer.Handicap.ToString();

        private string _golferGrossAvg;
        private string _golferNetAvg;

        public string GolferGrossAvg => _golferGrossAvg;
        public string GolferNetAvg => _golferNetAvg;

        public async Task LoadAsyncTasks()
        {
            var gross = await _roundService.GetGolferAvgGrossScore(_golfer.GolferId);
            var net = await _roundService.GetGolferAvgGrossScore(_golfer.GolferId);

            _golferGrossAvg = gross.ToString();
            _golferNetAvg = net.ToString();
        }
    }
}
