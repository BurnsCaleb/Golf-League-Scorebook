using Core.DTOs.HoleDTOs;
using Core.DTOs.MatchupDTOs;
using Core.Interfaces.Service;
using Core.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UI.Controls;
using UI.Pages.InfoPages;
using ViewModels;

namespace UI.Pages.Forms
{
    /// <summary>
    /// Interaction logic for EnterScoresPage.xaml
    /// </summary>
    public partial class EnterScoresPage : Page
    {
        private Matchup _matchup;
        private List<GolferHoleScore> scoreData = new List<GolferHoleScore>();

        private readonly IGolferService _golferService;
        private readonly IScoringService _scoringService;
        private readonly IMatchupService _matchupService;
        private readonly IHoleScoreService _holeScoreService;
        private readonly ISubstituteService _substituteService;
        private readonly ITeamService _teamService;

        // Data relative to current golfer being processed
        private List<Round> rounds;
        private Round currentRound;

        private List<Golfer> golfers = new List<Golfer>();
        private Golfer currentGolfer;

        private bool programmaticEntry = false;
        private int currentGolferCount = 0;
        private List<int> strokesReceivedByHole;
        private Hole currentHole;
        private GolferHoleScore currentHoleScoreData;
        private bool firstHole = true;
        private bool tenPlus = false;

        private LeagueInfoPage _parentPage;

        public EnterScoresPage(Matchup matchup, LeagueInfoPage parentPage)
        {
            InitializeComponent();

            _matchup = matchup;
            _parentPage = parentPage;

            _golferService = App.ServiceProvider.GetRequiredService<IGolferService>();
            _scoringService = App.ServiceProvider.GetRequiredService<IScoringService>();
            _matchupService = App.ServiceProvider.GetRequiredService<IMatchupService>();
            _holeScoreService = App.ServiceProvider.GetRequiredService<IHoleScoreService>();
            _substituteService = App.ServiceProvider.GetRequiredService<ISubstituteService>();
            _teamService = App.ServiceProvider.GetRequiredService<ITeamService>();

            GrabMatchupScores();

            Loaded += EnterScoresPage_Loaded;
        }

        private async void EnterScoresPage_Loaded(object sender, RoutedEventArgs e)
        {
            await InitializeData();
        }


        /// <summary>
        /// Attempts to populate scoreData with matchup information.
        /// </summary>
        private void GrabMatchupScores()
        {
            // call service
            var result = _matchupService.GrabMatchupScores(_matchup);

            if (result == null || result.Count == 0) return;

            // Reset score data and add data
            scoreData.Clear();

            foreach (var item in result)
            {
                scoreData.Add(item);
            }

        }


        /// <summary>
        /// Grabs necessary data and sets global variables
        /// </summary>
        /// <param name="golfer"></param>
        private void InitializeGolfer()
        {
            // Get Course Handicap / Total Strokes Received
            double courseHandicap = _golferService.GetCourseHandicap(currentGolfer, _matchup.League.Course);

            // Convert to int
            int roundedHandicap = (int)Math.Round(courseHandicap);

            // Get Strokes Received by Hole
            strokesReceivedByHole = _scoringService.DistributeStrokes(_matchup.League.Course.Holes.ToList(), roundedHandicap);
        }


        /// <summary>
        /// Only ran at the start of the page
        /// </summary>
        private async Task InitializeData()
        {
            // Set Golfers and current golfer
            foreach (var junction in _matchup.GolferMatchupJunctions)
            {
                golfers.Add(junction.Golfer);
            }

            currentGolfer = golfers.First();

            InitializeGolfer();

            // Set First Hole Data
            currentHole = _matchup.League.Course.Holes.Where(h => h.HoleNum == 1).First();

            // Set Rounds and current round
            rounds = _matchup.Rounds.ToList();
            if (rounds.Count != 0)
            {
                currentRound = rounds.First();
            }

            // Set Course Name
            CourseNameTextBlock.Text = _matchup.League.Course.CourseName;

            await NextHole();
        }


