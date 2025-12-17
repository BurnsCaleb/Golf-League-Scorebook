using Core.DTOs.HoleDTOs;
using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Models;
using Core.Services;
using Moq;

namespace Test_Golf_League_Scorebook.Services
{
    public class HoleScoreServiceTest
    {
        private readonly Mock<IHoleScoreRepository> _holeScoreRepo;
        private readonly Mock<IGolferTeamJunctionRepository> _junctionRepo;
        private readonly IHoleScoreService _holeScoreService;
        private readonly Mock<ISubstituteRepository> _substituteRepo;

        public HoleScoreServiceTest()
        {
            _holeScoreRepo = new Mock<IHoleScoreRepository>();
            _junctionRepo = new Mock<IGolferTeamJunctionRepository>();
            _substituteRepo = new Mock<ISubstituteRepository>();
            _holeScoreService = new HoleScoreService(_holeScoreRepo.Object, _junctionRepo.Object, _substituteRepo.Object);
        }

        [Fact]
        public void PopulateScorePreview_GoodValues_ReturnSuccessful()
        {
            // Arrange
            var currentGolfer = new Golfer
            {
                GolferId = 1,
                FirstName = "Caleb",
                LastName = "Burns",
                Handicap = 8.3
            };

            var otherGolfer = new Golfer
            {
                GolferId = 2,
                FirstName = "AJ",
                LastName = "Burns",
                Handicap = 14.7
            };

            var golfers = new List<Golfer>
            {
                currentGolfer,
                otherGolfer
            };

            var currentHole = new Hole
            {
                HoleId = 1,
                HoleNum = 1,
                Par = 4,
                Distance = 350,
                Handicap = 1
            };

            var nextHole = new Hole
            {
                HoleId = 2,
                HoleNum = 2,
                Par = 5,
                Distance = 450,
                Handicap = 2
            };

            var expectedScoreData = new GolferHoleScore
            {
                GolferId = otherGolfer.GolferId,
                GolferName = otherGolfer.FullName,
                TeamId = 1,
                HoleId = currentHole.HoleId,
                HoleNumber = currentHole.HoleNum,
                HoleDistance = currentHole.Distance,
                HoleHandicap = currentHole.Handicap,
                HolePar = currentHole.Par,
                GrossScore = 4,
                NetScore = 4,
                GolferHandicap = otherGolfer.Handicap,
            };

            var scoreData = new List<GolferHoleScore>
            {
                new GolferHoleScore
                {
                    GolferId = currentGolfer.GolferId,
                    GolferName = currentGolfer.FullName,
                    TeamId = 1,
                    HoleId = currentHole.HoleId,
                    HoleNumber = currentHole.HoleNum,
                    HoleDistance = currentHole.Distance,
                    HoleHandicap = currentHole.Handicap,
                    HolePar = currentHole.Par,
                    GrossScore = 4,
                    NetScore = 4,
                    GolferHandicap = currentGolfer.Handicap,
                },

                new GolferHoleScore
                {
                    GolferId = currentGolfer.GolferId,
                    GolferName = currentGolfer.FullName,
                    TeamId = 1,
                    HoleId = nextHole.HoleId,
                    HoleNumber = nextHole.HoleNum,
                    HoleDistance = nextHole.Distance,
                    HoleHandicap = nextHole.Handicap,
                    HolePar = nextHole.Par,
                    GrossScore = 4,
                    NetScore = 4,
                    GolferHandicap = currentGolfer.Handicap,
                },

                expectedScoreData,

                new GolferHoleScore
                {
                    GolferId = otherGolfer.GolferId,
                    GolferName = otherGolfer.FullName,
                    TeamId = 1,
                    HoleId = nextHole.HoleId,
                    HoleNumber = nextHole.HoleNum,
                    HoleDistance = nextHole.Distance,
                    HoleHandicap = nextHole.Handicap,
                    HolePar = nextHole.Par,
                    GrossScore = 4,
                    NetScore = 4,
                    GolferHandicap = otherGolfer.Handicap,
                },

            };

            // Act
            var result = _holeScoreService.PopulateScorePreviews(golfers, scoreData, currentGolfer, currentHole);

            // Assert
            Assert.Equal(expectedScoreData, result.First());
        }
    }
}
