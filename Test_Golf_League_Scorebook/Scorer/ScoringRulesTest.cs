using Core.DTOs.HoleDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Services.Rules;

namespace Test_Golf_League_Scorebook.Scorer
{
    public class ScoringRulesTest
    {
        private readonly TeamHandicapScorer _teamHandicapScorer;
        private readonly StrokePlayScorer _strokePlayScorer;
        private readonly StrokePlayHandicapScorer _strokePlayHandicapScorer;
        private readonly MatchPlayScorer _matchPlayScorer;
        private readonly MatchPlayHandicapScorer _matchPlayHandicapScorer;

        public List<GolferHoleScore> scoreDataTie = new List<GolferHoleScore>
            {
                new GolferHoleScore{ GolferId = 1, GolferName = "Caleb Burns", TeamId = 1, HoleId = 1, HoleNumber = 1, HoleDistance = 350, HoleHandicap = 1, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 8.3 },
                new GolferHoleScore{ GolferId = 1, GolferName = "Caleb Burns", TeamId = 1, HoleId = 2, HoleNumber = 2, HoleDistance = 375, HoleHandicap = 2, HolePar = 4, GrossScore = 5, NetScore = 4, GolferHandicap = 8.3 },
                new GolferHoleScore{ GolferId = 2, GolferName = "AJ Burns", TeamId = 1, HoleId = 1, HoleNumber = 1, HoleDistance = 350, HoleHandicap = 1, HolePar = 4, GrossScore = 5, NetScore = 4, GolferHandicap = 14.1 },
                new GolferHoleScore{ GolferId = 2, GolferName = "AJ Burns", TeamId = 1, HoleId = 2, HoleNumber = 2, HoleDistance = 375, HoleHandicap = 2, HolePar = 4, GrossScore = 5, NetScore = 4, GolferHandicap = 14.1 },
                new GolferHoleScore{ GolferId = 3, GolferName = "Cole Thomas", TeamId = 2, HoleId = 1, HoleNumber = 1, HoleDistance = 350, HoleHandicap = 1, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 9.5 },
                new GolferHoleScore{ GolferId = 3, GolferName = "Cole Thomas", TeamId = 2, HoleId = 2, HoleNumber = 2, HoleDistance = 375, HoleHandicap = 2, HolePar = 4, GrossScore = 5, NetScore = 4, GolferHandicap = 9.5 },
                new GolferHoleScore{ GolferId = 4, GolferName = "Ethan Thomas", TeamId = 2, HoleId = 1, HoleNumber = 1, HoleDistance = 350, HoleHandicap = 1, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 4.5 },
                new GolferHoleScore{ GolferId = 4, GolferName = "Ethan Thomas", TeamId = 2, HoleId = 2, HoleNumber = 2, HoleDistance = 375, HoleHandicap = 2, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 4.5 },
            };

