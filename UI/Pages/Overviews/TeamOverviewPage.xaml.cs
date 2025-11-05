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
    /// Interaction logic for TeamOverviewPage.xaml
    /// </summary>
    public partial class TeamOverviewPage : Page
    {
        public event EventHandler<TeamEvent>? Navigate;
        public event RoutedEventHandler? NewObject;

        private readonly ITeamService _teamService;
        private readonly IGolferTeamJunctionService _golferTeamJunctionService;
        private readonly ILeagueService _leagueService;
        public TeamOverviewPage()
        {
            InitializeComponent();

            _teamService = App.ServiceProvider.GetRequiredService<ITeamService>();
            _leagueService = App.ServiceProvider.GetRequiredService<ILeagueService>();
            _golferTeamJunctionService = App.ServiceProvider.GetRequiredService<IGolferTeamJunctionService>();

            Loaded += TeamOverviewPage_Loaded;
        }

        private async void TeamOverviewPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadLeagueFilter();
        }

        private void LoadCreateNewTeamPill()
        {
            string text = "Create New Team";
            CreateNewPill newPill = new CreateNewPill(text);

            TeamPillPanel.Children.Add(newPill);

            newPill.NewObject += (sender, e) => NewObject?.Invoke(sender, e);
        }

        private async Task LoadLeagueFilter()
        {
            // Get Leagues
            var leagues = await _leagueService.GetAll();

            // Initialize List with "Show All"
            List<string> filters = new List<string>() { "Show All" };
            
            foreach (var league in leagues)
            {
                filters.Add(league.LeagueName);
            }

            LeagueFilterComboBox.ItemsSource = filters;
            LeagueFilterComboBox.SelectedIndex = 0;
        }

        private async void LeagueFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // get selection
            if ((string)LeagueFilterComboBox.SelectedItem == "Show All")
            {
                await FilterLeagues(null);
                return;
            }

            var selectedLeague = await _leagueService.GetByName((string)LeagueFilterComboBox.SelectedItem);

            await FilterLeagues(selectedLeague);
        }

        private async Task FilterLeagues(League? selectedLeague)
        {
            List<Team> teams;
            if (selectedLeague == null)
            {
                // Get all existing teams
                teams = await _teamService.GetAll();
            } else
            {
                // Get Teams in the selected league
                teams = await _teamService.GetTeamsByLeague(selectedLeague.LeagueId);
            }

            // Repopulate StackPanel
            TeamPillPanel.Children.Clear();

            foreach (Team team in teams)
            {
                TeamInfoPill teamInfoPill = new TeamInfoPill(team, _golferTeamJunctionService, _leagueService);

                teamInfoPill.Navigate += (sender, e) => Navigate?.Invoke(sender, e);

                TeamPillPanel.Children.Add(teamInfoPill);
            }

            LoadCreateNewTeamPill();

        }
    }
}
