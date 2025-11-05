using Core.DTOs.CourseDTOs;
using Core.DTOs.HoleDTOs;
using Core.Interfaces.Service;
using Core.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using ViewModels;

namespace UI.Pages.Forms
{

    /// <summary>
    /// Interaction logic for CreateCoursePage.xaml
    /// </summary>
    public partial class CreateCoursePage : Page
    {
        private readonly ICourseService _courseService;
        private readonly IHoleService _holeService;
        private CreateCourseRequest _courseRequest;

        private Course? _course;
        private bool knownWarning = false;

        public event RoutedEventHandler? SaveCourseEvent;

        public CreateCoursePage()
        {
            InitializeComponent();

            _courseService = App.ServiceProvider.GetRequiredService<ICourseService>();
            _holeService = App.ServiceProvider.GetRequiredService<IHoleService>();

            InitializeCourseForm();
        }

        public CreateCoursePage(Course course)
        {
            InitializeComponent();

            _course = course;

            _courseService = App.ServiceProvider.GetRequiredService<ICourseService>();
            _holeService = App.ServiceProvider.GetRequiredService<IHoleService>();

            PopulateCourse(_course);
            InitializeCourseOverview();
        }

        #region Course Related Functions

        private void InitializeCourseForm()
        {
            NumHolesTextBlock.Text = "18";

            CourseErrorMessageTextBlock.Visibility = Visibility.Collapsed;
            CourseErrorMessageTextBlock.Text = String.Empty;

            CourseWarningMessageTextBlock.Visibility = Visibility.Collapsed;
            CourseWarningMessageTextBlock.Text = String.Empty;

            ShowCourseForm();
        }

        private void PopulateCourse(Course course)
        {
            // Initialize UI Form
            NameTextBox.Text = course.CourseName;
            LocationTextBox.Text = course.CourseLocation;
            NumHolesTextbox.Text = course.NumHoles.ToString();
            RatingTextbox.Text = course.CourseRating.ToString();
            SlopeTextbox.Text = course.CourseSlope.ToString();

            // Initialize CourseRequest
            _courseRequest = new CreateCourseRequest
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                CourseLocation = course.CourseLocation,
                CourseRating = course.CourseRating,
                CourseSlope = course.CourseSlope,
                NumHoles = course.NumHoles,
                CourseHoles = (List<Hole>)course.Holes,
            };
           
        }

        private void ShowCourseForm()
        {
            HoleFormGrid.Visibility = Visibility.Hidden;
            CourseOverviewGrid.Visibility = Visibility.Hidden;

            CourseFormGrid.Visibility = Visibility.Visible;
        }

