using Core.Models;

namespace ViewModels
{
    public class CourseViewModel
    {

        private Course _course;
        public CourseViewModel(Course course)
        {
            _course = course;

        }

        public string CourseName => _course.CourseName;
        public string CourseLocation => _course.CourseLocation;
        private int _courseHoles => _course.NumHoles;
        public string CourseHoles => $"{_courseHoles} Holes";
        private int _coursePar => _course.CoursePar;
        public string CoursePar => $"Par {_coursePar}";
    }
}
