#nullable disable

using Core.Models;
using InterfaceControls.Events;
using System.Windows;
using System.Windows.Controls;
using ViewModels;

namespace UI.Controls.InfoPills
{
    /// <summary>
    /// Interaction logic for LeagueSettingsInfoPill.xaml
    /// </summary>
    public partial class LeagueSettingsInfoPill : UserControl
    {
        public event EventHandler<LeagueSettingEvent> Navigate;

        private LeagueSetting _setting;
        public LeagueSettingsInfoPill(LeagueSetting setting)
        {
            InitializeComponent();

            _setting = setting;

            this.DataContext = new LeagueSettingsViewModel(setting);
        }

        private void LeagueSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Navigate?.Invoke(this, new LeagueSettingEvent(_setting));
        }
    }
}