        private void NumHolesTextbox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (NumHolesTextbox.Text == "18") NumHolesTextbox.Text = String.Empty;
        }

        private void NumHolesTextbox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (NumHolesTextbox.Text == String.Empty) NumHolesTextbox.Text = "18";
        }

        private async void AddHolesButton_Click(object sender, RoutedEventArgs e)
        {
            // Convert values
            int numHoles;
            double slope, rating;
            try
            {
                slope = Convert.ToDouble(SlopeTextbox.Text);
                rating = Convert.ToDouble(RatingTextbox.Text);
                numHoles = Convert.ToInt16(NumHolesTextbox.Text);
            }
            catch
            {
                CourseDisplayError("Course Slope, Rating, and Number of Holes must be numeric values");
                return;
            }

            // Create Initial Request
            var initialRequest = new CreateCourseRequest
            {
                CourseName = NameTextBox.Text.Trim(),
                CourseLocation = LocationTextBox.Text.Trim(),
                CourseSlope = slope,
                CourseRating = rating,
                NumHoles = numHoles,
                KnownWarning = knownWarning,
            };

            var result = await _courseService.InitialCheck(initialRequest);

            if (result.IsSuccess)
            {
                knownWarning = false;
                _courseRequest = initialRequest;
                await InitializeHoleForm();
            }
            else if (result.IsWarning)
            {
                knownWarning = result.KnownWarning;
                CourseDisplayWarning(result);
            }
            else
            {
                knownWarning = result.KnownWarning;
                CourseDisplayErrors(result);
            }
        }

        private void CourseDisplayWarning(CreateCourseResult result)
        {
            if (result.ValidationErrors.Any())
            {
                CourseWarningMessageTextBlock.Text = String.Join(", ", result.ValidationErrors);
                CourseWarningMessageTextBlock.Visibility = Visibility.Visible;
            }
            else if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                CourseWarningMessageTextBlock.Text = result.ErrorMessage;
                CourseWarningMessageTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void CourseDisplayError(string error)
        {

            CourseErrorMessageTextBlock.Text = error;
            CourseErrorMessageTextBlock.Visibility = Visibility.Visible;
        }

        private void CourseDisplayErrors(CreateCourseResult result)
        {
            if (result.ValidationErrors.Any())
            {
                CourseErrorMessageTextBlock.Text = String.Join(", ", result.ValidationErrors);
                CourseErrorMessageTextBlock.Visibility = Visibility.Visible;
            }
            else if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                CourseErrorMessageTextBlock.Text = result.ErrorMessage;
                CourseErrorMessageTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void HoleDisplayWarning(CreateCourseResult result)
        {
            CourseWarningMessageTextBlock.Text = result.ErrorMessage;
            CourseWarningMessageTextBlock.Visibility = Visibility.Visible;
        }

        #endregion

        #region Hole Related Functions

        private async Task InitializeHoleForm()
        {
            HoleErrorMessageTextBlock.Visibility = Visibility.Collapsed;
            HoleErrorMessageTextBlock.Text = String.Empty;

            HoleWarningMessageTextBlock.Visibility = Visibility.Collapsed;
            HoleWarningMessageTextBlock.Text = String.Empty;

            // Populate Course Information
            CourseNameTextBlock.Text = _courseRequest.CourseName;
            CourseLocationTextBlock.Text = _courseRequest.CourseLocation;
            NumHolesTextBlock.Text = $"{_courseRequest.NumHoles} Holes";

            // Always start on hole 1
            int startingIndex = 0;
            await PopulateHoleEntry(startingIndex);

            ShowHoleForm();
        }

        private async Task PopulateHoleEntry(int index)
        {
            Hole? currentHole;

            try
            {
                currentHole = _courseRequest.CourseHoles[index];
            } catch (ArgumentOutOfRangeException)
            {
                currentHole = null;
            }
                

            int holeNum = index + 1;

            // Check if there are no more holes
            if (holeNum > _courseRequest.NumHoles)
            {
                // Final Course Validation Check
                await CourseValidation();
                return;
            }

            if (currentHole == null)    // Display Empty Form
            {
                HoleNumTextBlock.Text = $"Hole {holeNum}";

                ParTextBox.Clear();
                DistanceTextBox.Clear();
                HandicapTextBox.Clear();
            }
            else   // Display Created Hole Info
            {
                HoleNumTextBlock.Text = $"Hole {currentHole.HoleNum.ToString()}";
                ParTextBox.Text = currentHole.Par.ToString();
                DistanceTextBox.Text = currentHole.Distance.ToString();
                HandicapTextBox.Text = currentHole.Handicap.ToString();
            }

            ShowHoleForm();
        }

        private void ShowHoleForm()
        {
            CourseFormGrid.Visibility = Visibility.Hidden;
            CourseOverviewGrid.Visibility = Visibility.Hidden;

            HoleFormGrid.Visibility = Visibility.Visible;
        }

        private void DistanceTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Grab numbers
            if (DistanceTextBox.Text != string.Empty)
            {
                DistanceTextBox.Text = Regex.Match(DistanceTextBox.Text, @"\d+").ToString();
            }
        }

        private void DistanceTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Add yds
            DistanceTextBox.Text.Trim();
            DistanceTextBox.Text += " yds";
        }

        private async void NextHoleButton_Click(object sender, RoutedEventArgs e)
        {
            // Reset error and warning messages
            HoleErrorMessageTextBlock.Visibility = Visibility.Collapsed;
            HoleWarningMessageTextBlock.Visibility = Visibility.Collapsed;

            int holeNum, par, distance, handicap;

            // Remove "Hole" from hole num
            // Remove "yds" from distance textbox
            string pattern = @"\d+";

            string holeNumbers = Regex.Match(HoleNumTextBlock.Text, pattern).Value;
            string distanceNumbers = Regex.Match(DistanceTextBox.Text, pattern).Value;

            // Grab Values
            try
            {
                holeNum = Convert.ToInt16(holeNumbers);
                par = Convert.ToInt16(ParTextBox.Text);
                distance = Convert.ToInt16(distanceNumbers);
                handicap = Convert.ToInt16(HandicapTextBox.Text);
            }
            catch
            {
                HoleDisplayError("All values must be numeric.");
                return;
            }

            // Make CreateHoleRequest
            var request = new CreateHoleRequest
            {
                HoleNum = holeNum,
                Par = par,
                Distance = distance,
                Handicap = handicap,
                KnownWarning = knownWarning,
            };

            // Create new hole or override existing hole

            // Call Service
            var response = _holeService.CheckHole(request);

            // Handle Response
            if (response.IsSuccess)
            {
                
                try
                {
                    // Grab hole to update
                    var hole = _courseRequest.CourseHoles[holeNum - 1];
                    hole.HoleNum = request.HoleNum;
                    hole.Par = request.Par;
                    hole.Distance = distance;
                    hole.Handicap = handicap;
                } catch (ArgumentOutOfRangeException)
                {
                    // Save Data
                    _courseRequest.CourseHoles.Add(new Hole
                    {
                        HoleNum = request.HoleNum,
                        Par = request.Par,
                        Distance = distance,
                        Handicap = handicap
                    });
                }

                knownWarning = false;

                // Go to next hole
                await PopulateHoleEntry(holeNum); // Hole Number of current hole is index of next hole
            }
            else if (response.IsWarning)
            {
                // Display warning and allow user to reenter information
                knownWarning = true;
                HoleDisplayWarning(response);
            }
            else
            {
                HoleDisplayErrors(response);
                return;
            }
        }

        private async void PrevHoleButton_Click(object sender, RoutedEventArgs e)
        {
            string pattern = @"\d+";

            string holeNumbers = Regex.Match(HoleNumTextBlock.Text, pattern).Value;

            // Get hole num from textblock
            int holeNum = Convert.ToInt16(holeNumbers);

            if (holeNum > 1)
            {
                await PopulateHoleEntry(holeNum - 2);
            }
            else
            {
                ShowCourseForm();
            }
        }

        private async Task CourseValidation()
        {
            CreateCourseResult result;
            if (_course == null)
            {
                // Call Service
                result = await _courseService.CreateCourse(_courseRequest);
            } else
            {
                result = await _courseService.UpdateCourse(_courseRequest);
            }

            // Handle Responses
            if (result.IsSuccess)
            {
                _course = result.Course;

                // Go to CourseOverview Page
                InitializeCourseOverview();
            }
            else if (result.IsWarning)
            {
                // Display Warning message
                HoleDisplayWarning(result);
            }
            else
            {
                HoleDisplayErrors(result);
            }
        }

        private void HoleDisplayError(string error)
        {
            HoleErrorMessageTextBlock.Text = error;
            HoleErrorMessageTextBlock.Visibility = Visibility.Visible;
        }

        private void HoleDisplayErrors(CreateHoleResult result)
        {
            if (result.ValidationErrors.Any())
            {
                HoleErrorMessageTextBlock.Text = String.Join(", ", result.ValidationErrors);
                HoleErrorMessageTextBlock.Visibility = Visibility.Visible;
            }
            else if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                HoleErrorMessageTextBlock.Text = result.ErrorMessage;
                HoleErrorMessageTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void HoleDisplayErrors(CreateCourseResult result)
        {
            if (result.ValidationErrors.Any())
            {
                HoleErrorMessageTextBlock.Text = String.Join(", ", result.ValidationErrors);
                HoleErrorMessageTextBlock.Visibility = Visibility.Visible;
            }
            else if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                HoleErrorMessageTextBlock.Text = result.ErrorMessage;
                HoleErrorMessageTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void HoleDisplayWarning(CreateHoleResult result)
        {
            HoleWarningMessageTextBlock.Text = result.ErrorMessage;
            HoleWarningMessageTextBlock.Visibility = Visibility.Visible;
        }

        #endregion

        #region Overview Related Functions

        private void InitializeCourseOverview()
        {
            OverviewErrorMessageTextBlock.Text = String.Empty;
            OverviewErrorMessageTextBlock.Visibility = Visibility.Collapsed;

            if (_course == null)
            {
                DisplayError("Course was not found.");
                return;
            }

            // Populate DataGrid
            var holeList = new HoleViewModel[_course.NumHoles];
            foreach (Hole hole in _course.Holes)
            {
                holeList[hole.HoleNum - 1] = new HoleViewModel(hole);
            }

            HoleDataGrid.ItemsSource = holeList;


            // Populate Course Info
            OverviewCourseNameTextBox.Text = _course.CourseName;
            OverviewCourseLocationTextBox.Text = _course.CourseLocation;
            OverviewNumHolesTextBox.Text = _course.NumHoles.ToString();

            ShowCourseOverview();
        }

        private void ShowCourseOverview()
        {
            HoleFormGrid.Visibility = Visibility.Hidden;
            CourseFormGrid.Visibility = Visibility.Hidden;

            CourseOverviewGrid.Visibility = Visibility.Visible;
        }

        private async void HoleDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HoleDataGrid.SelectedItem is HoleViewModel selectedHole)
            {
                var holeNum = selectedHole.HoleNum;

                await PopulateHoleEntry(holeNum - 1);
            }
        }

        private void SaveCourse_Click(object sender, RoutedEventArgs e)
        {
            // Change to courses page
            SaveCourseEvent?.Invoke(sender, e);
        }

        private async void DeleteCourse_Click(object sender, RoutedEventArgs e)
        {
            if (_course != null)
            {
                var result = await _courseService.DeleteCourse(_course.CourseId);

                if (result.IsSuccess)
                {
                    SaveCourseEvent?.Invoke(sender, e);
                    return;
                } else
                {
                    DisplayError(result.ErrorMessage);
                }
            }

            SaveCourseEvent?.Invoke(sender, e);
        }

        private void DisplayError(string error)
        {
            OverviewErrorMessageTextBlock.Text = error;
            OverviewErrorMessageTextBlock.Visibility = Visibility.Visible;
        }

        #endregion


    }
}
