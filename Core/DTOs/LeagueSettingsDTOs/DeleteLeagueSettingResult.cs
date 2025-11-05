namespace Core.DTOs.LeagueSettingsDTOs
{
    public class DeleteLeagueSettingResult
    {
        public bool IsSuccess { get; set; } = false;
        public string ErrorMessage { get; set; }

        public static DeleteLeagueSettingResult Success()
        {
            return new DeleteLeagueSettingResult
            {
                IsSuccess = true,
            };
        }

        public static DeleteLeagueSettingResult Failure(string message)
        {
            return new DeleteLeagueSettingResult
            {
                IsSuccess = false,
                ErrorMessage = message
            };
        }
    }
}
