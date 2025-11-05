namespace Core.DTOs.LeagueDTOs
{
    public class DeleteLeagueResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

        public static DeleteLeagueResult Success()
        {
            return new DeleteLeagueResult
            {
                IsSuccess = true,
            };
        }

        public static DeleteLeagueResult Failure(string error)
        {
            return new DeleteLeagueResult
            {
                IsSuccess = false,
                ErrorMessage = error
            };
        }
    }
}
