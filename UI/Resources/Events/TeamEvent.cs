using Core.Models;

namespace InterfaceControls.Events
{
    public class TeamEvent : EventArgs
    {
        public Team Team { get; set; }

        public TeamEvent(Team team)
        {
            Team = team;
        }
    }
}
