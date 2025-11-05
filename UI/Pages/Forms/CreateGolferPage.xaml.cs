#nullable disable

using Core.DTOs.GolferDTOs;
using Core.Interfaces.Service;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace UI.Pages.Forms
{
    /// <summary>
    /// Interaction logic for CreateGolferPage.xaml
    /// </summary>
    public partial class CreateGolferPage : Page
    {
        private readonly IGolferService _golferService;
        public event RoutedEventHandler SaveGolferEvent;

        public CreateGolferPage()
        {
            InitializeComponent();
            _golferService = App.ServiceProvider.GetRequiredService<IGolferService>();
        }

        private async void SaveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateGolferRequest request;
            try
            {
                request = new CreateGolferRequest
                {
                    FirstName = FirstNameTextBox.Text.Trim(),
                    LastName = LastNameTextBox.Text.Trim(),
                    Handicap = ParseHandicap(HandicapTextbox.Text),
                };
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
                return;
            }
            

            // Call service
            var result = await _golferService.CreateGolfer(request);

            if (result.IsSuccess)
            {
                ClearForm();

                SaveGolferEvent?.Invoke(sender, e);
            } else
            {
                DisplayErrors(result);
            }
        }

        private double ParseHandicap(string text)
        {
            string handicapText;

            if (text.StartsWith("+"))
            {
                handicapText = Regex.Match(text, @"\d+").ToString();

                if (double.TryParse(text, out double handicap))
                    return -handicap;
            }
            else
            {
                handicapText = Regex.Match(text, @"\d+").ToString();

                if (double.TryParse(text, out double handicap))
                {
                    return handicap;
                }
            }

                // If cannot parse to double
                throw new Exception("Handicap must be a number and start with - or +.");
        }

        private void ClearForm()
        {
            FirstNameTextBox.Clear();
            LastNameTextBox.Clear();
            HandicapTextbox.Clear();

            ErrorMessageTextBlock.Text = string.Empty;
            ErrorMessageTextBlock.Visibility = Visibility.Collapsed;
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
            else if (!string.IsNullOrEmpty(result.ErrorMessage)) {
                ErrorMessageTextBlock.Text = result.ErrorMessage;
                ErrorMessageTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}
