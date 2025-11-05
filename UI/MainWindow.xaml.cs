using InterfaceControls.Events;
using System.Windows;
using UI.Pages;
using UI.Pages.Forms;
using UI.Pages.InfoPages;
using UI.Pages.Overviews;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            NavigationBar_HomeClicked(null, null);
        }

        #region Home
        private void NavigationBar_HomeClicked(object? sender, RoutedEventArgs? e)
        {
            HomePage homePage = new HomePage();
            MainContentFrame.Navigate(homePage);

            homePage.Leagues += NavigationBar_LeaguesClicked;
            homePage.Teams += NavigationBar_TeamsClicked;
            homePage.Golfers += NavigationBar_PlayersClicked;
            homePage.Courses += NavigationBar_CoursesClicked;
        }
        #endregion

        private void GoTo_EnterScores(object sender, MatchupEvent e, LeagueInfoPage parentPage)
        {
            EnterScoresPage enterScores = new EnterScoresPage(e.Matchup, parentPage);
            MainContentFrame.Navigate(enterScores);
        }

        #region Leagues
        private void NavigationBar_LeaguesClicked(object sender, RoutedEventArgs e)
        {
            LeagueOverviewPage leagueOverviewPage = new LeagueOverviewPage();
            MainContentFrame.Navigate(leagueOverviewPage);

            leagueOverviewPage.Navigate += (sender, e) => GoTo_LeaguePage(sender, e);
            leagueOverviewPage.NewObject += (sender, e) => GoTo_LeaguePage(sender, e);

            
        }

        private void GoTo_LeaguePage(object sender, RoutedEventArgs e)
        {
            CreateLeaguePage createLeaguePage = new CreateLeaguePage();
            MainContentFrame.Navigate(createLeaguePage);

            createLeaguePage.Navigate += (sender, e) => NavigationBar_LeaguesClicked(sender, e);
        }

        private void GoTo_LeaguePage(object sender, LeagueEvent leagueEvent)
        {
            LeagueInfoPage leagueInfoPage = new LeagueInfoPage(leagueEvent.League);
            MainContentFrame.Navigate(leagueInfoPage);

            leagueInfoPage.Navigate += (sender, e) => NavigationBar_LeaguesClicked(sender, e);
            leagueInfoPage.Matchup += (sender, e) => GoTo_EnterScores(sender, e, leagueInfoPage);
            leagueInfoPage.Return += () => GoTo_LeaguePage(sender, leagueEvent);
        }
        #endregion

        #region Golfers
        private void NavigationBar_PlayersClicked(object sender, RoutedEventArgs e)
        {
            GolferOverviewPage golferOverviewPage = new GolferOverviewPage();
            MainContentFrame.Navigate(golferOverviewPage);

            golferOverviewPage.NewObject += (sender, e) => GoTo_CreateGolferPage(sender, e);
            golferOverviewPage.Navigate += (sender, e) => GoTo_GolferInfoPage(sender, e);
        }
        
        private void GoTo_CreateGolferPage(object sender, RoutedEventArgs e)
        {
            CreateGolferPage createGolferPage = new CreateGolferPage();
            MainContentFrame.Navigate(createGolferPage);

            createGolferPage.SaveGolferEvent += NavigationBar_PlayersClicked;
        }

        private void GoTo_GolferInfoPage(object sender, GolferEvent e)
        {
            GolferInfoPage golferInfoPage = new GolferInfoPage(e.Golfer);

            MainContentFrame.Navigate(golferInfoPage);

            golferInfoPage.Navigate += (sender, e) => NavigationBar_PlayersClicked(sender, e);
        }
        #endregion

        #region Teams

        private void NavigationBar_TeamsClicked(object sender, RoutedEventArgs e)
        {
            TeamOverviewPage teamOverviewPage = new TeamOverviewPage();
            MainContentFrame.Navigate(teamOverviewPage);

            teamOverviewPage.Navigate += (sender, e) => GoTo_TeamInfoPage(sender, e);
            teamOverviewPage.NewObject += (sender, e) => GoTo_CreateNewTeamPage(sender, e);
        }

        private void GoTo_CreateNewTeamPage(object sender, RoutedEventArgs e)
        {
            CreateTeamPage createTeamPage = new CreateTeamPage();
            MainContentFrame.Navigate(createTeamPage);

            createTeamPage.Navigate += (sender, e) => NavigationBar_TeamsClicked(sender, e);
        }

        private void GoTo_TeamInfoPage(object sender, TeamEvent e)
        {
            TeamInfoPage teamInfoPage = new TeamInfoPage(e.Team);
            MainContentFrame.Navigate(teamInfoPage);

            teamInfoPage.Navigate += (sender, e) => NavigationBar_TeamsClicked(sender, e);
        }

        #endregion

        #region Courses
        private void NavigationBar_CoursesClicked(object sender, RoutedEventArgs e)
        {
            CoursesOverviewPage coursesOverviewPage = new CoursesOverviewPage();
            MainContentFrame.Navigate(coursesOverviewPage);

            coursesOverviewPage.NewObject += GoTo_CreateCoursePage;
            coursesOverviewPage.Navigate += (sender, e) => GoTo_CreateCoursePage(sender, e);
        }

        private void GoTo_CreateCoursePage(object sender, RoutedEventArgs e)
        {
            CreateCoursePage coursePage = new CreateCoursePage();
            MainContentFrame.Navigate(coursePage);

            coursePage.SaveCourseEvent += (sender, e) => NavigationBar_HomeClicked(sender, e);
        }

        private void GoTo_CreateCoursePage(object sender, EditCourseEvent e)
        {
            CreateCoursePage coursePage = new CreateCoursePage(e.Course);
            MainContentFrame.Navigate(coursePage);

            coursePage.SaveCourseEvent += (sender, e) => NavigationBar_HomeClicked(sender, e);
        }
        #endregion

        #region Settings
        private void NavigationBar_SettingsClicked(object sender, RoutedEventArgs e)
        {
            LeagueSettingsOverviewPage leagueSettingsOverviewPage = new LeagueSettingsOverviewPage();

            MainContentFrame.Navigate(leagueSettingsOverviewPage);

            leagueSettingsOverviewPage.Navigate += (sender, e) => GoTo_CreateLeagueSettingPage_Edit(sender, e);
            leagueSettingsOverviewPage.NewObject += (sender, e) => GoTo_CreateLeagueSettingPage(sender, e);
        }

        private void GoTo_CreateLeagueSettingPage(object sender, RoutedEventArgs e)
        {
            CreateNewLeagueSettingsPage createNewLeagueSettingsPage = new CreateNewLeagueSettingsPage();

            MainContentFrame.Navigate(createNewLeagueSettingsPage);

            createNewLeagueSettingsPage.Navigate += (sender, e) => NavigationBar_HomeClicked(sender, e);
        }

        private void GoTo_CreateLeagueSettingPage_Edit(object sender, LeagueSettingEvent e)
        {
            CreateNewLeagueSettingsPage createNewLeagueSettingsPage = new CreateNewLeagueSettingsPage(e.LeagueSetting);

            MainContentFrame.Navigate(createNewLeagueSettingsPage);

            createNewLeagueSettingsPage.Navigate += (sender, e) => NavigationBar_HomeClicked(sender, e);
        }
        #endregion

    }
}