using Core.DTOs.ScorerDTOs;
using Core.DTOs.HoleDTOs;
using Core.Interfaces.Service;

namespace Core.Services.Rules
{
    public class MatchPlayScorer : IScoringCalculator
    {
        public PointsAwarded[] CalculatePoints(List<GolferHoleScore> scoreData)
        {
            // Group Golfers
            var groupedGolfers = scoreData.GroupBy(d => d.GolferId).ToList();
            int numGolfers = groupedGolfers.Count;

            PointsAwarded[] golferPoints = new PointsAwarded[numGolfers];

            // Populate golferPoints foreach golfer
            for (int i = 0; i < numGolfers; i++) 
            {
                golferPoints[i] = new PointsAwarded { id = groupedGolfers[i].First().GolferId, points = 0, isGolfer = true };
            }

            // sort by hole
            var groupedData = scoreData.GroupBy(s => s.HoleId);

            foreach (var hole in groupedData)       // For each hole
            {
                int lowestScore = -1;
                List<int?> golferIds = new List<int?>();

                foreach (var score in scoreData.Where(d => d.HoleId == hole.Key))    // For each score for this hole
                {
                    if (score.GrossScore < lowestScore || lowestScore == -1)
                    {
                        // Clear low golfer list
                        golferIds.Clear();

                        lowestScore = score.GrossScore;
                        golferIds.Add(score.GolferId);
                    }
                    else if (score.GrossScore == lowestScore)    // Tie on hole
                    {
                        // Add golfer Id to low score list
                        golferIds.Add(score.GolferId);
                    }
                }

                // 1 point to winner of hole. 
                // 1/2 point to tie
                if (golferIds.Count > 0)
                {
                    if (golferIds.Count > 1)
                    {
                        foreach (var id in golferIds)
                        {
                            golferPoints.Where(g => g.id == id).First().points += 0.5;
                        }

                    }

                    if (golferIds.Count == 1)
                    {
                        golferPoints.Where(g => g.id == golferIds[0]).First().points += 1;
                    }
                }

            }

            return golferPoints;
        }
    }
}
