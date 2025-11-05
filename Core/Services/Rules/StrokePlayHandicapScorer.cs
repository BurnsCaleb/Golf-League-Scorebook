using Core.DTOs.ScorerDTOs;
using Core.DTOs.HoleDTOs;
using Core.Interfaces.Service;

namespace Core.Services.Rules
{
    public class StrokePlayHandicapScorer : IScoringCalculator
    {
        public PointsAwarded[] CalculatePoints(List<GolferHoleScore> scoreData)
        {
            // Calculate Totals for each Golfer
            List<(int netScore, int golferId)> netSums = new List<(int, int)>();

            var groupedData = scoreData.GroupBy(d => d.GolferId);

            foreach (var group in groupedData)  // Every Golfer by ID
            {
                int netTotal = 0;
                foreach (var score in group)     // Every scoreData by Golfer
                {
                    netTotal += score.NetScore;
                }

                netSums.Add((netTotal, group.First().GolferId));
            }

            // Order by lowest score
            var orderedData = netSums.OrderBy(d => d.netScore).ToList();

            PointsAwarded[] golferPoints = new PointsAwarded[orderedData.Count];

            // Award Points
            int points = 0;
            int? currentScore = 0;
            int count = 0;

            for (int i = orderedData.Count - 1; i >= 0; i--)
            {
                // Check if score is a tie
                if (orderedData[i].netScore != currentScore)
                {
                    // Update Current Score
                    currentScore = orderedData[i].netScore;

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
