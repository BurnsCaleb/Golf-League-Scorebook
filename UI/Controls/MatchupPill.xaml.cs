using Core.Models;
using InterfaceControls.Events;
using System.Windows.Controls;
using System.Windows.Media;

namespace UI.Controls
{
    /// <summary>
    /// Interaction logic for MatchupPill.xaml
    /// </summary>
    public partial class MatchupPill : UserControl
    {
        private Matchup matchup;

        public event EventHandler<MatchupEvent>? EditMatchup;
        public MatchupPill(Matchup matchup)
        {
            InitializeComponent();

            this.matchup = matchup;

            this.DataContext = matchup;

            // Change background color if matchup has been played
            if (matchup.HasPlayed)
            {
                MatchupPillButton.Background = (Brush)this.FindResource("Dark Gray");
            }
        }

        private void MatchupPillButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            EditMatchup?.Invoke(this, new MatchupEvent(matchup));
        }
    }
}
