using Core.Models;

namespace Core.DTOs.GolferSearchBarDTOs
{
    public class CreateGolferSearchBarResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> ValidationErrors { get; set; } = new List<string>();
        public List<Golfer> Golfers { get; set; }

        public static CreateGolferSearchBarResult Success(List<Golfer> golfers)
        {
            return new CreateGolferSearchBarResult
            {
                IsSuccess = true,
                Golfers = golfers
            };
        }

        public static CreateGolferSearchBarResult Failure(string error)
        {
            return new CreateGolferSearchBarResult
            {
                IsSuccess = false,
                ErrorMessage = error
            };
        }
    }
}