        /// <summary>
        /// Process entered score as well as calculating net score and saving data.
        /// </summary>
        private async Task ProcessScore(object sender, RoutedEventArgs e, GolferHoleScore holeScoreData)
        {
            ErrorMessageTextBlock.Text = string.Empty;
            ErrorMessageTextBlock.Visibility = Visibility.Collapsed;

            // Empty TextBox
            if (ScoreTextBox.Text == string.Empty) return;

            // Grab only numeric values
            string scoreText = ScoreTextBox.Text;

            // Get Gross Score Entered
            int grossScore = 0;
            try
            {
                grossScore = Convert.ToInt16(scoreText);

                if (grossScore < 1) throw new Exception("Score must be larger than 0.");
            }
            catch (FormatException)
            {
                ErrorMessageTextBlock.Text = "A golfer's score must be a numeric value.";
                ErrorMessageTextBlock.Visibility = Visibility.Visible;
                return;
            }
            catch (Exception)
            {
                ErrorMessageTextBlock.Text = "An error occurred. Please try again.";
                ErrorMessageTextBlock.Visibility = Visibility.Visible;
                return;
            }

            int netScore = _scoringService.CalculateNetScore(strokesReceivedByHole, grossScore, currentHole);

            // Update GolferHoleScore
            holeScoreData.GrossScore = grossScore;
            holeScoreData.NetScore = netScore;

            // Check for an existing scoredata to update
            var existingData = scoreData
                .FirstOrDefault(d => d.GolferId == holeScoreData.GolferId && d.HoleId == holeScoreData.HoleId);

            if (existingData != null)
            {
                int index = scoreData.IndexOf(existingData);
                scoreData[index] = holeScoreData;
            } else
            {
                scoreData.Add(holeScoreData);
            }

            // Increment to Next Hole
            await NextHole();
        }


