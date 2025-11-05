using Core.Models;

namespace ViewModels
{
    public class HoleViewModel
    {
        private Hole _hole;

        public HoleViewModel(Hole hole)
        {
            _hole = hole;
        }

        public int HoleNum => _hole.HoleNum;
        public int HolePar => _hole.Par;
        public int HoleDistance => _hole.Distance;
        public int HoleHandicap => _hole.Handicap;
    }
}
