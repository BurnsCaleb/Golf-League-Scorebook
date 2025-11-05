using Core.Models;

namespace InterfaceControls.Events
{
    public class LeagueEvent : EventArgs
    {
        public League League { get; set; }

        public LeagueEvent(League league)
        {
            this.League = league;
        }
    }
}
