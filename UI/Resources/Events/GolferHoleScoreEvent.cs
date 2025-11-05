using Core.DTOs.HoleDTOs;

namespace InterfaceControls.Events
{
    public class GolferHoleScoreEvent : EventArgs
    {
        public GolferHoleScore ScoreData { get; set; }

        public GolferHoleScoreEvent(GolferHoleScore scoreData)
        {
            ScoreData = scoreData;
        }
    }
}
