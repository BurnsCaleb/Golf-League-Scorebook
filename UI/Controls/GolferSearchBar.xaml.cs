using Core.Interfaces.Service;
using Core.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UI.Controls
{
    /// <summary>
    /// Interaction logic for GolferSearchBar.xaml
    /// </summary>
    public partial class GolferSearchBar : UserControl
    {
        private readonly IGolferService _golferService;
        public string GolferName { get; set; } = string.Empty;

        public GolferSearchBar(string? name, IGolferService golferService)
        {
            InitializeComponent();

            _golferService = golferService;

            if (!string.IsNullOrEmpty(name))
            {
                GolferSearchBox.Text = name;

                SearchResultsListBox.Visibility = Visibility.Collapsed;
            }
        }

        private async void GolferSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Search Golfers
            string query = GolferSearchBox.Text.Trim().ToLower();

            var results = await _golferService.Search(query);
            
            // Show Results ListBox
            if (!string.IsNullOrEmpty(query) && results.Any())
            {
                SearchResultsListBox.ItemsSource = results;
                SearchResultsListBox.DisplayMemberPath = "FullName";
                SearchResultsListBox.Visibility = Visibility.Visible;
                SearchResultsListBox.SelectedIndex = 0;
            } else
            {
                SearchResultsListBox.Visibility = Visibility.Collapsed;
            }

            // Update GolferName
            GolferName = GolferSearchBox.Text;
        }

        private void SearchResultsListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && SearchResultsListBox.SelectedItem is Golfer selectedGolfer)
            {
                // Populate Search Box with Golfer Name
                GolferSearchBox.Text = selectedGolfer.FullName;

                // Set property to name
                GolferName = selectedGolfer.FullName;

                SearchResultsListBox.Visibility = Visibility.Collapsed;
            }
            else if (e.Key == Key.Escape)
            {
                SearchResultsListBox.Visibility = Visibility.Collapsed;
            }
        }

        private void SearchResultsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SearchResultsListBox.SelectedItem is Golfer selectedGolfer)
            {
                // Populate Search Box with Golfer Name
                GolferSearchBox.Text = selectedGolfer.FullName;

                // Set property to name
                GolferName = selectedGolfer.FullName;

                SearchResultsListBox.Visibility = Visibility.Collapsed;
            }
        }
    }
}
