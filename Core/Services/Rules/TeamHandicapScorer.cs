using Core.DTOs.ScorerDTOs;
using Core.DTOs.HoleDTOs;
using Core.Interfaces.Service;

namespace Core.Services.Rules
{
    public class TeamHandicapScorer : IScoringCalculator
    {
        public PointsAwarded[] CalculatePoints(List<GolferHoleScore> scoreData)
        {

            // Group scoreData
            var sortedData = scoreData.GroupBy(d => d.TeamId).ToList();

            // Separate Teams
            var team1 = sortedData[0].ToList();
            var team2 = sortedData[1].ToList();

            // Group golfers in their teams
            var sortedTeam1 = team1.GroupBy(d => d.GolferId).ToList();
            var sortedTeam2 = team2.GroupBy(d => d.GolferId).ToList();



            // Separate golfers from team
            var golfer1a = sortedTeam1[0].ToList();
            var golfer1b = sortedTeam1[1].ToList();

            var golfer2a = sortedTeam2[0].ToList();
            var golfer2b = sortedTeam2[1].ToList();

            // Total netScores
            var golfer1a_netTotal = golfer1a
                .Select(g => g.NetScore)
                .ToList()
                .Sum();

            var golfer1b_netTotal = golfer1b
                .Select(g => g.NetScore)
                .ToList()
                .Sum();

            var golfer2a_netTotal = golfer2a
                .Select(g => g.NetScore)
                .ToList()
                .Sum();

            var golfer2b_netTotal = golfer2b
                .Select(g => g.NetScore)
                .ToList()
                .Sum();

            var team1_netTotal = golfer1a_netTotal + golfer1b_netTotal;
            var team2_netTotal = golfer2a_netTotal + golfer2b_netTotal;

            // Create PointsAwarded variables
            PointsAwarded golfer1a_points = new PointsAwarded { id = golfer1a.First().GolferId, points = 0, isGolfer = true };
            PointsAwarded golfer1b_points = new PointsAwarded { id = golfer1b.First().GolferId, points = 0, isGolfer = true };
            PointsAwarded golfer2a_points = new PointsAwarded { id = golfer2a.First().GolferId, points = 0, isGolfer = true };
            PointsAwarded golfer2b_points = new PointsAwarded { id = golfer2b.First().GolferId, points = 0, isGolfer = true };
            PointsAwarded team1_points = new PointsAwarded { id = golfer1a.First().TeamId, points = 0, isGolfer = false };
            PointsAwarded team2_points = new PointsAwarded { id = golfer2a.First().TeamId, points = 0, isGolfer = false };

            // Compare Individual Scores
            CompareScores(golfer1a_netTotal, golfer2a_netTotal, ref golfer1a_points, ref golfer2a_points);
            CompareScores(golfer1a_netTotal, golfer2b_netTotal, ref golfer1a_points, ref golfer2b_points);

            CompareScores(golfer1b_netTotal, golfer2a_netTotal, ref golfer1b_points, ref golfer2a_points);
            CompareScores(golfer1b_netTotal, golfer2b_netTotal, ref golfer1b_points, ref golfer2b_points);

            // Compare Team Scores
            CompareScores(team1_netTotal, team2_netTotal, ref team1_points, ref team2_points);


            // Prepare return variable
            PointsAwarded[] pointsAwarded = { golfer1a_points, golfer1b_points, golfer2a_points, golfer2b_points, team1_points, team2_points };

            return pointsAwarded;
        }

        private void CompareScores(int? scoreA, int? scoreB, ref PointsAwarded pointsA, ref PointsAwarded pointsB)
        {
            // Compare scores and update reference variables.
            if (scoreA < scoreB)
            {
                pointsA.points += 2;
            }
            else if (scoreA == scoreB)
            {
                pointsA.points += 1;
                pointsB.points += 1;
            }
            else if (scoreA > scoreB)
            {
                pointsB.points += 2;
            }
        }
    }
}
