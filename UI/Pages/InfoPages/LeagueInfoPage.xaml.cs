using Core.DTOs.GolferDTOs;
using Core.Interfaces.Service;
using Core.Models;
using Data;
using InterfaceControls.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using UI.Controls;
using UI.Windows;
using ViewModels;

namespace UI.Pages.InfoPages
{
    /// <summary>
    /// Interaction logic for LeagueInfoPage.xaml
    /// </summary>
    public partial class LeagueInfoPage : Page
    {
        private League _league;
        private readonly ILeagueService _leagueService;
        private readonly IMatchupService _matchupService;
        private readonly ITeamService _teamService;
        private readonly ITeamMatchupJunctionService _teamMatchupJunctionService;
        private readonly IGolferService _golferService;
        private readonly IGolferMatchupJunctionService _golferMatchupJunctionService;
        private readonly IGolferTeamJunctionService _golferTeamJunctionService;
        private readonly ISeasonService _seasonService;
        private readonly ISubstituteService _substituteService;

        private int currentMatchupIndex = 0;
        private List<Matchup>? matchups;

        public event RoutedEventHandler? Navigate;
        public event EventHandler<MatchupEvent>? Matchup;
        public event Action? Return;
        public LeagueInfoPage(League league)
        {
            InitializeComponent();

            _league = league;
            _leagueService = App.ServiceProvider.GetRequiredService<ILeagueService>();
            _matchupService = App.ServiceProvider.GetRequiredService<IMatchupService>();
            _teamService = App.ServiceProvider.GetRequiredService<ITeamService>();
            _teamMatchupJunctionService = App.ServiceProvider.GetRequiredService<ITeamMatchupJunctionService>();
            _golferService = App.ServiceProvider.GetRequiredService<IGolferService>();
            _golferMatchupJunctionService = App.ServiceProvider.GetRequiredService<IGolferMatchupJunctionService>();
            _golferTeamJunctionService = App.ServiceProvider.GetRequiredService<IGolferTeamJunctionService>();
            _seasonService = App.ServiceProvider.GetRequiredService<ISeasonService>();
            _substituteService = App.ServiceProvider.GetRequiredService<ISubstituteService>();

            // Set League Name
            HeaderTextBlock.Text = _league.LeagueName;

            // Set Year and Week ComboBox
            Loaded += LeagueInfoPage_Loaded;

            // PopulateUI is called with PopulateSeasonFilter()
        }

        private async void LeagueInfoPage_Loaded(object sender, RoutedEventArgs e)
        {
            await PopulateSeasonFilter();
        }

        private void ShowDataTables()
        {
            DataGridsStackPanel.Visibility = Visibility.Visible;
            MatchupPillsScroller.Visibility = Visibility.Visible;

            NextWeekButton.Visibility = Visibility.Collapsed;
            EnterScoresButton.Visibility = Visibility.Collapsed;
            NewSeasonButton.Visibility = Visibility.Collapsed;

        }

        private void ShowEnterScoresButton(int week)
        {
            DataGridsStackPanel.Visibility = Visibility.Collapsed;
            NextWeekButton.Visibility = Visibility.Collapsed;
            NewSeasonButton.Visibility = Visibility.Collapsed;

            MatchupPillsScroller.Visibility = Visibility.Visible;
            EnterScoresText.Text = $"Enter Scores for Week {week + 1}";
            EnterScoresButton.Visibility = Visibility.Visible;
        }

        private void ShowCreateSeasonButton()
        {
            DataGridsStackPanel.Visibility = Visibility.Collapsed;
            MatchupPillsScroller.Visibility = Visibility.Collapsed;
            NextWeekButton.Visibility = Visibility.Collapsed;
            EnterScoresButton.Visibility = Visibility.Collapsed;

            NewSeasonButton.Visibility = Visibility.Visible;
        }

