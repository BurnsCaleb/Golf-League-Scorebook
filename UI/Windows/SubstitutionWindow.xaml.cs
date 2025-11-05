using Core.DTOs.GolferSearchBarDTOs;
using Core.Interfaces.Service;
using Core.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using UI.Controls;

namespace UI.Windows
{
    /// <summary>
    /// Interaction logic for SubstitutionWindow.xaml
    /// </summary>
    public partial class SubstitutionWindow : Window
    {
        private readonly IGolferService _golferService;
        private readonly IGolferSearchBarService _searchBarService;

        private GolferSearchBar _searchBar;
        private League _league;
        private int _teamId;

        public Golfer Golfer { get; private set; }

        public SubstitutionWindow(League league, int teamId)
        {
            InitializeComponent();

            _golferService = App.ServiceProvider.GetRequiredService<IGolferService>();
            _searchBarService = App.ServiceProvider.GetRequiredService<IGolferSearchBarService>();

            _league = league;
            _teamId = teamId;

            // Create GolferSearchBar
            _searchBar = new GolferSearchBar(null, _golferService);
            GolferPanel.Children.Add(_searchBar);

            Loaded += SubstitutionWindow_Loaded;
        }

        private async void SubstitutionWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await Dispatcher.BeginInvoke(new Action(() =>
            {
                _searchBar.GolferSearchBox.Focus();
            }), System.Windows.Threading.DispatcherPriority.Input);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Close without saving
            DialogResult = false;
            Close();
        }

        private async void AddSubButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear Error Message
            ErrorMessageTextBlock.Visibility = Visibility.Collapsed;

            // Create request
            var request = new CreateGolferSearchBarRequest
            {
                GolferNames = new List<string> { _searchBar.GolferName },
                League = _league,
                TeamId = _teamId
            };

            // Call service
            var result = await _searchBarService.PerformGolferCheck(request);

            if (result.IsSuccess)
            {
                Golfer = result.Golfers.First();
                DialogResult = true;
                Close();
            } else
            {
                DisplayError(result.ErrorMessage);
                return;
            }
        }

        private void DisplayError(string error)
        {
            ErrorMessageTextBlock.Text = error;
            ErrorMessageTextBlock.Visibility = Visibility.Visible;
        }
    }
}
