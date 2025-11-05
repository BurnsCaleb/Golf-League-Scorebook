using Core.Models;

namespace Core.DTOs.GolferDTOs
{
    public class CreateGolferResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> ValidationErrors { get; set; } = new List<string>();
        public Golfer Golfer { get; set; }  

        // Factory methods for easy creation
        public static CreateGolferResult Success(Golfer golfer)
        {
            return new CreateGolferResult
            {
                IsSuccess = true,
                Golfer = golfer
            };
        }

        public static CreateGolferResult Failure(string error)
        {
            return new CreateGolferResult
            {
                IsSuccess = false,
                ErrorMessage = error
            };
        }

        public static CreateGolferResult ValidationFailure(List<string> errors)
        {
            return new CreateGolferResult
            {
                IsSuccess = false,
                ErrorMessage = "Validation failed",
                ValidationErrors = errors
            };
        }
    }
}
