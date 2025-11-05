#nullable disable

using Core.Interfaces.Service;
using Core.Models;
using InterfaceControls.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using UI.Controls;
using UI.Controls.InfoPills;

namespace UI.Pages.Overviews
{
    /// <summary>
    /// Interaction logic for LeagueSettingsOverviewPage.xaml
    /// </summary>
    public partial class LeagueSettingsOverviewPage : Page
    {
        public event EventHandler<LeagueSettingEvent> Navigate;
        public event RoutedEventHandler NewObject;

        private readonly ILeagueSettingService _leagueSettingsService;
        public LeagueSettingsOverviewPage()
        {
            InitializeComponent();

            _leagueSettingsService = App.ServiceProvider.GetRequiredService<ILeagueSettingService>();

            Loaded += LeagueSettingsOverviewPage_Loaded;
        }

        private async void LeagueSettingsOverviewPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadSettingPills(); 
        }

        private async Task LoadSettingPills()
        {
            // Grab settings
            var settings = await _leagueSettingsService.GetAll();

            if (settings.Any())
            {
                foreach (LeagueSetting setting in settings)
                {
                    LeagueSettingsInfoPill settingPill = new LeagueSettingsInfoPill(setting);

                    settingPill.Navigate += (sender, e) => Navigate?.Invoke(sender, e);

                    SettingsPillStackPanel.Children.Add(settingPill);
                }
            }

            string text = "Create New Settings";
            CreateNewPill newSettingPill = new CreateNewPill(text);

            newSettingPill.NewObject += (sender, e) => NewObject?.Invoke(sender, e);

            SettingsPillStackPanel.Children.Add(newSettingPill);
        }
    }
}
