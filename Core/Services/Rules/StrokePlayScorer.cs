using Core.DTOs.ScorerDTOs;
using Core.DTOs.HoleDTOs;
using Core.Interfaces.Service;

namespace Core.Services.Rules
{
    public class StrokePlayScorer : IScoringCalculator
    {
        public PointsAwarded[] CalculatePoints(List<GolferHoleScore> scoreData)
        {
            // Calculate Totals for each Golfer
            List<(int grossScore, int golferId)> grossSums = new List<(int, int)>();

            var groupedData = scoreData.GroupBy(d => d.GolferId);

            foreach (var group in groupedData)  // Every Golfer by ID
            {
                int grossTotal = 0;
                foreach(var score in group)     // Every scoreData by Golfer
                {
                    grossTotal += score.GrossScore;
                }

                grossSums.Add((grossTotal, group.First().GolferId));
            }

            // Order by lowest score
            var orderedData = grossSums.OrderBy(d => d.grossScore).ToList();

            PointsAwarded[] golferPoints = new PointsAwarded[orderedData.Count];

            // Award Points
            int points = 0;
            int? currentScore = 0;
            int count = 0;

            for (int i = orderedData.Count - 1; i >= 0; i--)
            {
                // Check if score is a tie
                if (orderedData[i].grossScore != currentScore)
                {
                    // Update Current Score
                    currentScore = orderedData[i].grossScore;

                    // Increment points
                    points++;
                }

                // Create PointsAwarded Object and Append to list
                golferPoints[count] = new PointsAwarded { id = orderedData[i].golferId, points = points, isGolfer = true };

                count++;
            }

            return golferPoints;
        }
    }
}
