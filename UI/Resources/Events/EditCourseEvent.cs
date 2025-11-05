using Core.Models;

namespace InterfaceControls.Events
{
    public class EditCourseEvent : EventArgs
    {
        public Course Course { get; set; }

        public EditCourseEvent(Course course)
        {
            Course = course;
        }
    }
}
