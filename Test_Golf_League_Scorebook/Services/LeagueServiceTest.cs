using Core.DTOs.LeagueDTOs;
using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Models;
using Core.Services;
using Moq;

namespace Test_Golf_League_Scorebook.Services
{
    public class LeagueServiceTest
    {
        private readonly Mock<ILeagueRepository> _mockLeagueRepo;
        private readonly Mock<ICourseRepository> _mockCourseRepo;
        private readonly Mock<ILeagueSettingRepository> _mockLeagueSettingRepo;
        private readonly Mock<IMatchupRepository> _mockMatchupRepo;
        private readonly Mock<ITeamMatchupJunctionRepository> _mockTeamMatchupRepo;
        private readonly Mock<IGolferTeamJunctionRepository> _mockGolferTeamRepo;
        private readonly Mock<IGolferMatchupJunctionRepository> _mockGolferMatchupRepo;
        private readonly ILeagueService _leagueService;

        public LeagueServiceTest()
        {
            _mockLeagueRepo = new Mock<ILeagueRepository>();
            _mockCourseRepo = new Mock<ICourseRepository>();
            _mockLeagueSettingRepo = new Mock<ILeagueSettingRepository>();
            _mockMatchupRepo = new Mock<IMatchupRepository>();
            _mockTeamMatchupRepo = new Mock<ITeamMatchupJunctionRepository>();
            _mockGolferTeamRepo = new Mock<IGolferTeamJunctionRepository>();
            _mockGolferMatchupRepo = new Mock<IGolferMatchupJunctionRepository>();
            _leagueService = new LeagueService(_mockLeagueRepo.Object, _mockLeagueSettingRepo.Object, _mockCourseRepo.Object, _mockMatchupRepo.Object, _mockTeamMatchupRepo.Object, _mockGolferTeamRepo.Object, _mockGolferMatchupRepo.Object);
        }

        #region CreateLeague Method Tests