        private async Task PopulateSeasonFilter()
        {
            // Group Matchups By Season
            var groupedMatchup = await _matchupService.GetYearly(_league.LeagueId);

            var seasons = new List<Season>();

            // Grab Seasons from groups of matchups
            foreach (var matchup in groupedMatchup)
            {
                seasons.Add(matchup.First().Season);
            }

            SeasonComboBox.DisplayMemberPath = "SeasonName";
            SeasonComboBox.SelectedValuePath = "SeasonId";

            // Set ItemList
            SeasonComboBox.ItemsSource = seasons.OrderByDescending(y => y.SeasonId);
            SeasonComboBox.SelectedIndex = 0;

            if (seasons.Count() == 0)
            {
                ShowCreateSeasonButton();
            }
        }

        private async Task PopulateWeekFilter(Season season)
        {
            // Get matchups by season, grouped by week.
            var groupedMatchups = await _matchupService.GetWeekly(_league.LeagueId, season.SeasonId);

            // Get count of weeks
            int weeks = groupedMatchups.Count();

            // Create data list for ItemList
            List<string> weeksDisplay = new List<string>();
            for (int i = 1; i <= weeks; i++)
            {
                weeksDisplay.Add($"Week {i}");
            }

            if (weeks == 0)
            {
                weeksDisplay.Add("Week 1");

                WeekComboBox.ItemsSource = weeksDisplay;
                WeekComboBox.SelectedIndex = 0;

                return;
            }

            // Set Item List
            WeekComboBox.ItemsSource = weeksDisplay;

            // Get Most recent week with a played matchup
            int recentWeek = await _matchupService.GetLatestMatchupWeek(_league.LeagueId, season.SeasonId);

            if (recentWeek == 0) recentWeek = 1;
            WeekComboBox.SelectedIndex = recentWeek - 1;

        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = await _leagueService.Delete(_league.LeagueId);
            if (result.IsSuccess)
            {
                Navigate?.Invoke(sender, e);
            }
            else
            {
                DisplayError(result.ErrorMessage);
            }
        }

        private void EndSeasonButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }


        #region Populate UI with Data

        private async Task PopulateUI(Season season, int week)
        {
            if (season == null)
            {
                ShowCreateSeasonButton();
            }
            else
            {
                await PopulateTeamRoundsTable(season, week);
                await PopulateGolferRoundsTable(season, week);
                await PopulateLeagueLeaderboard(season);
                await PopulateMatchupPills(season, week);
                await CheckNextWeek(season, week);
            }
        }

        private async Task PopulateTeamRoundsTable(Season season, int week)
        {
            TeamRoundsDataGrid.ItemsSource = null;

            // Get List of MatchupIds for this week
            var groupedMatchups = await _matchupService.GetByWeek(_league.LeagueId, season.SeasonId, week + 1);

            if (groupedMatchups.Count() == 0)
            {
                return;
            }

            // Grab MatchupIds
            var matchupIds = groupedMatchups.Select(m => m.MatchupId).ToList();

            // Get TeamIds
            var teams = await _teamService.GetTeamsByLeague(_league.LeagueId);
            var teamIds = teams.Select(t => t.TeamId).ToList();

            // Get all TeamMatchupJunctions that belong to this league
            var teamMatchupJunctions = await _teamMatchupJunctionService.GetByLeague(teamIds, matchupIds);

            // Populate Team Rounds DataGrid
            List<TeamRoundViewModel> teamRoundViewModels = new List<TeamRoundViewModel>();

            foreach (var teamMatchupJunction in teamMatchupJunctions)
            {
                teamRoundViewModels.Add(new TeamRoundViewModel(teamMatchupJunction));
            }

            var rounds = teamRoundViewModels.OrderByDescending(t => t.PointsAwarded).ToList();

            TeamRoundsDataGrid.ItemsSource = rounds;


            ShowDataTables();

            // Show enter scores if matchups do not have dates
            if (teamRoundViewModels.Any(t => t.HasPlayed == false))
            {
                ShowEnterScoresButton(week);
            }

        }