        public List<GolferHoleScore> scoreDataOutrightWinner = new List<GolferHoleScore>
            {
                    new GolferHoleScore{ GolferId = 1, GolferName = "Caleb Burns", TeamId = 1, HoleId = 1, HoleNumber = 1, HoleDistance = 350, HoleHandicap = 1, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 8.3 },
                    new GolferHoleScore{ GolferId = 1, GolferName = "Caleb Burns", TeamId = 1, HoleId = 2, HoleNumber = 2, HoleDistance = 375, HoleHandicap = 2, HolePar = 4, GrossScore = 5, NetScore = 4, GolferHandicap = 8.3 },
                    new GolferHoleScore{ GolferId = 2, GolferName = "AJ Burns", TeamId = 1, HoleId = 1, HoleNumber = 1, HoleDistance = 350, HoleHandicap = 1, HolePar = 4, GrossScore = 5, NetScore = 4, GolferHandicap = 14.1 },
                    new GolferHoleScore{ GolferId = 2, GolferName = "AJ Burns", TeamId = 1, HoleId = 2, HoleNumber = 2, HoleDistance = 375, HoleHandicap = 2, HolePar = 4, GrossScore = 5, NetScore = 4, GolferHandicap = 14.1 },
                    new GolferHoleScore{ GolferId = 3, GolferName = "Cole Thomas", TeamId = 2, HoleId = 1, HoleNumber = 1, HoleDistance = 350, HoleHandicap = 1, HolePar = 4, GrossScore = 6, NetScore = 5, GolferHandicap = 9.5 },
                    new GolferHoleScore{ GolferId = 3, GolferName = "Cole Thomas", TeamId = 2, HoleId = 2, HoleNumber = 2, HoleDistance = 375, HoleHandicap = 2, HolePar = 4, GrossScore = 6, NetScore = 5, GolferHandicap = 9.5 },
                    new GolferHoleScore{ GolferId = 4, GolferName = "Ethan Thomas", TeamId = 2, HoleId = 1, HoleNumber = 1, HoleDistance = 350, HoleHandicap = 1, HolePar = 4, GrossScore = 6, NetScore = 5, GolferHandicap = 4.5 },
                    new GolferHoleScore{ GolferId = 4, GolferName = "Ethan Thomas", TeamId = 2, HoleId = 2, HoleNumber = 2, HoleDistance = 375, HoleHandicap = 2, HolePar = 4, GrossScore = 6, NetScore = 5, GolferHandicap = 4.5 },
            };

        public List<GolferHoleScore> scoreDataPartialWinner = new List<GolferHoleScore>
            {
                    new GolferHoleScore{ GolferId = 1, GolferName = "Caleb Burns", TeamId = 1, HoleId = 1, HoleNumber = 1, HoleDistance = 350, HoleHandicap = 1, HolePar = 4, GrossScore = 6, NetScore = 5, GolferHandicap = 8.3 },
                    new GolferHoleScore{ GolferId = 1, GolferName = "Caleb Burns", TeamId = 1, HoleId = 2, HoleNumber = 2, HoleDistance = 375, HoleHandicap = 2, HolePar = 4, GrossScore = 5, NetScore = 4, GolferHandicap = 8.3 },
                    new GolferHoleScore{ GolferId = 2, GolferName = "AJ Burns", TeamId = 1, HoleId = 1, HoleNumber = 1, HoleDistance = 350, HoleHandicap = 1, HolePar = 4, GrossScore = 5, NetScore = 4, GolferHandicap = 14.1 },
                    new GolferHoleScore{ GolferId = 2, GolferName = "AJ Burns", TeamId = 1, HoleId = 2, HoleNumber = 2, HoleDistance = 375, HoleHandicap = 2, HolePar = 4, GrossScore = 5, NetScore = 4, GolferHandicap = 14.1 },
                    new GolferHoleScore{ GolferId = 3, GolferName = "Cole Thomas", TeamId = 2, HoleId = 1, HoleNumber = 1, HoleDistance = 350, HoleHandicap = 1, HolePar = 4, GrossScore = 5, NetScore = 5, GolferHandicap = 9.5 },
                    new GolferHoleScore{ GolferId = 3, GolferName = "Cole Thomas", TeamId = 2, HoleId = 2, HoleNumber = 2, HoleDistance = 375, HoleHandicap = 2, HolePar = 4, GrossScore = 5, NetScore = 4, GolferHandicap = 9.5 },
                    new GolferHoleScore{ GolferId = 4, GolferName = "Ethan Thomas", TeamId = 2, HoleId = 1, HoleNumber = 1, HoleDistance = 350, HoleHandicap = 1, HolePar = 4, GrossScore = 3, NetScore = 3, GolferHandicap = 4.5 },
                    new GolferHoleScore{ GolferId = 4, GolferName = "Ethan Thomas", TeamId = 2, HoleId = 2, HoleNumber = 2, HoleDistance = 375, HoleHandicap = 2, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 4.5 },
            };

