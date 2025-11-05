using Core.DTOs.GolferSearchBarDTOs;
using Core.DTOs.TeamDTOs;
using Core.Interfaces.Service;
using Core.Models;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using UI.Controls;

namespace UI.Pages.Forms
{
    /// <summary>
    /// Interaction logic for CreateTeamPage.xaml
    /// </summary>
    public partial class CreateTeamPage : Page
    {
        private readonly ITeamService _teamService;
        private readonly IGolferSearchBarService _searchBarService;
        private readonly ILeagueService _leagueService;
        private readonly ILeagueSettingService _leagueSettingService;
        private readonly IGolferService _golferService;
        private readonly IGolferTeamJunctionService _golferTeamJunctionService;

        private readonly Team? _team;

        private List<GolferSearchBar> _searchBars = new List<GolferSearchBar>();

        public event RoutedEventHandler? Navigate;
        public CreateTeamPage()
        {
            InitializeComponent();

            _teamService = App.ServiceProvider.GetRequiredService<ITeamService>();
            _searchBarService = App.ServiceProvider.GetRequiredService<IGolferSearchBarService>();
            _leagueService = App.ServiceProvider.GetRequiredService<ILeagueService>();
            _leagueSettingService = App.ServiceProvider.GetRequiredService<ILeagueSettingService>();
            _golferService = App.ServiceProvider.GetRequiredService<IGolferService>();
            _golferTeamJunctionService = App.ServiceProvider.GetRequiredService<IGolferTeamJunctionService>();

            Loaded += CreateTeamPage_Loaded;
        }

        private async void CreateTeamPage_Loaded(object sender, RoutedEventArgs e)
        {
            await PopulateLeagues();
        }

        public CreateTeamPage(Team team)
        {
            InitializeComponent();

            _team = team;
            _teamService = App.ServiceProvider.GetRequiredService<ITeamService>();
            _searchBarService = App.ServiceProvider.GetRequiredService<GolferSearchBarService>();
            _leagueService = App.ServiceProvider.GetRequiredService<ILeagueService>();
            _leagueSettingService = App.ServiceProvider.GetRequiredService<ILeagueSettingService>();
            _golferService = App.ServiceProvider.GetRequiredService<IGolferService>();
            _golferTeamJunctionService = App.ServiceProvider.GetRequiredService<IGolferTeamJunctionService>();

            Loaded += (sender, e) => CreateTeamPage_Loaded(sender, e, _team.LeagueId);
        }

        private async void CreateTeamPage_Loaded(object sender, RoutedEventArgs e, int leagueId)
        {
            await PopulateLeagues(leagueId);
        }

        private async Task PopulateLeagues()
        {
            // Get All Leagues
            var leagues = await _leagueService.GetAll();

            if (!leagues.Any())
            {
                DisplayError("No leagues exist.");
                return;
            }

            LeagueComboBox.ItemsSource = leagues;
            LeagueComboBox.DisplayMemberPath = "LeagueName";
            LeagueComboBox.SelectedIndex = 0;
        }

        private async Task PopulateLeagues(int leagueId)
        {
            // Get All Leagues
            var leagues = await _leagueService.GetAll();

            if (!leagues.Any())
            {
                DisplayError("No leagues exist.");
                return;
            }
            
            LeagueComboBox.ItemsSource = leagues;
            LeagueComboBox.DisplayMemberPath = "LeagueName";

            // Get Selected league
            var selectedLeague = _leagueService.GetById(leagueId);

            if (selectedLeague != null)
            {
                LeagueComboBox.SelectedItem = selectedLeague;
            } else
            {
                LeagueComboBox.SelectedIndex = 0;
            }
        }

        private async void LeagueComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (LeagueComboBox.SelectedIndex == - 1)
            {
                DisplayError("Select a league.");
                return;
            }

            var selectedLeague = LeagueComboBox.SelectedItem as League;

            if (selectedLeague == null)
            {
                DisplayError("League does not exist.");
                return;
            }

            // Get team size from selected league's league settings
            var leagueSetting = await _leagueSettingService.GetById(selectedLeague.LeagueSettingsId);
            int teamSize = leagueSetting.TeamSize;

            // Show SearchBars
            await PopulateGolferPanel(teamSize);
        }

