using System.Windows;

namespace UI.Windows
{
    /// <summary>
    /// Interaction logic for CreateSeasonWindow.xaml
    /// </summary>
    public partial class CreateSeasonWindow : Window
    {
        public string SeasonName { get; private set; } = string.Empty;

        public CreateSeasonWindow()
        {
            InitializeComponent();

            // Focus the textbox when dialog opens
            Loaded += (s, e) => SeasonNameTextBox.Focus();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Hide previous error message
            ErrorMessageTextBlock.Visibility = Visibility.Collapsed;

            // Validate input
            if (string.IsNullOrWhiteSpace(SeasonNameTextBox.Text))
            {
                DisplayError("Please enter a season name.");
                return;
            }

            // Set the season name and close with success
            SeasonName = SeasonNameTextBox.Text.Trim();
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Close without saving
            DialogResult = false;
            Close();
        }

        private void DisplayError(string error)
        {
            ErrorMessageTextBlock.Text = error;
            ErrorMessageTextBlock.Visibility = Visibility.Visible;
        }

    }
}
