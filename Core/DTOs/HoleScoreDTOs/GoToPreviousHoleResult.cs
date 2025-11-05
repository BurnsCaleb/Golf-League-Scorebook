using Core.DTOs.HoleDTOs;

namespace Core.DTOs.HoleScoreDTOs
{
    public class GoToPreviousHoleResult
    {
        public GolferHoleScore HoleScoreData { get; set; }
        public bool HasData { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

        public static GoToPreviousHoleResult ExistingData(GolferHoleScore scoreData)
        {
            return new GoToPreviousHoleResult
            {
                HoleScoreData = scoreData,
                HasData = true,
                IsSuccess = true
            };
        }

        public static GoToPreviousHoleResult NewData(GolferHoleScore scoreData)
        {
            return new GoToPreviousHoleResult
            {
                HoleScoreData = scoreData,
                HasData = false,
                IsSuccess = true,
            };
        }

        public static GoToPreviousHoleResult Failure(string message)
        {
            return new GoToPreviousHoleResult
            {
                HasData = false,
                ErrorMessage = message,
                IsSuccess = false
            };
        }
    }
}