        [Fact]
        public async Task CreateLeague_GoodValues_ReturnsSuccessful()
        {
            // Arrange
            var request = new CreateLeagueRequest
            {
                LeagueName = "Men's League",
                LeagueSettingsId = 1,
                CourseId = 1,
            };

            _mockCourseRepo.Setup(r => r.GetById(request.CourseId))
                .ReturnsAsync(new Course { CourseId = 1 });

            _mockLeagueSettingRepo.Setup(r => r.GetById(request.LeagueSettingsId))
                .ReturnsAsync(new LeagueSetting { LeagueSettingsId = 1 });

            // Act
            var result = await _leagueService.CreateLeague(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Men's League", result.League.LeagueName);
            _mockLeagueRepo.Verify(r => r.Add(It.IsAny<League>()), Times.Once);
        }

        [Fact]
        public async Task CreateLeague_BadValues_ReturnsFailure()
        {
            // Arrange
            var request = new CreateLeagueRequest
            {
                LeagueName = string.Empty,
                LeagueSettingsId = 1,
                CourseId = 1,
            };

            _mockCourseRepo.Setup(r => r.GetById(request.CourseId))
                .ReturnsAsync((Course)null);

            _mockLeagueSettingRepo.Setup(r => r.GetById(request.LeagueSettingsId))
                .ReturnsAsync((LeagueSetting)null);

            var expectedOutcome = new List<string>
            {
                "League name is required.",
                "Course is required",
                "League settings is required"
            };

            // Act
            var result = await _leagueService.CreateLeague(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedOutcome, result.ValidationErrors);
            _mockLeagueRepo.Verify(r => r.Add(It.IsAny<League>()), Times.Never);
        }

        [Fact]
        public async Task CreateLeague_DuplicateLeague_ReturnsFailure()
        {
            // Arrange
            var existentLeague = new League
            {
                LeagueId = 1,
                LeagueName = "Men's League",
                CourseId = 1,
                LeagueSettingsId = 1,
            };

            var request = new CreateLeagueRequest
            {
                LeagueName = "Men's League",
                LeagueSettingsId = 1,
                CourseId = 1,
            };

            _mockLeagueRepo.Setup(r => r.GetByName(request.LeagueName))
                .ReturnsAsync(existentLeague);

            _mockCourseRepo.Setup(r => r.GetById(request.CourseId))
                .ReturnsAsync(new Course { CourseId = 1 });

            _mockLeagueSettingRepo.Setup(r => r.GetById(request.LeagueSettingsId))
                .ReturnsAsync(new LeagueSetting { LeagueSettingsId = 1 });

            // Act
            var result = await _leagueService.CreateLeague(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("A league by the name Men's League already exists.", result.ErrorMessage);
            _mockLeagueRepo.Verify(r => r.Add(It.IsAny<League>()), Times.Never);
        }

        #endregion

        #region CreateLeagueSchedule Method Tests

        [Fact]
        public async Task CreateLeagueSchedule_GoodValues_ReturnsSuccessful()
        {
            // Arrange
            int leagueId = 1;
            int year = 2025;
            var teams = new List<Team>
            {
                new Team { TeamId = 1, TeamName = "Team A", LeagueId = leagueId },
                new Team { TeamId = 2, TeamName = "Team B", LeagueId = leagueId },
                new Team { TeamId = 3, TeamName = "Team C", LeagueId = leagueId },
                new Team { TeamId = 4, TeamName = "Team D", LeagueId = leagueId }
            };
            
            var teamIds = new List<int> { 1, 2, 3, 4 };

            var golfers = new List<Golfer>
            {
                new Golfer { GolferId = 1, FullName = "Caleb Burns" },
                new Golfer { GolferId = 2, FullName = "AJ Burns" },
                new Golfer { GolferId = 3, FullName = "Ethan Thomas" },
                new Golfer { GolferId = 4, FullName = "Cole Thomas" },
                new Golfer { GolferId = 5, FullName = "Garrett Trapp" },
                new Golfer { GolferId = 6, FullName = "Gavin Trapp" },
                new Golfer { GolferId = 7, FullName = "Colton Johnson" },
                new Golfer { GolferId = 8, FullName = "Landon Johnson" },
            };



            _mockGolferTeamRepo.Setup(r => r.GetAllGolfersByTeam(It.IsAny<List<int>>()))
                .Returns((List<int> teamIds) => Task.FromResult(golfers
                .Where(g => teamIds.Any(id => g.GolferId == id * 2 || g.GolferId == id * 2 - 1))
                .ToList()));


            // Act
            var result = await _leagueService.CreateLeagueSchedule(teams, leagueId, year);

            // Assert
            Assert.True(result.IsSuccess);

            // 4 teams = 3 rounds, 2 matchups per round = 6 total matchups
            _mockMatchupRepo.Verify(x => x.Add(It.IsAny<Matchup>()), Times.Exactly(6));
            _mockTeamMatchupRepo.Verify(x => x.Add(It.IsAny<TeamMatchupJunction>()), Times.Exactly(12)); // 2 teams per matchup
            _mockGolferMatchupRepo.Verify(x => x.Add(It.IsAny<GolferMatchupJunction>()), Times.Exactly(24)); // 2 golfers per team
        }

        [Fact]
        public async Task CreateLeagueSchedule_OddTeams_ReturnsSuccessful()
        {
            // Arrange
            int leagueId = 1;
            int year = 2025;
            var teams = new List<Team>
            {
                new Team { TeamId = 1, TeamName = "Team A", LeagueId = leagueId },
                new Team { TeamId = 2, TeamName = "Team B", LeagueId = leagueId },
                new Team { TeamId = 3, TeamName = "Team C", LeagueId = leagueId }
            };

            var teamIds = new List<int> { 1, 2, 3, 4 };

            var golfers = new List<Golfer>
            {
                new Golfer { GolferId = 1, FullName = "Caleb Burns" },
                new Golfer { GolferId = 2, FullName = "AJ Burns" },
                new Golfer { GolferId = 3, FullName = "Ethan Thomas" },
                new Golfer { GolferId = 4, FullName = "Cole Thomas" },
                new Golfer { GolferId = 5, FullName = "Garrett Trapp" },
                new Golfer { GolferId = 6, FullName = "Gavin Trapp" },
            };



            _mockGolferTeamRepo.Setup(r => r.GetAllGolfersByTeam(It.IsAny<List<int>>()))
                .Returns((List<int> teamIds) => Task.FromResult(golfers
                .Where(g => teamIds.Any(id => g.GolferId == id * 2 || g.GolferId == id * 2 - 1))
                .ToList()));


            // Act
            var result = await _leagueService.CreateLeagueSchedule(teams, leagueId, year);

            // Assert
            Assert.True(result.IsSuccess);

            // 4 teams = 3 rounds, 2 matchups per round = 6 total matchups
            _mockMatchupRepo.Verify(x => x.Add(It.IsAny<Matchup>()), Times.Exactly(3));
            _mockTeamMatchupRepo.Verify(x => x.Add(It.IsAny<TeamMatchupJunction>()), Times.Exactly(6)); // 2 teams per matchup
            _mockGolferMatchupRepo.Verify(x => x.Add(It.IsAny<GolferMatchupJunction>()), Times.Exactly(12)); // 2 golfers per team
        }
        #endregion
    }
}
