using Core.DTOs.LeagueDTOs;
using Core.Interfaces.Service;
using Core.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace UI.Pages.Forms
{
    /// <summary>
    /// Interaction logic for CreateLeaguePage.xaml
    /// </summary>
    public partial class CreateLeaguePage : Page
    {
        public event RoutedEventHandler? Navigate;

        private readonly ILeagueService _leagueService;
        private readonly ILeagueSettingService _leagueSettingService;
        private readonly ICourseService _courseService;

        private readonly League? _league;
        private readonly int? _leagueId;

        public CreateLeaguePage()
        {
            InitializeComponent();

            _leagueService = App.ServiceProvider.GetRequiredService<ILeagueService>();
            _leagueSettingService = App.ServiceProvider.GetRequiredService<ILeagueSettingService>();
            _courseService = App.ServiceProvider.GetRequiredService<ICourseService>();

            Loaded += CreateLeaguePage_Loaded;
        }

        private async void CreateLeaguePage_Loaded(object sender, RoutedEventArgs e)
        {
            await PopulateSettings();
            await PopulateCourses();
        }

        public CreateLeaguePage(League league)
        {
            InitializeComponent();

            _league = league;
            _leagueService = App.ServiceProvider.GetRequiredService<ILeagueService>();
            _leagueSettingService = App.ServiceProvider.GetRequiredService<ILeagueSettingService>();
            _courseService = App.ServiceProvider.GetRequiredService<ICourseService>();

            Loaded += (sender, e) => CreateLeaguePage_Loaded(sender, e, _league.LeagueSettingsId, _league.CourseId);
        }

        private async void CreateLeaguePage_Loaded(object sender, RoutedEventArgs e, int leagueSettingsId, int courseId)
        {
            await PopulateSettings(leagueSettingsId);
            await PopulateCourses(courseId);
        }

        private async Task PopulateSettings()
        {
            var leagueSettings = await _leagueSettingService.GetLeagueSettings();

            if (leagueSettings == null)
            {
                DisplayError("No league settings found.");
                return;
            }

            SettingsComboBox.ItemsSource = leagueSettings;
            SettingsComboBox.DisplayMemberPath = "LeagueSettingsName";

            SettingsComboBox.SelectedIndex = 0;
        }

        private async Task PopulateSettings(int leagueSettingsId)
        {
            var leagueSettings = await _leagueSettingService.GetLeagueSettings();

            if (leagueSettings == null)
            {
                DisplayError("No league settings found.");
                return;
            }

            SettingsComboBox.ItemsSource = leagueSettings;
            SettingsComboBox.DisplayMemberPath = "LeagueSettingsName";

            var selectedLeagueSettings = _leagueSettingService.GetById(leagueSettingsId);
            if (selectedLeagueSettings != null)
            {
                SettingsComboBox.SelectedItem = selectedLeagueSettings;
            } else
            {
                SettingsComboBox.SelectedIndex = 0;
            }
        }

        private async Task PopulateCourses()
        {
            var courses = await _courseService.GetAll();

            if (courses == null)
            {
                DisplayError("No courses found.");
                return;
            }

            CourseComboBox.ItemsSource = courses;
            CourseComboBox.DisplayMemberPath = "CourseName";

            CourseComboBox.SelectedIndex = 0;
        }

        private async Task PopulateCourses(int courseId)
        {
            var courses = await _courseService.GetAll();

            if (courses == null)
            {
                DisplayError("No courses found.");
                return;
            }

            CourseComboBox.ItemsSource = courses;
            CourseComboBox.DisplayMemberPath = "CourseName";

            var selectedCourse = await _courseService.GetById(courseId);
            if (selectedCourse != null)
            {
                CourseComboBox.SelectedItem = selectedCourse;
            } else
            {
                CourseComboBox.SelectedIndex = 0;
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessageTextBlock.Text = string.Empty;
            ErrorMessageTextBlock.Visibility = Visibility.Collapsed;

            // Check if settings has a value
            if (SettingsComboBox.SelectedIndex == -1)
            {
                DisplayError("Please select a league setting.");
                return;
            }

            // Check if course has a value
            if (CourseComboBox.SelectedIndex == -1)
            {
                DisplayError("Please select a course.");
                return;
            }

            // Grab league setting
            var selectedLeagueSetting = SettingsComboBox.SelectedItem as LeagueSetting;
            if (selectedLeagueSetting == null)
            {
                DisplayError("Select a league setting.");
                return;
            }

            // Grab course
            var selectedCourse = CourseComboBox.SelectedItem as Course;
            if (selectedCourse == null)
            {
                DisplayError("Select a course.");
                return;
            }


            var request = new CreateLeagueRequest
            {
                LeagueName = LeagueNameTextBox.Text.Trim(),
                LeagueSettingsId = selectedLeagueSetting.LeagueSettingsId,
                CourseId = selectedCourse.CourseId
            };

            // Create new league or update previous
            CreateLeagueResult result;
            if (_leagueId == null)
            {
                result = await _leagueService.CreateLeague(request);
            } else
            {
                request.LeagueId = _leagueId;

                result = await _leagueService.UpdateLeague(request);
            }

            if (result.IsSuccess)
            {
                ClearForm();
                Navigate?.Invoke(sender, e);
            } else
            {
                DisplayErrors(result);
            }
        }

        private void ClearForm()
        {
            LeagueNameTextBox.Clear();
            SettingsComboBox.ItemsSource = null;
            CourseComboBox.ItemsSource = null;
        }

        private void DisplayError(string error)
        {
            ErrorMessageTextBlock.Text = error;
            ErrorMessageTextBlock.Visibility = Visibility.Visible;
        }

        private void DisplayErrors(CreateLeagueResult result)
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
