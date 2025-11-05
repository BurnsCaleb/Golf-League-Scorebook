using Core.Models;

namespace InterfaceControls.Events
{
    public class LeagueSettingEvent : EventArgs
    {
        public LeagueSetting LeagueSetting { get; set; }

        public LeagueSettingEvent(LeagueSetting leagueSetting)
        {
            LeagueSetting = leagueSetting;
        }
    }
}
