#nullable disable
namespace ViewModels
{
    public class ScorecardTabViewModel
    {
        public string GolferName { get; set; }
        public string TopHoleNum { get; set; }
        public string TopHoleScore { get; set; } = "-";
        public string MiddleHoleNum { get; set; }
        public string MiddleHoleScore { get; set; } = "-";
        public string BottomHoleNum { get; set; }
        public string BottomHoleScore { get; set; } = "-";
    }
}
