using Core.DTOs.LeagueSettingsDTOs;
using Core.Interfaces.Service;
using Core.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace UI.Pages.Forms
{
    /// <summary>
    /// Interaction logic for CreateNewLeagueSettingsPage.xaml
    /// </summary>
    public partial class CreateNewLeagueSettingsPage : Page
    {
        private readonly ILeagueSettingService _settingService;
        private readonly IScoringRuleService _ruleService;

        private readonly LeagueSetting? _leagueSetting;

        public event RoutedEventHandler? Navigate;


        public CreateNewLeagueSettingsPage()
        {
            InitializeComponent();

            _settingService = App.ServiceProvider.GetRequiredService<ILeagueSettingService>();
            _ruleService = App.ServiceProvider.GetRequiredService<IScoringRuleService>();

            Loaded += CreateNewLeagueSettingsPage_Loaded;
        }

        private async void CreateNewLeagueSettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            PopulatePlayDateComboBox();
            await PopulateScoringRule();
        }

        public CreateNewLeagueSettingsPage(LeagueSetting leagueSetting)
        {
            InitializeComponent();

            _settingService = App.ServiceProvider.GetRequiredService<ILeagueSettingService>();
            _ruleService = App.ServiceProvider.GetRequiredService<IScoringRuleService>();

            _leagueSetting = leagueSetting;

            Loaded += (sender, e) => CreateNewLeagueSettingsPage_Loaded(sender, e, leagueSetting.PlayDate, leagueSetting.ScoringRuleId, leagueSetting.TeamSize);
        }

        private async void CreateNewLeagueSettingsPage_Loaded(object sender, RoutedEventArgs e, string playDate, int ruleId, int teamSize)
        {
            SettingNameTextBox.Text = _leagueSetting?.LeagueSettingsName;
            PopulatePlayDateComboBox(playDate);
            await PopulateScoringRule(ruleId);
            PopulateTeamSize(teamSize);
        }


        private void PopulatePlayDateComboBox()
        {
            List<DayOfWeek> daysOfWeek = new List<DayOfWeek>() { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday };

            DayOfWeekComboBox.ItemsSource = daysOfWeek;
        }

        private void PopulatePlayDateComboBox(string selectedDayString)
        {
            List<DayOfWeek> daysOfWeek = new List<DayOfWeek>() { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday };

            DayOfWeekComboBox.ItemsSource = daysOfWeek;

            // Select previously selected day or default to first item
            if (Enum.TryParse(selectedDayString, out DayOfWeek selectedDay))
            {
                DayOfWeekComboBox.SelectedItem = selectedDay;
            }
            else
            {
                DayOfWeekComboBox.SelectedIndex = 0;
            }
        }

        private async Task PopulateScoringRule()
        {
            var scoringRules = await _ruleService.GetAll();

            if (!scoringRules.Any())
            {
                DisplayError("There are no saved scoring rules");
            }

            ScoringRuleComboBox.ItemsSource = scoringRules;
            ScoringRuleComboBox.DisplayMemberPath = "RuleName";
        }

        private async Task PopulateScoringRule(int scoringRuleId)
        {
            var scoringRules = await _ruleService.GetAll();

            if (!scoringRules.Any())
            {
                DisplayError("There are no saved scoring rules");
                return;
            }

            ScoringRuleComboBox.ItemsSource = scoringRules;
            ScoringRuleComboBox.DisplayMemberPath = "RuleName";

            // Grab selected scoring rule
            var selectedRule = await _ruleService.GetById(scoringRuleId);
            if (selectedRule != null)
            {
                ScoringRuleComboBox.SelectedItem = selectedRule;
            }
            else
            {
                ScoringRuleComboBox.SelectedIndex = 0;
            }

        }

        private void PopulateTeamSize(int teamSize)
        {
            TeamSizeTextBox.Text = teamSize.ToString();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessageTextBlock.Text = String.Empty;
            ErrorMessageTextBlock.Visibility = Visibility.Collapsed;

            // Check if DayOfWeek has value
            if (DayOfWeekComboBox.SelectedIndex == -1)
            {
                DisplayError("Please select a play date.");
                return;
            }

            // Check if ScoringId has value
            if (ScoringRuleComboBox.SelectedIndex == -1)
            {
                DisplayError("Please select a scoring rule.");
                return;
            }

            // Check Team Size
            int teamSize;
            try
            {
                teamSize = Convert.ToInt16(TeamSizeTextBox.Text);
            }
            catch
            {
                DisplayError("Team size must be a number.");
                return;
            }

            if (teamSize <= 0)
            {
                DisplayError("Team size must be larger than 0");
                return;
            }

            // Grab Scoring Rule
            var scoringRule = ScoringRuleComboBox.SelectedItem as ScoringRule;
            if (scoringRule == null)
            {
                DisplayError("Select a scoring rule.");
                return;
            }

            // Grab Day of Week
            DayOfWeek playDate;
            try
            {
                playDate = (DayOfWeek)DayOfWeekComboBox.SelectedItem;
            } catch
            {
                DisplayError("Select a play date.");
                return;
            }

            var request = new CreateLeagueSettingRequest
            {
                LeagueSettingsName = SettingNameTextBox.Text.Trim(),
                PlayDate = playDate.ToString(),
                ScoringRuleId = scoringRule.ScoringRuleId,
                TeamSize = teamSize
            };

            // Create new League Setting or update previous
            CreateLeagueSettingResult result;
            if (_leagueSetting == null)
            {
                result = await _settingService.CreateLeagueSetting(request);
            }
            else
            {
                request.LeagueSettingsId = _leagueSetting.LeagueSettingsId;

                result = await _settingService.UpdateLeagueSetting(request);
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

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_leagueSetting == null) 
            {
                DisplayError("There is no selected league setting.");
                return;
            }

            var result = await _settingService.DeleteLeagueSetting(_leagueSetting.LeagueSettingsId);
            if (result.IsSuccess)
            {
                Navigate?.Invoke(sender, e);
            } else
            {
                DisplayError(result.ErrorMessage);
            }
        }

        private void ClearForm()
        {
            SettingNameTextBox.Clear();
            TeamSizeTextBox.Clear();
            DayOfWeekComboBox.ItemsSource = null;
            ScoringRuleComboBox.ItemsSource = null;
        }

        private void DisplayError(string error)
        {
            ErrorMessageTextBlock.Text = error;
            ErrorMessageTextBlock.Visibility = Visibility.Visible;
        }

        private void DisplayErrors(CreateLeagueSettingResult result)
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
