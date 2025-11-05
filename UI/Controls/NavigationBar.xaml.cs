#nullable disable

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace UI.Controls
{
    /// <summary>
    /// Interaction logic for NavigationBar.xaml
    /// </summary>
    public partial class NavigationBar : UserControl
    {
        private bool _isExpanded = false;
        private Storyboard _expandAnimation;
        private Storyboard _collapseAnimation;

        public NavigationBar()
        {
            InitializeComponent();
            _expandAnimation = (Storyboard)this.Resources["ExpandAnimation"];
            _collapseAnimation = (Storyboard)this.Resources["CollapseAnimation"];
        }

        // Events for navigation
        public event RoutedEventHandler HomeClicked;
        public event RoutedEventHandler LeaguesClicked;
        public event RoutedEventHandler PlayersClicked;
        public event RoutedEventHandler TeamsClicked;
        public event RoutedEventHandler CoursesClicked;
        public event RoutedEventHandler SettingsClicked;

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isExpanded)
            {
                CollapseNavBar();
            }
            else
            {
                ExpandNavBar();
            }
        }

        private void ExpandNavBar()
        {
            _isExpanded = true;
            _expandAnimation.Begin();

            // Show expanded content, hide icons
            HamburgerIcon.Visibility = Visibility.Collapsed;
            ExpandedToggle.Visibility = Visibility.Visible;

            HomeIcon.Visibility = Visibility.Collapsed;
            HomeExpanded.Visibility = Visibility.Visible;

            LeaguesIcon.Visibility = Visibility.Collapsed;
            LeaguesExpanded.Visibility = Visibility.Visible;

            PlayersIcon.Visibility = Visibility.Collapsed;
            PlayersExpanded.Visibility = Visibility.Visible;

            TeamsIcon.Visibility = Visibility.Collapsed;
            TeamsExpanded.Visibility = Visibility.Visible;

            CoursesIcon.Visibility = Visibility.Collapsed;
            CoursesExpanded.Visibility = Visibility.Visible;

            SettingsIcon.Visibility = Visibility.Collapsed;
            SettingsExpanded.Visibility = Visibility.Visible;

            // Clear tooltips when expanded
            HomeButton.ToolTip = null;
            LeaguesButton.ToolTip = null;
            PlayersButton.ToolTip = null;
            CoursesButton.ToolTip = null;
            SettingsButton.ToolTip = null;
            ToggleButton.ToolTip = "Collapse Navigation";
        }

        private void CollapseNavBar()
        {
            _isExpanded = false;
            _collapseAnimation.Begin();

            // Show icons, hide expanded content
            HamburgerIcon.Visibility = Visibility.Visible;
            ExpandedToggle.Visibility = Visibility.Collapsed;

            HomeIcon.Visibility = Visibility.Visible;
            HomeExpanded.Visibility = Visibility.Collapsed;

            LeaguesIcon.Visibility = Visibility.Visible;
            LeaguesExpanded.Visibility = Visibility.Collapsed;

            PlayersIcon.Visibility = Visibility.Visible;
            PlayersExpanded.Visibility = Visibility.Collapsed;

            TeamsIcon.Visibility= Visibility.Visible;
            TeamsExpanded.Visibility = Visibility.Collapsed;

            CoursesIcon.Visibility = Visibility.Visible;
            CoursesExpanded.Visibility = Visibility.Collapsed;

            SettingsIcon.Visibility = Visibility.Visible;
            SettingsExpanded.Visibility = Visibility.Collapsed;

            // Restore tooltips when collapsed
            HomeButton.ToolTip = "Home";
            LeaguesButton.ToolTip = "Leagues";
            PlayersButton.ToolTip = "Players";
            TeamsButton.ToolTip = "Teams";
            CoursesButton.ToolTip = "Courses";
            SettingsButton.ToolTip = "Settings";
            ToggleButton.ToolTip = "Toggle Navigation";
        }

        // Navigation event handlers
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            HomeClicked?.Invoke(sender, e);
        }

        private void LeaguesButton_Click(object sender, RoutedEventArgs e)
        {
            LeaguesClicked?.Invoke(sender, e);
        }

        private void PlayersButton_Click(object sender, RoutedEventArgs e)
        {
            PlayersClicked?.Invoke(sender, e);
        }

        private void CoursesButton_Click(object sender, RoutedEventArgs e)
        {
            CoursesClicked?.Invoke(sender, e);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsClicked?.Invoke(sender, e);
        }

        private void TeamsButton_Click(object sender, RoutedEventArgs e)
        {
            TeamsClicked?.Invoke(sender, e);
        }
    }
}
