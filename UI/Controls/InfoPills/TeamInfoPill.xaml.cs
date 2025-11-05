using Core.Interfaces.Service;
using Core.Models;
using InterfaceControls.Events;
using System.Windows;
using System.Windows.Controls;
using ViewModels;

namespace UI.Controls.InfoPills
{
    /// <summary>
    /// Interaction logic for TeamInfoPill.xaml
    /// </summary>
    public partial class TeamInfoPill : UserControl
    {
        public event EventHandler<TeamEvent>? Navigate;
        private Team _team;

        private readonly IGolferTeamJunctionService _golferTeamJunctionService;
        private readonly ILeagueService _leagueService;

        public TeamInfoPill(Team team, IGolferTeamJunctionService golferTeamJunctionService, ILeagueService leagueService)
        {
            InitializeComponent();

            _team = team;
            _golferTeamJunctionService = golferTeamJunctionService;
            _leagueService = leagueService;

            this.DataContext = new TeamViewModel(team, _golferTeamJunctionService, _leagueService);
        }

        private void TeamButton_Click(object sender, RoutedEventArgs e)
        {
            Navigate?.Invoke(this, new TeamEvent(_team));
        }
    }
}
