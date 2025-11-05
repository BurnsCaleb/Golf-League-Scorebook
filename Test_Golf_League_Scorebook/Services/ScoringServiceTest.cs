using Core.Interfaces.Service;
using Core.Models;
using Core.Services;

namespace Test_Golf_League_Scorebook.Services
{
    public class ScoringServiceTest
    {
        private readonly IScoringService _scoringService;


        public ScoringServiceTest()
        {
            _scoringService = new ScoringService();
        }

        [Fact]
        public void CalculateNetScore_GoodValues_ReturnsSuccessful()
        {
            // Arrange
            var hole = new Hole { HoleNum = 1 };

            int grossScore = 6;

            List<int> strokesByHole = new List<int> { 1 };

            // Act
            var result = _scoringService.CalculateNetScore(strokesByHole, grossScore, hole);

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public void DistributeStrokes_GoodValues_ReturnsSuccessful()
        {
            // Arrange
            var holes = new List<Hole>
            {
                new Hole { Handicap = 1, Par = 3, },
                new Hole { Handicap = 2, Par = 4, },
                new Hole { Handicap = 3, Par = 5, },
            };

            int totalStrokes = 5;

            // Act
            var result = _scoringService.DistributeStrokes(holes, totalStrokes);

            // Assert
            Assert.True(result.Count == 3);
            Assert.True(result[0] == 2);
            Assert.True(result[1] == 2);
            Assert.True(result[2] == 1);
        }
    }
}
