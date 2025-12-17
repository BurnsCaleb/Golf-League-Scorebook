using Core.DTOs.GolferSearchBarDTOs;
using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Models;
using Core.Services;
using Moq;

namespace Test_Golf_League_Scorebook.Services
{
    public class GolferSearchBarServiceTest
    {
        private readonly Mock<IGolferRepository> _mockGolferRepo;
        private readonly Mock<IGolferLeagueJunctionService> _mockJunctionService;
        private readonly GolferSearchBarService _searchService;

        public GolferSearchBarServiceTest()
        {
            _mockGolferRepo = new Mock<IGolferRepository>();
            _mockJunctionService = new Mock<IGolferLeagueJunctionService>();
            _searchService = new GolferSearchBarService(_mockGolferRepo.Object, _mockJunctionService.Object);
        }

        [Fact]
        public async Task PerformGolferCheck_GoodValues_ReturnsSuccessful()
        {
            // Arrange
            League league = new League
            {
                LeagueId = 1,
                LeagueName = "Test League",
                LeagueSettingsId = 1,
                CourseId = 1,
            };

            Golfer golfer = new Golfer
            {
                GolferId = 1,
                FirstName = "Caleb",
                LastName = "Burns",
                Handicap = 8.3,
                FullName = "Caleb Burns",
            };

            var request = new CreateGolferSearchBarRequest
            {
                GolferNames = new List<string> { "Caleb Burns" },
                League = league

            };

            _mockGolferRepo.Setup(r => r.GetByName(golfer.FullName))
                .ReturnsAsync(golfer);

            _mockJunctionService.Setup(r => r.GolferExistsInLeague(golfer.GolferId, league.LeagueId))
                .ReturnsAsync(false);

            // Act
            var result = await _searchService.PerformGolferCheck(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Golfers);
        }

        [Fact]
        public async Task PerformGolferCheck_NoGolfer_ReturnsFailure()
        {
            // Arrange
            League league = new League
            {
                LeagueId = 1,
                LeagueName = "Test League",
                LeagueSettingsId = 1,
                CourseId = 1,
            };

            Golfer golfer = new Golfer
            {
                GolferId = 1,
                FirstName = "Caleb",
                LastName = "Burns",
                Handicap = 8.3,
                FullName = "Caleb Burns",
            };

            var request = new CreateGolferSearchBarRequest
            {
                GolferNames = new List<string> { string.Empty },
                League = league

            };

            // Act
            var result = await _searchService.PerformGolferCheck(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Enter a golfer name.", result.ErrorMessage);
        }

        [Fact]
        public async Task PerformGolferCheck_GolferDoesntExist_ReturnsFailure()
        {
            // Arrange
            League league = new League
            {
                LeagueId = 1,
                LeagueName = "Test League",
                LeagueSettingsId = 1,
                CourseId = 1,
            };

            Golfer golfer = new Golfer
            {
                GolferId = 1,
                FirstName = "Caleb",
                LastName = "Burns",
                Handicap = 8.3,
                FullName = "Caleb Burns",
            };

            var request = new CreateGolferSearchBarRequest
            {
                GolferNames = new List<string> { "Caleb Burns" },
                League = league

            };

            _mockGolferRepo.Setup(r => r.GetByName(golfer.FullName))
                .ReturnsAsync((Golfer)null);

            // Act
            var result = await _searchService.PerformGolferCheck(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Caleb Burns is not a created golfer.", result.ErrorMessage);
        }

        [Fact]
        public async Task PerformGolferCheck_GolferInLeague_ReturnsFailure()
        {
            // Arrange
            League league = new League
            {
                LeagueId = 1,
                LeagueName = "Test League",
                LeagueSettingsId = 1,
                CourseId = 1,
            };

            Golfer golfer = new Golfer
            {
                GolferId = 1,
                FirstName = "Caleb",
                LastName = "Burns",
                Handicap = 8.3,
                FullName = "Caleb Burns",
            };

            var request = new CreateGolferSearchBarRequest
            {
                GolferNames = new List<string> { "Caleb Burns" },
                League = league

            };

            _mockGolferRepo.Setup(r => r.GetByName(golfer.FullName))
                .ReturnsAsync(golfer);

            _mockJunctionService.Setup(r => r.GolferExistsInLeague(golfer.GolferId, league.LeagueId))
                .ReturnsAsync(true);

            // Act
            var result = await _searchService.PerformGolferCheck(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Caleb Burns already plays in this league.", result.ErrorMessage);
        }
    }
}
