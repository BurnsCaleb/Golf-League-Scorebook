using Core.Interfaces.Service;
using Core.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using ViewModels;

namespace UI.Pages.InfoPages
{
    /// <summary>
    /// Interaction logic for TeamInfoPage.xaml
    /// </summary>
    public partial class TeamInfoPage : Page
    {
        public event RoutedEventHandler? Navigate;
        private readonly ITeamService _teamService;
        private readonly ITeamMatchupJunctionService _teamMatchupJunctionService;

        private Team _team;

        public TeamInfoPage(Team team)
        {
            InitializeComponent();

            _team = team;
            _teamService = App.ServiceProvider.GetRequiredService<ITeamService>();
            _teamMatchupJunctionService = App.ServiceProvider.GetRequiredService<ITeamMatchupJunctionService>();
            
            Loaded += TeamInfoPage_Loaded;
        }

        private async void TeamInfoPage_Loaded(object sender, RoutedEventArgs e)
        {
            await PopulateDataGrid();

            HeaderTextBlock.Text = _team.TeamName;
        }

        private async Task PopulateDataGrid()
        {
            // Grab all Team Matchup Junctions for this team
            var junctions = await _teamMatchupJunctionService.GetByTeam(_team.TeamId);

            if (junctions == null) return;

            List<TeamRoundViewModel> viewModels = new List<TeamRoundViewModel>();
            foreach (var junction in junctions)
            {
                TeamRoundViewModel teamRoundViewModel = new TeamRoundViewModel(junction);
                viewModels.Add(teamRoundViewModel);
            }

            TeamRoundsDataGrid.ItemsSource = viewModels;
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = await _teamService.Delete(_team.TeamId);
            if (result.IsSuccess)
            {
                Navigate?.Invoke(sender, e);
            } else
            {
                DisplayError(result.ErrorMessage);
            }
        }

        private void DisplayError(string error)
        {
            ErrorMessageTextBlock.Text = error;
            ErrorMessageTextBlock.Visibility = Visibility.Visible;
        }
    }
}