        /// <summary>
        /// Updates data for the next hole.
        /// </summary>
        private async Task NextHole()
        {
            if (!firstHole) // Skip this logic on first hole.
            {
                // Get The Next Hole Number
                var nextHoleNum = currentHole.HoleNum + 1;


                // Check if there is another hole
                if (currentHole.HoleNum == _matchup.League.Course.NumHoles)
                {
                    if (golfers[golfers.Count - 1].GolferId == currentGolfer.GolferId)  // Last golfer
                    {
                        // Check if all scoreData has values
                        int expectedScoreData = golfers.Count * _matchup.League.Course.NumHoles;
                        if (scoreData.Count != expectedScoreData)
                        {
                            DisplayError($"Some holes in this matchup are missing scores.");
                            return;
                        }

                        // Check if gross score has value
                        foreach (var data in scoreData)
                        {
                            if (data.GrossScore <= 0)
                            {
                                
                                DisplayError($"Cannot receive a score of 0 for {data.GolferName} on hole {data.HoleNumber}.");
                                return;
                            }
                        }

                        await EndMatchup();
                        return;
                    }

                    // Update next round
                    int nextIndex = golfers.IndexOf(currentGolfer) + 1;
                    currentGolfer = golfers[nextIndex];

                    InitializeGolfer();

                    // Restart round
                    nextHoleNum = 1;
                }

                // Update Current Hole
                currentHole = _matchup.League.Course.Holes.Where(h => h.HoleNum == nextHoleNum).First();
            }
            else
            {
                firstHole = false;
            }

            if (currentHole.HoleNum == 1)
            {
                PrevHoleButton.Visibility = Visibility.Hidden;
            } else
            {
                PrevHoleButton.Visibility = Visibility.Visible;
            }
            
            ResetTenPlus();

            int teamId;

            // Get current golfers team id
            var team = await _teamService.GetByGolferLeague(currentGolfer.GolferId, _matchup.LeagueId);

            // Grab TeamId from substitutes table if team is null
            if (team == null)
            {
                var substitute = await _substituteService.GetByGolferMatchup(currentGolfer.GolferId, _matchup.MatchupId);

                if (substitute == null)
                {
                    DisplayError($"Could not find {currentGolfer.FullName}'s team.");
                    return;
                }

                teamId = substitute.TeamId;
            } else
            {
                teamId = team.TeamId;
            }

            // See if Score Data has value for this golfer for new current hole
            GolferHoleScore? existentScoreData = scoreData
                .Where(d => d.GolferId == currentGolfer.GolferId)
                .Where(d => d.HoleId == currentHole.HoleId)
                .FirstOrDefault();

            if (existentScoreData != null)
            {
                currentHoleScoreData = existentScoreData;

                programmaticEntry = true;

                this.DataContext = currentHoleScoreData;

                await Dispatcher.BeginInvoke(new Action(() =>
                {
                    // reset programmaticEntry
                    programmaticEntry = false;

                    // Focus on the textbox
                    ScoreTextBox.Focus();

                    // Position caret behind all text
                    ScoreTextBox.CaretIndex = ScoreTextBox.Text.Length;
                }), System.Windows.Threading.DispatcherPriority.Input);
            } else
            {
                // Set GolferHoleScore ViewModel for new hole
                currentHoleScoreData = new GolferHoleScore
                {
                    GolferId = currentGolfer.GolferId,
                    GolferName = currentGolfer.FullName,
                    TeamId = teamId,
                    GolferHandicap = currentGolfer.Handicap,
                    HoleId = currentHole.HoleId,
                    HoleNumber = currentHole.HoleNum,
                    HoleDistance = currentHole.Distance,
                    HolePar = currentHole.Par,
                    HoleHandicap = currentHole.Handicap,
                };

                programmaticEntry = true;

                this.DataContext = currentHoleScoreData;

                await Dispatcher.BeginInvoke(new Action(() =>
                {
                    // reset programmaticEntry
                    programmaticEntry = false;

                    // Focus on the textbox
                    ScoreTextBox.Focus();

                    // Position caret behind all text
                    ScoreTextBox.CaretIndex = ScoreTextBox.Text.Length;
                }), System.Windows.Threading.DispatcherPriority.Input);
            }

            // New ScoreTab
            ScorecardTab scorecard = new ScorecardTab(CreateScoreTabView(), _matchup, teamId);

            scorecard.SubstituteGolfer += async (sender, e) => await MakeSubstitution(currentGolfer, e.Golfer);
            scorecard.RemoveSubstitute += async (sender, e) => await RemoveSubstitution(scorecard.ActiveGolfer, scorecard.OriginalGolfer);

            // ScoreBugs
            PopulateScorePreviews();

            // Put in view
            ScoreTabPanel.Children.Clear();
            ScoreTabPanel.Children.Add(scorecard);

        }


        /// <summary>
        /// Perform score calculations and award points based on scoring rule.
        /// </summary>
        private async Task EndMatchup()
        {
            // Create request
            var request = new EndMatchupRequest
            {
                Matchup = _matchup,
                ScoreData = scoreData,
                Golfers = golfers,
                Rounds = rounds,
            };

            // Call service
            var result = await _matchupService.EndMatchup(request);

            if (result.IsSuccess)
            {
                _parentPage.OnMatchupScoreCompleted();
            } else
            {
                DisplayError(result.ErrorMessage);
                return;
            }
        }


        /// <summary>
        /// Creates and returns a ScoreTabVieModel for 3 holes.
        /// </summary>
        /// <returns></returns>
        private ScorecardTabViewModel CreateScoreTabView()
        {
            ScorecardTabViewModel view = new ScorecardTabViewModel();

            // Grab Current Hole Number
            int holeNum = currentHole.HoleNum;

            // Grab All GolferHoleScore for current golfer. Order By Hole Number
            var holeData = scoreData.Where(d => d.GolferId == currentGolfer.GolferId).OrderBy(d => d.HoleNumber).ToList();

            int startHole, middleHole, endHole;

            if (holeNum == 1 || holeNum == 2)
            {
                // Hole 1: Show holes 1, 2, 3
                startHole = 1;
                middleHole = 2;
                endHole = 3;
            }
            else
            {
                // Hole 3+: Show previous 2, current
                startHole = holeNum - 2;
                middleHole = holeNum - 1;
                endHole = holeNum;
            }

            List<string> displayData = new List<string>();
            int[] displayHoles = new[] { startHole, middleHole, endHole };

            foreach (int hole in displayHoles)
            {
                var tempHoleData = holeData
                    .Where(d => d.HoleNumber == hole)
                    .Select(d => d.GrossScore)
                    .DefaultIfEmpty(0)
                    .First();

                string holeScore = (tempHoleData == 0) ? "-" : tempHoleData.ToString();

                displayData.Add(holeScore);
            }

            // Set Golfer Name
            view.GolferName = currentGolfer.FullName;

            // Set Values
            view.TopHoleNum = $"Hole {startHole} :";
            view.TopHoleScore = displayData[0];

            view.MiddleHoleNum = $"Hole {middleHole} :";
            view.MiddleHoleScore = displayData[1];

            view.BottomHoleNum = $"Hole {endHole} :";
            view.BottomHoleScore = displayData[2];

            return view;
        }