        private async Task PopulateGolferRoundsTable(Season season, int week)
        {
            GolferRoundsDataGrid.ItemsSource = null;

            // Get List of MatchupIds for this week
            var groupedMatchups = await _matchupService.GetByWeek(_league.LeagueId, season.SeasonId, week + 1);

            if (groupedMatchups.Count() == 0) return;

            // Grab MatchupIds
            var matchupIds = groupedMatchups.Select(m => m.MatchupId).ToList();

            // Get All GolfersIds that belong to this league
            var golfers = await _golferService.GetByLeague(_league.LeagueId);
            var golferIds = golfers.Select(g => g.GolferId).ToList();

            // Get All Golfer Matchup Junctions for the Week
            var golferJunctionsByWeek = await _golferMatchupJunctionService.GetById(golferIds, matchupIds);

            // Populate Golfer Rounds
            List<GolferRoundViewModel> golferRoundViewModels = new List<GolferRoundViewModel>();

            foreach (var golferJunction in golferJunctionsByWeek)
            {
                GolferRoundViewModelRequest request = await _golferService.GetViewModelData(golferJunction);
                golferRoundViewModels.Add(new GolferRoundViewModel(request));
            }

            var golferRounds = golferRoundViewModels.OrderByDescending(r => r.PointsAwarded);

            GolferRoundsDataGrid.ItemsSource = golferRounds;
        }

        private async Task PopulateLeagueLeaderboard(Season season)
        {
            LeagueLeaderBoardDataGrid.ItemsSource = null;

            // Populate League Leaderboard
            List<LeagueLeaderboardViewModel> teamPointsData = new List<LeagueLeaderboardViewModel>();

            var teams = await _teamService.GetTeamsByLeague(_league.LeagueId);

            foreach (Team team in teams)
            {
                // Get Total Team Points
                int totalTeamPoints = await _teamMatchupJunctionService.GetTotalTeamPointsByYear(team.TeamId, season.SeasonId);

                // Get Total golfer Points
                var golfers = await _golferTeamJunctionService.GetAllGolfersByTeam(team.TeamId);
                var golferIds = golfers.Select(g => g.GolferId).ToList();  // Ids of golfers on team

                // Get Substitute Golfer Ids
                var substitutes = await _substituteService.GetByTeam(team.TeamId);

                var golferPoints = 0;
                foreach (int id in golferIds)
                {
                    golferPoints += await _golferMatchupJunctionService.GetTotalGolferPointsByYear(id, season.SeasonId);
                }

                foreach (Substitute sub in substitutes)
                {
                    golferPoints += await _golferMatchupJunctionService.GetTotalGolferPointsByYear(sub.GolferId, season.SeasonId);
                }

                totalTeamPoints += golferPoints;

                // Create view model and add to list
                teamPointsData.Add(new LeagueLeaderboardViewModel
                {
                    TeamId = team.TeamId,
                    TeamName = team.TeamName,
                    TotalPoints = totalTeamPoints,
                });
            }

            LeagueLeaderBoardDataGrid.ItemsSource = teamPointsData.OrderByDescending(t => t.TotalPoints);
        }

        private async Task PopulateMatchupPills(Season season, int week)
        {
            MatchupPillPanel.Children.Clear();

            // Get all matchups for league season and week
            var leagueMatchups = await _matchupService.GetMatchups(_league.LeagueId, season.SeasonId, week + 1);

            if (leagueMatchups.Count > 0)
            {
                // Create MatchupPills
                foreach (var matchup in leagueMatchups)
                {
                    var fullMatchup = _matchupService.GetFullMatchup(matchup.MatchupId);

                    MatchupPill pill = new MatchupPill(matchup);

                    // Event Handler
                    pill.EditMatchup += (sender, e) => Matchup?.Invoke(sender, e);

                    MatchupPillPanel.Children.Add(pill);
                }
            }

        }

        #endregion

        private async void CreateMatchupsButton(Season season)
        {
            // Get all teams assigned to this league
            var teams = await _teamService.GetTeamsByLeague(_league.LeagueId);

            // Call Service
            var result = await _leagueService.CreateLeagueSchedule(teams, _league.LeagueId, season.SeasonId);

            // Handle Response
            if (result.IsSuccess)
            {
                NavigationService.Refresh();
            }
            else
            {
                DisplayError(result.ErrorMessage);
            }
        }