        public List<GolferHoleScore> scoreDataIndividualWinner = new List<GolferHoleScore>
            {
                    new GolferHoleScore{ GolferId = 1, GolferName = "Caleb Burns", TeamId = 1, HoleId = 1, HoleNumber = 1, HoleDistance = 350, HoleHandicap = 1, HolePar = 4, GrossScore = 3, NetScore = 3, GolferHandicap = 8.3 },
                    new GolferHoleScore{ GolferId = 1, GolferName = "Caleb Burns", TeamId = 1, HoleId = 2, HoleNumber = 2, HoleDistance = 375, HoleHandicap = 2, HolePar = 4, GrossScore = 3, NetScore = 3, GolferHandicap = 8.3 },
                    new GolferHoleScore{ GolferId = 1, GolferName = "Caleb Burns", TeamId = 1, HoleId = 3, HoleNumber = 3, HoleDistance = 325, HoleHandicap = 3, HolePar = 4, GrossScore = 3, NetScore = 3, GolferHandicap = 8.3 },
                    new GolferHoleScore{ GolferId = 1, GolferName = "Caleb Burns", TeamId = 1, HoleId = 4, HoleNumber = 4, HoleDistance = 390, HoleHandicap = 4, HolePar = 4, GrossScore = 3, NetScore = 3, GolferHandicap = 8.3 },

                    new GolferHoleScore{ GolferId = 2, GolferName = "Ethan Thomas", TeamId = 2, HoleId = 1, HoleNumber = 1, HoleDistance = 350, HoleHandicap = 1, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 9.3 },
                    new GolferHoleScore{ GolferId = 2, GolferName = "Ethan Thomas", TeamId = 2, HoleId = 2, HoleNumber = 2, HoleDistance = 375, HoleHandicap = 2, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 9.3 },
                    new GolferHoleScore{ GolferId = 2, GolferName = "Ethan Thomas", TeamId = 2, HoleId = 3, HoleNumber = 3, HoleDistance = 325, HoleHandicap = 3, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 9.3 },
                    new GolferHoleScore{ GolferId = 2, GolferName = "Ethan Thomas", TeamId = 2, HoleId = 4, HoleNumber = 4, HoleDistance = 390, HoleHandicap = 4, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 9.3 },

                    new GolferHoleScore{ GolferId = 3, GolferName = "AJ Burns", TeamId = 3, HoleId = 1, HoleNumber = 1, HoleDistance = 350, HoleHandicap = 1, HolePar = 4, GrossScore = 5, NetScore = 5, GolferHandicap = 14.1 },
                    new GolferHoleScore{ GolferId = 3, GolferName = "AJ Burns", TeamId = 3, HoleId = 2, HoleNumber = 2, HoleDistance = 375, HoleHandicap = 2, HolePar = 4, GrossScore = 5, NetScore = 5, GolferHandicap = 14.1 },
                    new GolferHoleScore{ GolferId = 3, GolferName = "AJ Burns", TeamId = 3, HoleId = 3, HoleNumber = 3, HoleDistance = 325, HoleHandicap = 3, HolePar = 4, GrossScore = 5, NetScore = 5, GolferHandicap = 14.1 },
                    new GolferHoleScore{ GolferId = 3, GolferName = "AJ Burns", TeamId = 3, HoleId = 4, HoleNumber = 4, HoleDistance = 390, HoleHandicap = 4, HolePar = 4, GrossScore = 5, NetScore = 5, GolferHandicap = 14.1 },
            };

