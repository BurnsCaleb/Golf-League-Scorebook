#nullable disable

using Core.Interfaces.Service;
using Core.Models;
using InterfaceControls.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using UI.Controls;
using UI.Controls.InfoPills;

namespace UI.Pages.Overviews
{
    /// <summary>
    /// Interaction logic for CoursesOverviewPage.xaml
    /// </summary>
    public partial class CoursesOverviewPage : Page
    {
        public event EventHandler<EditCourseEvent> Navigate;
        public event RoutedEventHandler NewObject;

        private readonly ICourseService _courseService;

        public CoursesOverviewPage()
        {
            InitializeComponent();

            _courseService = App.ServiceProvider.GetRequiredService<ICourseService>();
           
            Loaded += CoursesOverviewPage_Loaded;
        }

        private async void CoursesOverviewPage_Loaded(object sender, RoutedEventArgs e)
        {
            await PopulateView();
        }

        private async Task PopulateView()
        {
            // Grab all courses
            var courses = await _courseService.GetAll();

            if (courses != null)
            {
                // Create Course Pills
                foreach (Course course in courses)
                {
                    CourseInfoPill courseInfo = new CourseInfoPill(course);

                    // Subscribe to event
                    courseInfo.Navigate += (sender, e) => Navigate?.Invoke(this, e);

                    CoursePillStackPanel.Children.Add(courseInfo);
                }
            }

            // Add Create New Course Pill
            string text = "Create New Course";
            CreateNewPill createNewCoursePill = new CreateNewPill(text);
            CoursePillStackPanel.Children.Add(createNewCoursePill);

            createNewCoursePill.NewObject += (sender, e) => NewObject?.Invoke(sender, e);
        }
    }
}
