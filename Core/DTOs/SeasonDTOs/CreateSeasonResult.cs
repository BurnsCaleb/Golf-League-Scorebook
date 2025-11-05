using Core.Models;

namespace Core.DTOs.SeasonDTOs
{
    public class CreateSeasonResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public Season Season { get; set; }

        public static CreateSeasonResult Success(Season season)
        {
            return new CreateSeasonResult
            {
                IsSuccess = true,
                Season = season,
            };
        }

        public static CreateSeasonResult Failure(string error)
        {
            return new CreateSeasonResult
            {
                IsSuccess = false,
                ErrorMessage = error
            };
        }
    }
}
