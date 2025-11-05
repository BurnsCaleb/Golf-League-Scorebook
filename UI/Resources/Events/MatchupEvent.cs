using Core.Models;

namespace InterfaceControls.Events
{
    public class MatchupEvent : EventArgs
    {
        public Matchup Matchup { get; set; }

        public MatchupEvent(Matchup matchup)
        {
            Matchup = matchup;
        }
    }
}
