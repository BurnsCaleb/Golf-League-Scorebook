using Core.Models;

namespace Core.DTOs.LeagueSettingsDTOs
{
    public class CreateLeagueSettingResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> ValidationErrors { get; set; } = new List<string>();
        public LeagueSetting LeagueSetting { get; set; }

        public static CreateLeagueSettingResult Success(LeagueSetting leagueSetting)
        {
            return new CreateLeagueSettingResult
            {
                IsSuccess = true,
                LeagueSetting = leagueSetting
            };
        }

        public static CreateLeagueSettingResult Failure(string error)
        {
            return new CreateLeagueSettingResult
            {
                IsSuccess = false,
                ErrorMessage = error
            };
        }

        public static CreateLeagueSettingResult ValidationFailure(List<string> errors)
        {
            return new CreateLeagueSettingResult
            {
                IsSuccess = false,
                ErrorMessage = "Validation failed",
                ValidationErrors = errors
            };
        }
    }
}