        public List<GolferHoleScore> scoreDataIndividualTie = new List<GolferHoleScore>
            {
                    new GolferHoleScore{ GolferId = 1, GolferName = "Caleb Burns", TeamId = 1, HoleId = 1, HoleNumber = 1, HoleDistance = 350, HoleHandicap = 1, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 8.3 },
                    new GolferHoleScore{ GolferId = 1, GolferName = "Caleb Burns", TeamId = 1, HoleId = 2, HoleNumber = 2, HoleDistance = 375, HoleHandicap = 2, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 8.3 },
                    new GolferHoleScore{ GolferId = 1, GolferName = "Caleb Burns", TeamId = 1, HoleId = 3, HoleNumber = 3, HoleDistance = 325, HoleHandicap = 3, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 8.3 },
                    new GolferHoleScore{ GolferId = 1, GolferName = "Caleb Burns", TeamId = 1, HoleId = 4, HoleNumber = 4, HoleDistance = 390, HoleHandicap = 4, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 8.3 },

                    new GolferHoleScore{ GolferId = 2, GolferName = "Ethan Thomas", TeamId = 2, HoleId = 1, HoleNumber = 1, HoleDistance = 350, HoleHandicap = 1, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 9.3 },
                    new GolferHoleScore{ GolferId = 2, GolferName = "Ethan Thomas", TeamId = 2, HoleId = 2, HoleNumber = 2, HoleDistance = 375, HoleHandicap = 2, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 9.3 },
                    new GolferHoleScore{ GolferId = 2, GolferName = "Ethan Thomas", TeamId = 2, HoleId = 3, HoleNumber = 3, HoleDistance = 325, HoleHandicap = 3, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 9.3 },
                    new GolferHoleScore{ GolferId = 2, GolferName = "Ethan Thomas", TeamId = 2, HoleId = 4, HoleNumber = 4, HoleDistance = 390, HoleHandicap = 4, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 9.3 },

                    new GolferHoleScore{ GolferId = 3, GolferName = "AJ Burns", TeamId = 3, HoleId = 1, HoleNumber = 1, HoleDistance = 350, HoleHandicap = 1, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 14.1 },
                    new GolferHoleScore{ GolferId = 3, GolferName = "AJ Burns", TeamId = 3, HoleId = 2, HoleNumber = 2, HoleDistance = 375, HoleHandicap = 2, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 14.1 },
                    new GolferHoleScore{ GolferId = 3, GolferName = "AJ Burns", TeamId = 3, HoleId = 3, HoleNumber = 3, HoleDistance = 325, HoleHandicap = 3, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 14.1 },
                    new GolferHoleScore{ GolferId = 3, GolferName = "AJ Burns", TeamId = 3, HoleId = 4, HoleNumber = 4, HoleDistance = 390, HoleHandicap = 4, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 14.1 },
            };

        public List<GolferHoleScore> scoreDataIndividualPartialTie = new List<GolferHoleScore>
        {
                    new GolferHoleScore{ GolferId = 1, GolferName = "Caleb Burns", TeamId = 1, HoleId = 1, HoleNumber = 1, HoleDistance = 350, HoleHandicap = 1, HolePar = 4, GrossScore = 3, NetScore = 3, GolferHandicap = 8.3 },
                    new GolferHoleScore{ GolferId = 1, GolferName = "Caleb Burns", TeamId = 1, HoleId = 2, HoleNumber = 2, HoleDistance = 375, HoleHandicap = 2, HolePar = 4, GrossScore = 3, NetScore = 3, GolferHandicap = 8.3 },
                    new GolferHoleScore{ GolferId = 1, GolferName = "Caleb Burns", TeamId = 1, HoleId = 3, HoleNumber = 3, HoleDistance = 325, HoleHandicap = 3, HolePar = 4, GrossScore = 3, NetScore = 3, GolferHandicap = 8.3 },
                    new GolferHoleScore{ GolferId = 1, GolferName = "Caleb Burns", TeamId = 1, HoleId = 4, HoleNumber = 4, HoleDistance = 390, HoleHandicap = 4, HolePar = 4, GrossScore = 3, NetScore = 3, GolferHandicap = 8.3 },

                    new GolferHoleScore{ GolferId = 2, GolferName = "Ethan Thomas", TeamId = 2, HoleId = 1, HoleNumber = 1, HoleDistance = 350, HoleHandicap = 1, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 9.3 },
                    new GolferHoleScore{ GolferId = 2, GolferName = "Ethan Thomas", TeamId = 2, HoleId = 2, HoleNumber = 2, HoleDistance = 375, HoleHandicap = 2, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 9.3 },
                    new GolferHoleScore{ GolferId = 2, GolferName = "Ethan Thomas", TeamId = 2, HoleId = 3, HoleNumber = 3, HoleDistance = 325, HoleHandicap = 3, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 9.3 },
                    new GolferHoleScore{ GolferId = 2, GolferName = "Ethan Thomas", TeamId = 2, HoleId = 4, HoleNumber = 4, HoleDistance = 390, HoleHandicap = 4, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 9.3 },

                    new GolferHoleScore{ GolferId = 3, GolferName = "AJ Burns", TeamId = 3, HoleId = 1, HoleNumber = 1, HoleDistance = 350, HoleHandicap = 1, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 14.1 },
                    new GolferHoleScore{ GolferId = 3, GolferName = "AJ Burns", TeamId = 3, HoleId = 2, HoleNumber = 2, HoleDistance = 375, HoleHandicap = 2, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 14.1 },
                    new GolferHoleScore{ GolferId = 3, GolferName = "AJ Burns", TeamId = 3, HoleId = 3, HoleNumber = 3, HoleDistance = 325, HoleHandicap = 3, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 14.1 },
                    new GolferHoleScore{ GolferId = 3, GolferName = "AJ Burns", TeamId = 3, HoleId = 4, HoleNumber = 4, HoleDistance = 390, HoleHandicap = 4, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 14.1 },
        };

