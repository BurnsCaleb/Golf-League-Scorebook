using Core.Models;

namespace Core.DTOs.LeagueDTOs
{
    public class CreateLeagueResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> ValidationErrors { get; set; } = new List<string>();
        public League League { get; set; }

        public static CreateLeagueResult Success(League league)
        {
            return new CreateLeagueResult
            {
                IsSuccess = true,
                League = league
            };
        }

        public static CreateLeagueResult Failure(string error)
        {
            return new CreateLeagueResult
            {
                IsSuccess = false,
                ErrorMessage = error
            };
        }

        public static CreateLeagueResult ValidationFailure(List<string> errors)
        {
            return new CreateLeagueResult
            {
                IsSuccess = false,
                ErrorMessage = "Validation failed",
                ValidationErrors = errors
            };
        }
    }
}
