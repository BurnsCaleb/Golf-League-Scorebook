using Core.Models;

namespace ViewModels
{
    public class LeagueSettingsViewModel
    {
        private LeagueSetting _setting;

        public LeagueSettingsViewModel(LeagueSetting leagueSetting) 
        { 
            _setting = leagueSetting;
            
        }

        public string SettingName => _setting.LeagueSettingsName;

        public DateOnly PlayDate => DateOnly.FromDateTime(Convert.ToDateTime(_setting.PlayDate));

        private int scoringRuleId => _setting.ScoringRuleId;
        public string ScoringRule => scoringRuleId switch
        {
            1 => "Team Handicap",
            2 => "Stroke Play",
            3 => "Stroke Play Handicap",
            4 => "Match Play",
            5 => "Match Play Handicap",
            _ => "Stroke Play"
        };
    }
}
