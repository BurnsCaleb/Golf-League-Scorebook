using Core.Models;
using System.Windows.Controls;
using System.Windows.Media;

namespace UI.Controls
{
    /// <summary>
    /// Interaction logic for LeagueOption.xaml
    /// </summary>
    public partial class LeagueOption : UserControl
    {
        public bool IsSelected { get; set; }
        public LeagueOption(League league)
        {
            InitializeComponent();

            this.DataContext = league;
        }

        private void LeagueOptionButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            IsSelected = !IsSelected;

            HighlightButton();
        }

        private void HighlightButton()
        {
            if (IsSelected)
            {
                LeagueOptionButton.Background = (Brush)this.FindResource("Accent");
            } else
            {
                LeagueOptionButton.Background = (Brush)this.FindResource("Background");
            }
        }
    }
}
