using Core.DTOs.GolferDTOs;
using Core.Interfaces.Service;
using Core.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using ViewModels;

namespace UI.Pages.InfoPages
{
    //TODO: Handle Clicks on round. Not Important
        // Edit any hole or delete round
    public partial class GolferInfoPage : Page
    {
        public event RoutedEventHandler? Navigate;

        private Golfer _golfer;
        private IRoundService _roundService;
        private IGolferService _golferService;

        public GolferInfoPage(Golfer golfer)
        {
            InitializeComponent();

            _golfer = golfer;
            _golferService = App.ServiceProvider.GetRequiredService<IGolferService>();
            _roundService = App.ServiceProvider.GetRequiredService<IRoundService>();

            this.DataContext = golfer;

            Loaded += GolferInfoPage_Loaded;
        }

        private async void GolferInfoPage_Loaded(object sender, RoutedEventArgs e)
        {
            await PopulateDataGrid();
        }

        private async Task PopulateDataGrid()
        {
            // Get Rounds with golferId
            var rounds = await _roundService.GetByGolferId(_golfer.GolferId);

            // Create ViewModels
            var roundViewModels = new List<RoundViewModel>();
            for (int i = 0; i < rounds.Count; i++)
            {
                roundViewModels.Add(new RoundViewModel(rounds[i]));
            }

            // Add to DataGrid
            GolferRoundsDataGrid.ItemsSource = roundViewModels;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            _golferService.Delete(_golfer.GolferId);

            Navigate?.Invoke(sender, e);
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessageTextBlock.Text = string.Empty;
            ErrorMessageTextBlock.Visibility = Visibility.Collapsed;

            CreateGolferRequest request;
            try
            {
                request = new CreateGolferRequest
                {
                    FirstName = GolferFirstNameTextBox.Text,
                    LastName = GolferLastNameTextBox.Text,
                    Handicap = ParseHandicap(GolferHandicapTextBox.Text),
                };
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
                return;
            }


            // Call service
            var result = await _golferService.UpdateGolfer(request);

            if (result.IsSuccess)
            {
                // Navigate back to this page with updated values
                Navigate?.Invoke(sender, e);
            }
            else
            {
                DisplayErrors(result);
            }
        }

        private double ParseHandicap(string text)
        {
            if (double.TryParse(text, out double handicap))
                return handicap;

            // If cannot parse to double
            throw new Exception("Handicap must be a number");
        }

        private void GolferForm_Changed(object sender, TextChangedEventArgs e)
        {
            SaveButton.Visibility = Visibility.Visible;
        }

        private void DisplayError(string error)
        {
            ErrorMessageTextBlock.Text = error;
            ErrorMessageTextBlock.Visibility = Visibility.Visible;
        }

        private void DisplayErrors(CreateGolferResult result)
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
