namespace Core.DTOs.HoleScoreDTOs
{
    public class CreateHoleScoreResult
    {
        public bool IsSuccess { get; set; } = false;
        public string ErrorMessage { get; set; }
        public List<string> ValidationErrors { get; set; } = new List<string>();

        // Factory methods for easy creation
        public static CreateHoleScoreResult Success()
        {
            return new CreateHoleScoreResult
            {
                IsSuccess = true,
            };
        }

        public static CreateHoleScoreResult Failure(string error)
        {
            return new CreateHoleScoreResult
            {
                ErrorMessage = error
            };
        }
    }
}
