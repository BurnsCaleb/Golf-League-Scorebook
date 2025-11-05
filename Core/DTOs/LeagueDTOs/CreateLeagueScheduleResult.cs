using Core.Models;

namespace Core.DTOs.LeagueDTOs
{
    public class CreateLeagueScheduleResult
    {
        public bool IsSuccess { get; set; } = false;
        public string ErrorMessage { get; set; }

        public static CreateLeagueScheduleResult Success()
        {
            return new CreateLeagueScheduleResult
            {
                IsSuccess = true
            };
        }

        public static CreateLeagueScheduleResult Failure(string error)
        {
            return new CreateLeagueScheduleResult
            {
                IsSuccess = false,
                ErrorMessage = error
            };
        }
    }
}