        private async void EnterScoresButton_Click(object sender, RoutedEventArgs e)
        {
            // Get season and week
            var season = (Season)SeasonComboBox.SelectedItem;
            var week = WeekComboBox.SelectedIndex;

            // Get unplayed matchups
            matchups = await _matchupService.GetUnplayedMatchups(_league.LeagueId, season.SeasonId, week + 1);

            GoToNextMatchup();
        }

        private async void GoToNextMatchup()
        {
            if (matchups == null)
            {
                Return?.Invoke();
                await PopulateUI((Season)SeasonComboBox.SelectedItem, WeekComboBox.SelectedIndex); // Reset UI in case of data changes
                return;
            }

            if (currentMatchupIndex < matchups.Count)
            {
                var currentMatchup = matchups[currentMatchupIndex];
                var fullMatchup = await _matchupService.GetFullMatchup(currentMatchup.MatchupId);
                Matchup?.Invoke(this, new MatchupEvent(fullMatchup));
            }
            else  // When matchup is done
            {
                Return?.Invoke();
                await PopulateUI((Season)SeasonComboBox.SelectedItem, WeekComboBox.SelectedIndex); // Reset UI in case of data changes
            }
        }

        public void OnMatchupScoreCompleted()
        {
            currentMatchupIndex++;
            GoToNextMatchup();
        }

        private async void SeasonComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get Value and pass to Week ComboBox
            Season season = (Season)SeasonComboBox.SelectedItem;

            if (season == null) return;

            await PopulateWeekFilter(season);
        }

        private async void WeekComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get Selected Week
            var selectedWeek = WeekComboBox.SelectedIndex;

            // Get Selected Year
            var selectedSeason = (Season)SeasonComboBox.SelectedItem;

            // Update DataGrids With selected week
            await PopulateUI(selectedSeason, selectedWeek);
        }

        private async Task CheckNextWeek(Season season, int week)
        {

            bool hasMatchups = await _matchupService.CheckMatchups(_league.LeagueId, season.SeasonId);

            // get matchups for selected week
            bool stillPlaying = await _matchupService.UnfinishedMatchups(_league.LeagueId, week + 1, season.SeasonId);

            // Show button if both are true
            bool showButton = hasMatchups && !stillPlaying;

            if (showButton)
            {
                // Check if any more weeks exist
                bool inSeason = await _matchupService.CheckMatchups(_league.LeagueId, season.SeasonId, week + 2);

                if (inSeason)
                {
                    NextWeekButton.Visibility = Visibility.Visible;
                } else
                {
                    NewSeasonButton.Visibility = Visibility.Visible;
                }

                
            }

        }

        private void NextWeekButton_Click(object sender, RoutedEventArgs e)
        {
            WeekComboBox.SelectedIndex = WeekComboBox.SelectedIndex + 1;
        }

        private void DisplayError(string error)
        {
            ErrorMessageTextBlock.Text = error;
            ErrorMessageTextBlock.Visibility = Visibility.Visible;
        }

        private async void NewSeasonButton_Click(object sender, RoutedEventArgs e)
        {
            // Show New Season Form
            var seasonForm = new CreateSeasonWindow();
            seasonForm.Owner = Window.GetWindow(this);

            var result = seasonForm.ShowDialog();

            if (result == true)
            {
                // User clicked Save
                string seasonName = seasonForm.SeasonName;

                try
                {
                    // Call service
                    var seasonResult = await _seasonService.CreateSeason(seasonName, _league.LeagueId);

                    if (seasonResult.IsSuccess)
                    {
                        // Update SeasonFilter
                        CreateMatchupsButton(seasonResult.Season);
                    }
                    else
                    {
                        DisplayError(seasonResult.ErrorMessage);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    DisplayError(ex.Message);
                    return;
                }
            }
        }
    }
}
