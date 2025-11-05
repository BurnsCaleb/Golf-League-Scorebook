using Core.Models;

namespace Core.DTOs.CourseDTOs
{
    public class CreateCourseRequest
    {
        public int? CourseId { get; set; }
        public required string CourseName { get; set; }
        public required string CourseLocation { get; set; }
        public required double CourseRating { get; set; }
        public required double CourseSlope { get; set; }
        public int NumHoles { get; set; }
        public int CoursePar { get; set; }
        public List<Hole>? CourseHoles { get; set; } = new List<Hole>();

        public bool KnownWarning { get; set; } = false;
    }
}
