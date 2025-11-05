namespace Core.DTOs.TeamDTOs
{
    public class DeleteTeamResult
    {
        public bool IsSuccess { get; set; } = false;
        public string ErrorMessage { get; set; }

        public static DeleteTeamResult Success()
        {
            return new DeleteTeamResult
            {
                IsSuccess = true,
            };
        }

        public static DeleteTeamResult Failure(string message)
        {
            return new DeleteTeamResult
            {
                IsSuccess = false,
                ErrorMessage = message
            };
        }
    }
}
