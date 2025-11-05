using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Core.DTOs.HoleDTOs
{
    public class GolferHoleScore : INotifyPropertyChanged
    {
        private int _golferId;
        public int GolferId
        {
            get => _golferId;
            set
            {
                _golferId = value;
                OnPropertyChanged();
            }
        }

        private string _golferName = string.Empty;
        public string GolferName
        {
            get => _golferName;
            set
            {
                _golferName = value;
                OnPropertyChanged();
            }
        }

        public int TeamId { get; set; }

        private int _holeId;
        public int HoleId
        {
            get => _holeId;
            set
            {
                _holeId = value;
                OnPropertyChanged();
            }
        }

        public int HoleNumber { get; set; }

        public int HoleDistance { get; set; }

        public int HoleHandicap { get; set; }

        public int HolePar { get; set; }

        private int _grossScore;
        public int GrossScore
        {
            get => _grossScore;
            set
            {
                _grossScore = value;
                OnPropertyChanged();
            }
        }

        public int NetScore { get; set; }
        public double GolferHandicap {  get; set; }

        public string GrossScoreDisplay
        {
            get
            {
                if (_grossScore == 0)
                    return "-";
                return _grossScore.ToString();
            }
        }

        public string HoleScoreDisplay
        {
            get
            {
                if (_grossScore == 0) return string.Empty;
                return _grossScore.ToString();
            }
        }

        public string HoleNameDisplay => $"Hole {HoleNumber}";

        public string HoleDistanceDisplay => $"{HoleDistance} yds";

        public string HoleParDisplay => $"Par {HolePar}";

        public string HoleHandicapDisplay => $"{HoleHandicap} Handicap";


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
