
using Core.Models;
using InterfaceControls.Events;
using System.Windows.Controls;
using ViewModels;

namespace UI.Controls.InfoPills
{
    /// <summary>
    /// Interaction logic for LeagueInfoPill.xaml
    /// </summary>
    public partial class LeagueInfoPill : UserControl
    {
        public event EventHandler<LeagueEvent>? Navigate;
        private League _league;

        public LeagueInfoPill(League league)
        {
            InitializeComponent();

            _league = league;

            this.DataContext = new LeagueViewModel(league);
        }

        private void GolferButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Navigate?.Invoke(this, new LeagueEvent(_league));
        }
    }
}