        public List<GolferHoleScore> scoreDataIndividualPartialWinner = new List<GolferHoleScore>
        {
                    new GolferHoleScore{ GolferId = 1, GolferName = "Caleb Burns", TeamId = 1, HoleId = 1, HoleNumber = 1, HoleDistance = 350, HoleHandicap = 1, HolePar = 4, GrossScore = 3, NetScore = 3, GolferHandicap = 8.3 },
                    new GolferHoleScore{ GolferId = 1, GolferName = "Caleb Burns", TeamId = 1, HoleId = 2, HoleNumber = 2, HoleDistance = 375, HoleHandicap = 2, HolePar = 4, GrossScore = 3, NetScore = 3, GolferHandicap = 8.3 },
                    new GolferHoleScore{ GolferId = 1, GolferName = "Caleb Burns", TeamId = 1, HoleId = 3, HoleNumber = 3, HoleDistance = 325, HoleHandicap = 3, HolePar = 4, GrossScore = 3, NetScore = 3, GolferHandicap = 8.3 },
                    new GolferHoleScore{ GolferId = 1, GolferName = "Caleb Burns", TeamId = 1, HoleId = 4, HoleNumber = 4, HoleDistance = 390, HoleHandicap = 4, HolePar = 4, GrossScore = 3, NetScore = 3, GolferHandicap = 8.3 },

                    new GolferHoleScore{ GolferId = 2, GolferName = "Ethan Thomas", TeamId = 2, HoleId = 1, HoleNumber = 1, HoleDistance = 350, HoleHandicap = 1, HolePar = 4, GrossScore = 3, NetScore = 3, GolferHandicap = 9.3 },
                    new GolferHoleScore{ GolferId = 2, GolferName = "Ethan Thomas", TeamId = 2, HoleId = 2, HoleNumber = 2, HoleDistance = 375, HoleHandicap = 2, HolePar = 4, GrossScore = 3, NetScore = 3, GolferHandicap = 9.3 },
                    new GolferHoleScore{ GolferId = 2, GolferName = "Ethan Thomas", TeamId = 2, HoleId = 3, HoleNumber = 3, HoleDistance = 325, HoleHandicap = 3, HolePar = 4, GrossScore = 3, NetScore = 3, GolferHandicap = 9.3 },
                    new GolferHoleScore{ GolferId = 2, GolferName = "Ethan Thomas", TeamId = 2, HoleId = 4, HoleNumber = 4, HoleDistance = 390, HoleHandicap = 4, HolePar = 4, GrossScore = 3, NetScore = 3, GolferHandicap = 9.3 },

                    new GolferHoleScore{ GolferId = 3, GolferName = "AJ Burns", TeamId = 3, HoleId = 1, HoleNumber = 1, HoleDistance = 350, HoleHandicap = 1, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 14.1 },
                    new GolferHoleScore{ GolferId = 3, GolferName = "AJ Burns", TeamId = 3, HoleId = 2, HoleNumber = 2, HoleDistance = 375, HoleHandicap = 2, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 14.1 },
                    new GolferHoleScore{ GolferId = 3, GolferName = "AJ Burns", TeamId = 3, HoleId = 3, HoleNumber = 3, HoleDistance = 325, HoleHandicap = 3, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 14.1 },
                    new GolferHoleScore{ GolferId = 3, GolferName = "AJ Burns", TeamId = 3, HoleId = 4, HoleNumber = 4, HoleDistance = 390, HoleHandicap = 4, HolePar = 4, GrossScore = 4, NetScore = 4, GolferHandicap = 14.1 },
        };