        private async Task PopulateGolferPanel(int teamSize)
        {
            // Reset View
            GolferPanel.Children.Clear();

            if (_searchBars.Count > 0)
            {
                _searchBars.Clear();
            }

            if (_team == null)
            {
                // Add teamSize amount of SearchBars to view
                for (int i = 0; i < teamSize; i++)
                {
                    GolferSearchBar searchBar = new GolferSearchBar(null, _golferService);

                    _searchBars.Add(searchBar);
                    GolferPanel.Children.Add(searchBar);
                }
            } else
            {
                // Get list of golfers in team
                var golfers = await _golferTeamJunctionService.GetAllGolfersByTeam(_team.TeamId);

                if (golfers.Count == teamSize)
                {
                    // Populate with golfer names
                    foreach (var golfer in golfers)
                    {
                        GolferSearchBar searchBar = new GolferSearchBar(golfer.FullName, _golferService);

                        _searchBars.Add(searchBar);
                        GolferPanel.Children.Add(searchBar);
                    }
                } else
                {
                    // Add teamSize amount of SearchBars to view
                    for (int i = 0; i < teamSize; i++)
                    {
                        GolferSearchBar searchBar = new GolferSearchBar(null, _golferService);

                        _searchBars.Add(searchBar);
                        GolferPanel.Children.Add(searchBar);
                    }
                }
            }
        }

        private async Task<List<Golfer>> CheckSearchBars()
        {
            if (_searchBars.Count == 0)
            {
                throw new Exception("Select a league.");
            }

            if (LeagueComboBox.SelectedItem as League == null)
            {
                throw new Exception("Select a league.");
            }

            // Get Values from Search Bars
            var names = new List<string>();
            foreach (var searchBar in _searchBars)
            {
                names.Add(searchBar.GolferName);
            }

            // Create SearchBar Request
            CreateGolferSearchBarRequest request;
            if (_team == null)
            {
                request = new CreateGolferSearchBarRequest
                {
                    GolferNames = names,
                    League = LeagueComboBox.SelectedItem as League,
                };
            } else
            {
                request = new CreateGolferSearchBarRequest
                {
                    GolferNames = names,
                    League = LeagueComboBox.SelectedItem as League,
                    TeamId = _team.TeamId
                };
            }

            // Check Values
            var result = await _searchBarService.PerformGolferCheck(request);

            if (result.IsSuccess)
            {
                return result.Golfers;
            } else
            {
                throw new Exception(result.ErrorMessage);
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessageTextBlock.Text = String.Empty;
            ErrorMessageTextBlock.Visibility = Visibility.Collapsed;

            // Get Golfer Names from search bars
            List<Golfer> golfers;
            try
            {
                golfers = await CheckSearchBars();
            }
            catch (Exception ex)
            {
                DisplayError(ex);
                return;
            }

            if (golfers == null)
            {
                DisplayError("Enter golfer names.");
                return;
            }

            // Grab League
            var league = LeagueComboBox.SelectedItem as League;

            if (league == null)
            {
                DisplayError("Select a league.");
                return;
            }


            if (_team == null)
            {

                var team = new CreateTeamRequest
                {
                    TeamName = string.Join(" & ", _searchBars.Select(s => s.GolferName)),
                    LeagueId = league.LeagueId,
                    Golfers = golfers,
                };

                var result = await _teamService.CreateTeam(team);

                if (result.IsSuccess)
                {
                    Navigate?.Invoke(sender, e);
                } else
                {
                    DisplayErrors(result);
                }
            } else
            {
                var team = new CreateTeamRequest
                {
                    TeamName = string.Join(" & ", _searchBars.Select(s => s.GolferName)),
                    TeamId = _team.TeamId,
                    LeagueId = league.LeagueId,
                    Golfers = golfers,
                };

                var result = await _teamService.UpdateTeam(team);

                if (result.IsSuccess)
                {
                    Navigate?.Invoke(sender, e);
                }
                else
                {
                    DisplayErrors(result);
                }
            }
        }

        private void DisplayError(string error)
        {
            ErrorMessageTextBlock.Text = error;
            ErrorMessageTextBlock.Visibility = Visibility.Visible;
        }

        private void DisplayError(Exception error)
        {
            ErrorMessageTextBlock.Text = error.Message;
            ErrorMessageTextBlock.Visibility = Visibility.Visible;
        }

        private void DisplayErrors(CreateTeamResult result)
        {
            if (result.ValidationErrors.Any())
            {
                ErrorMessageTextBlock.Text = String.Join(", ", result.ValidationErrors);
                ErrorMessageTextBlock.Visibility = Visibility.Visible;
            }
            else if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                ErrorMessageTextBlock.Text = result.ErrorMessage;
                ErrorMessageTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void DisplayErrors(CreateGolferSearchBarResult result)
        {
            if (result.ValidationErrors.Any())
            {
                ErrorMessageTextBlock.Text = String.Join(", ", result.ValidationErrors);
                ErrorMessageTextBlock.Visibility = Visibility.Visible;
            }
            else if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                ErrorMessageTextBlock.Text = result.ErrorMessage;
                ErrorMessageTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}
