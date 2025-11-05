using Core.Models;

namespace Core.DTOs.MatchupDTOs
{
    public class EndMatchupResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> ValidationErrors { get; set; } = new List<string>();
        public Matchup Matchup { get; set; }

        public static EndMatchupResult Success(Matchup matchup)
        {
            return new EndMatchupResult
            {
                IsSuccess = true,
                Matchup = matchup,
            };
        }

        public static EndMatchupResult Failure(string error)
        {
            return new EndMatchupResult
            {
                IsSuccess = false,
                ErrorMessage = error
            };
        }

        public static EndMatchupResult ValidationFailure(List<string> errors)
        {
            return new EndMatchupResult
            {
                IsSuccess = false,
                ErrorMessage = "Validation failed",
                ValidationErrors = errors
            };
        }
    }
}