        public ScoringRulesTest()
        {
            _teamHandicapScorer = new TeamHandicapScorer();
            _strokePlayScorer = new StrokePlayScorer();
            _strokePlayHandicapScorer = new StrokePlayHandicapScorer();
            _matchPlayScorer = new MatchPlayScorer();
            _matchPlayHandicapScorer = new MatchPlayHandicapScorer();
        }

        #region TeamHandicap
        [Fact]
        public void CalculatePoints_TeamHandicap_Tie()
        {
            // Arrange

            // Act
            var result = _teamHandicapScorer.CalculatePoints(scoreDataTie);

            // Get results by team id
            var teamPoints = result
                .Where(r => r.isGolfer == false)
                .ToList();

            var golferPoints = result
                .Where(r => r.isGolfer == true)
                .ToList();

            // Assert
            foreach (var team in teamPoints)
            {
                Assert.Equal(1, team.points);
            }

            foreach (var golfer in golferPoints)
            {
                Assert.Equal(2, golfer.points);
            }
        }

        [Fact]
        public void CalculatePoints_TeamHandicap_OutrightWinner()
        {
            // Act
            var result = _teamHandicapScorer.CalculatePoints(scoreDataOutrightWinner);

            var team1Points = result
                .Where(r => r.id == 1)
                .Where(r => r.isGolfer == false)
                .Select(r => r.points)
                .ToList();

            var team2Points = result
                .Where(r => r.id == 2)
                .Where(r => r.isGolfer == false)
                .Select(r => r.points)
                .ToList();

            var team1GolferPoints = result
                .Where(r => r.id == 1 || r.id == 2)
                .Where(r => r.isGolfer == true)
                .Select(r => r.points)
                .ToList();

            var team2GolferPoints = result
                .Where(r => r.id == 3 || r.id == 4)
                .Where(r => r.isGolfer == true)
                .Select(r => r.points)
                .ToList();

            // Assert
            foreach (var team in team1Points)
            {
                Assert.Equal(2, team);
            }

            foreach (var team in team2Points)
            {
                Assert.Equal(0, team);
            }

            foreach (var golfer in team1GolferPoints)
            {
                Assert.Equal(4, golfer);
            }

            foreach (var golfer in team2GolferPoints)
            {
                Assert.Equal(0, golfer);
            }
        }