        /// <summary>
        /// Populate the tabs underneath the header for other golfer scores
        /// </summary>
        private void PopulateScorePreviews()
        {
            ScoresPanel.Children.Clear();

            // Call service
            List<GolferHoleScore> result = _holeScoreService.PopulateScorePreviews(golfers, scoreData, currentGolfer, currentHole);

            // Create scorebugs
            foreach (GolferHoleScore data in result)
            {
                Scorebug scorebug = new Scorebug(data);
                scorebug.Navigate += async (sender, e) => await JumpToHoleEntry(e.ScoreData);

                ScoresPanel.Children.Add(scorebug);
            }
        }

        private async Task JumpToHoleEntry(GolferHoleScore data)
        {
            List<Hole> holes = _matchup.League.Course.Holes.ToList();

            var result = _holeScoreService.JumpToHole(data, golfers, holes);

            // Set first hole true to skip hole increment logic
            firstHole = true;

            // Set current golfer
            currentGolfer = result.Golfer;

            // Set current hole
            currentHole = result.Hole;

            await NextHole();

        }

        private async void ScoreTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!programmaticEntry)
            {
                await ProcessScore(sender, e, currentHoleScoreData);
            } else
            {
                programmaticEntry = false;
            }
        }

        private async void NextHoleButton_Click(object sender, RoutedEventArgs e)
        {
            await NextHole();
        }

        private async void PrevHoleButton_Click(object sender, RoutedEventArgs e)
        {
            List<Hole> holes = _matchup.League.Course.Holes.ToList();
            List<Team> teams = _matchup.TeamMatchupJunctions.Select(t => t.Team).ToList();

            var result = await _holeScoreService.GoToPreviousHole(currentHole, holes, currentGolfer, teams, scoreData, _matchup);

            if (result.IsSuccess)
            {
                if (result.HasData)
                {
                    currentHoleScoreData = result.HoleScoreData;

                    programmaticEntry = true;
                    this.DataContext = currentHoleScoreData;

                    await Dispatcher.BeginInvoke(new Action(() =>
                    {
                        // reset programmaticEntry
                        programmaticEntry = false;

                        // Focus on the textbox
                        ScoreTextBox.Focus();

                        // Position caret behind all text
                        ScoreTextBox.CaretIndex = ScoreTextBox.Text.Length;
                    }), System.Windows.Threading.DispatcherPriority.Input);
                }
                else
                {
                    currentHoleScoreData = result.HoleScoreData;
                    this.DataContext = currentHoleScoreData;
                }
            } else
            {
                DisplayError(result.ErrorMessage);
                return;
            }

            var newCurrentHole = holes.FirstOrDefault(h => h.HoleId == result.HoleScoreData.HoleId);
            currentHole = newCurrentHole ?? currentHole;

            // New ScoreTab
            ScorecardTab scorecard = new ScorecardTab(CreateScoreTabView(), _matchup, currentHoleScoreData.TeamId);

            // ScoreBugs
            PopulateScorePreviews();

            // Put in view
            ScoreTabPanel.Children.Clear();
            ScoreTabPanel.Children.Add(scorecard);
        }

        private void TenPlusButton_Click(object sender, RoutedEventArgs e)
        {
            // Toggle mode
            tenPlus = !tenPlus;

            // Update UI
            if (tenPlus)
            {
                TenPlusButton.Background = (Brush)FindResource("Accent");
                programmaticEntry = true;
            } else
            {
                TenPlusButton.Background = (Brush)FindResource("Background");
                programmaticEntry = false;
            }

        }

        private void ResetTenPlus()
        {
            tenPlus = false;

            TenPlusButton.Background = (Brush)FindResource("Background");
            programmaticEntry = false;
        }

        private async Task MakeSubstitution(Golfer oldGolfer, Golfer newGolfer)
        {
            await _substituteService.MakeSubstitution(oldGolfer, newGolfer, _matchup);

            // Update Golfer
            int index = golfers.IndexOf(oldGolfer);
            golfers[index] = newGolfer;

            currentGolfer = newGolfer;

            // Clear score data for old golfer
            var scoreDataToRemove = scoreData.Where(s => s.GolferId == oldGolfer.GolferId).ToList();
            foreach (var data in scoreDataToRemove)
            {
                scoreData.Remove(data);
            }

            // Update Handicap Data
            InitializeGolfer();

            // Get teamId for UI
            Team team = await _teamService.GetByGolferLeague(oldGolfer.GolferId, _matchup.LeagueId);
            int teamId = team.TeamId;

            // Update CurrentHoleScoreData
            currentHoleScoreData = new GolferHoleScore
            {
                GolferId = currentGolfer.GolferId,
                GolferName = currentGolfer.FullName,
                TeamId = teamId,
                GolferHandicap = currentGolfer.Handicap,
                HoleId = currentHole.HoleId,
                HoleNumber = currentHole.HoleNum,
                HoleDistance = currentHole.Distance,
                HolePar = currentHole.Par,
                HoleHandicap = currentHole.Handicap,
            };

            // Update UI
            // New ScoreTab
            ScorecardTab scorecard = new ScorecardTab(CreateScoreTabView(), _matchup, teamId);

            scorecard.SubstituteGolfer += async (sender, e) => await MakeSubstitution(currentGolfer, e.Golfer);
            scorecard.RemoveSubstitute += async (sender, e) => await RemoveSubstitution(scorecard.ActiveGolfer, scorecard.OriginalGolfer);

            // ScoreBugs
            PopulateScorePreviews();

            // Put in view
            ScoreTabPanel.Children.Clear();
            ScoreTabPanel.Children.Add(scorecard);
        }

        private async Task RemoveSubstitution(Golfer oldGolfer, Golfer originalGolfer)
        {
            await _substituteService.RemoveSubstitution(oldGolfer, _matchup);

            // Update Golfer
            int index = golfers.IndexOf(oldGolfer);
            golfers[index] = originalGolfer;

            currentGolfer = originalGolfer;

            // Clear score data for old golfer
            var scoreDataToRemove = scoreData.Where(s => s.GolferId == oldGolfer.GolferId).ToList();
            foreach (var data in scoreDataToRemove)
            {
                scoreData.Remove(data);
            }

            // Get teamId for UI
            Team team = await _teamService.GetByGolferLeague(originalGolfer.GolferId, _matchup.LeagueId);
            int teamId = team.TeamId;

            // Update UI
            // New ScoreTab
            ScorecardTab scorecard = new ScorecardTab(CreateScoreTabView(), _matchup, teamId);

            scorecard.SubstituteGolfer += async (sender, e) => await MakeSubstitution(currentGolfer, e.Golfer);
            scorecard.RemoveSubstitute += async (sender, e) => await RemoveSubstitution(scorecard.ActiveGolfer, scorecard.OriginalGolfer);

            // ScoreBugs
            PopulateScorePreviews();

            // Put in view
            ScoreTabPanel.Children.Clear();
            ScoreTabPanel.Children.Add(scorecard);

        }

        private void DisplayError(string error)
        {
            ErrorMessageTextBlock.Text = error;
            ErrorMessageTextBlock.Visibility = Visibility.Visible;
        }
    }
}
