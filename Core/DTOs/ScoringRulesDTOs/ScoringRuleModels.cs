using Core.Models;

namespace Core.DTOs.ScoringRulesDTOs
{
    public static class ScoringRuleModels
    {
        public static ScoringRule TeamHandicap { get; } =
            new ScoringRule
            {
                ScoringRuleId = 1,
                RuleName = "Team Handicap",
                Description = "10 total points available between two teams. Each golfer compares their score to both golfers on the other team. For each comparison, the lower score receives two points. A tie results in one point each. The total score for each team is then compared with two points available."
            };

        public static ScoringRule StrokePlay { get; } =
            new ScoringRule
            {
                ScoringRuleId = 2,
                RuleName = "Stroke Play",
                Description = "Individual gross scores are compared. Last place gets 0 points. Every position after that gets 1 more point than the last."
            };

        public static ScoringRule StrokePlayHandicap { get; } =
            new ScoringRule
            {
                ScoringRuleId = 3,
                RuleName = "Stroke Play Handicap",
                Description = "Individual net scores are compared. Last place gets 0 points. Every position after that gets 1 more point than the last."
            };

        public static ScoringRule MatchPlay { get; } =
            new ScoringRule
            {
                ScoringRuleId = 4,
                RuleName = "Match Play",
                Description = "Golfer with the lowest gross score on each hole gets 1 point. In the event of a tie, each golfer in the tie receives half a point."
            };

        public static ScoringRule MatchPlayHandicap { get; } =
            new ScoringRule
            {
                ScoringRuleId = 5,
                RuleName = "Match Play Handicap",
                Description = "Golfer with the lowest net score on each hole gets 1 point. In the event of a tie, each golfer in the tie receives half a point."
            };
    }
}