        [Fact]
        public void CalculatePoints_TeamHandicap_PartialWinner()
        {
            // Act
            var result = _teamHandicapScorer.CalculatePoints(scoreDataPartialWinner);

            var team1Points = result
                .Where(r => r.id == 1)
                .Where(r => r.isGolfer == false)
                .Select(r => r.points)
                .Sum();

            var team2Points = result
                .Where(r => r.id == 2)
                .Where(r => r.isGolfer == false)
                .Select(r => r.points)
                .Sum();

            var golfer1Points = result
                .Where(r => r.id == 1 && r.isGolfer == true)
                .Select(r => r.points)
                .First();

            var golfer2Points = result
                .Where(r => r.id == 2 && r.isGolfer == true)
                .Select(r => r.points)
                .First();

            var golfer3Points = result
                .Where(r => r.id == 3 && r.isGolfer == true)
                .Select(r => r.points)
                .First();

            var golfer4Points = result
                .Where(r => r.id == 4 && r.isGolfer == true)
                .Select(r => r.points)
                .First();

            // Assert
            Assert.Equal(1, golfer1Points);
            Assert.Equal(2, golfer2Points);
            Assert.Equal(1, golfer3Points);
            Assert.Equal(4, golfer4Points);
            Assert.Equal(0, team1Points);
            Assert.Equal(2, team2Points);
        }

        #endregion

        #region StrokePlay

