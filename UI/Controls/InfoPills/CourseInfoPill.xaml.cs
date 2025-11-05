#nullable disable

using Core.Models;
using InterfaceControls.Events;
using System.Windows.Controls;
using ViewModels;

namespace UI.Controls.InfoPills
{
    /// <summary>
    /// Interaction logic for CourseInfoPill.xaml
    /// </summary>
    public partial class CourseInfoPill : UserControl
    {
        public event EventHandler<EditCourseEvent> Navigate;

        private Course _course;

        public CourseInfoPill(Course course)
        {
            InitializeComponent();

            _course = course;
            
            this.DataContext = new CourseViewModel(course);
        }

        private void CourseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Navigate?.Invoke(this, new EditCourseEvent(_course));
        }
    }
}
