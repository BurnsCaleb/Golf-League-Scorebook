using System.Windows;
using System.Windows.Controls;

namespace UI.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public RoutedEventHandler? Leagues;
        public RoutedEventHandler? Teams;
        public RoutedEventHandler? Golfers;
        public RoutedEventHandler? Courses;

        public HomePage()
        {
            InitializeComponent();
        }

        private void LeagueButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Leagues?.Invoke(this, e);
        }

        private void TeamButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Teams?.Invoke(this, e);
        }

        private void GolferButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Golfers?.Invoke(this, e);
        }

        private void CoursesButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Courses?.Invoke(this, e);
        }
    }
}