        [Fact]
        public void CalculatePoints_StrokePlay_Tie()
        {
            // Arrange

            // Act
            var result = _strokePlayScorer.CalculatePoints(scoreDataIndividualTie);

            // Assert
            foreach (var points in result)
            {
                Assert.Equal(1, points.points);
            }
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void CalculatePoints_StrokePlay_OutrightWinner()
        {
            // Arrange

            // Act
            var result = _strokePlayScorer.CalculatePoints(scoreDataIndividualWinner);

            // Assert
            foreach (var points in result)
            {
                if (points.id == 1)
                {
                    Assert.Equal(3, points.points);
                }
                else if (points.id == 2)
                {
                    Assert.Equal(2, points.points);
                }
                else if (points.id == 3)
                {
                    Assert.Equal(1, points.points);
                }
            }
        }

        [Fact]
        public void CalculatePoints_StrokePlay_PartialWinner()
        {
            // Arrange

            // Act
            var result = _strokePlayScorer.CalculatePoints(scoreDataIndividualPartialTie);

            // Assert
            foreach (var points in result)
            {
                if (points.id == 1)
                {
                    Assert.Equal(2, points.points);
                }
                else if (points.id == 2)
                {
                    Assert.Equal(1, points.points);
                }
                else if (points.id == 3)
                {
                    Assert.Equal(1, points.points);
                }
            }
        }

        #endregion

        #region StrokePlayHandicap

        [Fact]
        public void CalculatePoints_StrokePlayHandicap_Tie()
        {
            // Arrange

            // Act
            var result = _strokePlayHandicapScorer.CalculatePoints(scoreDataIndividualTie);

            // Assert
            foreach (var points in result)
            {
                Assert.Equal(1, points.points);
            }
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void CalculatePoints_StrokePlayHandicap_OutrightWinner()
        {
            // Arrange

            // Act
            var result = _strokePlayHandicapScorer.CalculatePoints(scoreDataIndividualWinner);

            // Assert
            foreach (var points in result)
            {
                if (points.id == 1)
                {
                    Assert.Equal(3, points.points);
                }
                else if (points.id == 2)
                {
                    Assert.Equal(2, points.points);
                }
                else if (points.id == 3)
                {
                    Assert.Equal(1, points.points);
                }
            }
        }

        [Fact]
        public void CalculatePoints_StrokePlayHandicap_PartialWinner()
        {
            // Arrange

            // Act
            var result = _strokePlayHandicapScorer.CalculatePoints(scoreDataIndividualPartialTie);

            // Assert
            foreach (var points in result)
            {
                if (points.id == 1)
                {
                    Assert.Equal(2, points.points);
                }
                else if (points.id == 2)
                {
                    Assert.Equal(1, points.points);
                }
                else if (points.id == 3)
                {
                    Assert.Equal(1, points.points);
                }
            }
        }

        #endregion

        #region MatchPlay

        [Fact]
        public void CalculatePoints_MatchPlay_Tie()
        {
            // Arrange

            // Act
            var result = _matchPlayScorer.CalculatePoints(scoreDataIndividualTie);

            // Assert
            foreach (var points in result)
            {
                Assert.Equal(2, points.points);
            }
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void CalculatePoints_MatchPlay_OutrightWinner()
        {
            // Arrange

            // Act
            var result = _matchPlayScorer.CalculatePoints(scoreDataIndividualWinner);

            var golfer1Points = result
                .Where(p => p.id == 1 && p.isGolfer == true)
                .Select(p => p.points)
                .First();

            var golfer2Points = result
                .Where(p => p.id == 2 && p.isGolfer == true)
                .Select(p => p.points)
                .First();

            var golfer3Points = result
                .Where(p => p.id == 3 && p.isGolfer == true)
                .Select(p => p.points)
                .First();

            // Assert
            Assert.Equal(3, result.Count());

            Assert.Equal(4, golfer1Points);
            Assert.Equal(0, golfer2Points);
            Assert.Equal(0, golfer3Points);
        }

        [Fact]
        public void CalculatePoints_MatchPlay_PartialWinner()
        {
            // Arrange

            // Act
            var result = _matchPlayScorer.CalculatePoints(scoreDataIndividualPartialWinner);

            var golfer1Points = result
                .Where(p => p.id == 1 && p.isGolfer == true)
                .Select(p => p.points)
                .First();

            var golfer2Points = result
                .Where(p => p.id == 2 && p.isGolfer == true)
                .Select(p => p.points)
                .First();

            var golfer3Points = result
                .Where(p => p.id == 3 && p.isGolfer == true)
                .Select(p => p.points)
                .First();

            // Assert
            Assert.Equal(3, result.Count());

            Assert.Equal(2, golfer1Points);
            Assert.Equal(2, golfer2Points);
            Assert.Equal(0, golfer3Points);
        }

        #endregion

        #region MatchPlayHandicap

        [Fact]
        public void CalculatePoints_MatchPlayHandicap_Tie()
        {
            // Arrange

            // Act
            var result = _matchPlayHandicapScorer.CalculatePoints(scoreDataIndividualTie);

            // Assert
            foreach (var points in result)
            {
                Assert.Equal(2, points.points);
            }
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void CalculatePoints_MatchPlayHandicap_OutrightWinner()
        {
            // Arrange

            // Act
            var result = _matchPlayHandicapScorer.CalculatePoints(scoreDataIndividualWinner);

            var golfer1Points = result
                .Where(p => p.id == 1 && p.isGolfer == true)
                .Select(p => p.points)
                .First();

            var golfer2Points = result
                .Where(p => p.id == 2 && p.isGolfer == true)
                .Select(p => p.points)
                .First();

            var golfer3Points = result
                .Where(p => p.id == 3 && p.isGolfer == true)
                .Select(p => p.points)
                .First();

            // Assert
            Assert.Equal(3, result.Count());

            Assert.Equal(4, golfer1Points);
            Assert.Equal(0, golfer2Points);
            Assert.Equal(0, golfer3Points);
        }

        [Fact]
        public void CalculatePoints_MatchPlayHandicap_PartialWinner()
        {
            // Arrange

            // Act
            var result = _matchPlayHandicapScorer.CalculatePoints(scoreDataIndividualPartialWinner);

            var golfer1Points = result
                .Where(p => p.id == 1 && p.isGolfer == true)
                .Select(p => p.points)
                .First();

            var golfer2Points = result
                .Where(p => p.id == 2 && p.isGolfer == true)
                .Select(p => p.points)
                .First();

            var golfer3Points = result
                .Where(p => p.id == 3 && p.isGolfer == true)
                .Select(p => p.points)
                .First();

            // Assert
            Assert.Equal(3, result.Count());

            Assert.Equal(2, golfer1Points);
            Assert.Equal(2, golfer2Points);
            Assert.Equal(0, golfer3Points);
        }

        #endregion
    }
}
