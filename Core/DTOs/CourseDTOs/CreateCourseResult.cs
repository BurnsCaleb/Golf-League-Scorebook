using Core.Models;

namespace Core.DTOs.CourseDTOs
{
    public class CreateCourseResult
    {
        public bool IsSuccess { get; set; } = false;
        public bool IsWarning { get; set; } = false;
        public bool KnownWarning { get; set; } = false;
        public string ErrorMessage { get; set; }
        public List<string> ValidationErrors { get; set; } = new List<string>();
        public Course Course { get; set; }

        // Factory methods for easy creation
        public static CreateCourseResult Success(Course course)
        {
            return new CreateCourseResult
            {
                IsSuccess = true,
                Course = course
            };
        }

        public static CreateCourseResult InitialSuccess()
        {
            return new CreateCourseResult
            {
                IsSuccess = true,
            };
        }

        public static CreateCourseResult Warning(string warning)
        {
            return new CreateCourseResult
            {
                IsWarning = true,
                ErrorMessage = warning,
                KnownWarning = true
            };
        }

        public static CreateCourseResult Failure(string error)
        {
            return new CreateCourseResult
            {
                ErrorMessage = error
            };
        }

        public static CreateCourseResult ValidationFailure(List<string> errors)
        {
            return new CreateCourseResult
            {
                ErrorMessage = "Validation failed",
                ValidationErrors = errors
            };
        }
    }
}
