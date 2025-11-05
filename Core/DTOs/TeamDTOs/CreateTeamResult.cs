using Core.Models;

namespace Core.DTOs.TeamDTOs
{
    public class CreateTeamResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> ValidationErrors { get; set; } = new List<string>();
        public Team Team { get; set; }

        public static CreateTeamResult Success(Team team)
        {
            return new CreateTeamResult
            {
                IsSuccess = true,
                Team = team,
            };
        }

        public static CreateTeamResult Failure(string error)
        {
            return new CreateTeamResult
            {
                IsSuccess = false,
                ErrorMessage = error
            };
        }

        public static CreateTeamResult ValidationFailure(List<string> errors)
        {
            return new CreateTeamResult
            {
                IsSuccess = false,
                ErrorMessage = "Validation failed",
                ValidationErrors = errors
            };
        }
    }
}
