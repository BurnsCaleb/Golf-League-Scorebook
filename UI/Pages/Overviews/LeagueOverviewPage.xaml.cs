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
    /// Interaction logic for LeagueOverviewPage.xaml
    /// </summary>
    public partial class LeagueOverviewPage : Page
    {
        public event EventHandler<LeagueEvent>? Navigate;
        public event RoutedEventHandler? NewObject;

        private readonly ILeagueService _leagueService;
        public LeagueOverviewPage()
        {
            InitializeComponent();

            _leagueService = App.ServiceProvider.GetRequiredService<ILeagueService>();

            Loaded += LeagueOverviewPage_Loaded;
        }

        private async void LeagueOverviewPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadLeaguePills();
        }

        private async Task LoadLeaguePills()
        {
            var leagues = await _leagueService.GetAll();

            if (leagues.Any())
            {
                foreach (League league in leagues)
                {

                    LeagueInfoPill leagueInfo = new LeagueInfoPill(league);
                    LeaguesPillStackPanel.Children.Add(leagueInfo);

                    leagueInfo.Navigate += (sender, e) => Navigate?.Invoke(sender, e);
                }
            }

            string text = "Create New League";
            CreateNewPill createNewPill = new CreateNewPill(text);
            LeaguesPillStackPanel.Children.Add(createNewPill);

            createNewPill.NewObject += (sender, e) => NewObject?.Invoke(sender, e);
        }
    }
}
