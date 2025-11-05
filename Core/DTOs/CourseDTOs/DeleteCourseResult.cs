namespace Core.DTOs.CourseDTOs
{
    public class DeleteCourseResult
    {
        public bool IsSuccess { get; set; } = false;
        public string ErrorMessage { get; set; }

        public static DeleteCourseResult Success()
        {
            return new DeleteCourseResult
            {
                IsSuccess = true,
            };
        }

        public static DeleteCourseResult Failure(string message)
        {
            return new DeleteCourseResult
            {
                IsSuccess = false,
                ErrorMessage = message
            };
        }
    }
}
