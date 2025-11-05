using Core.DTOs.TeamDTOs;
using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Models;
using Core.Services;
using Moq;

namespace Test_Golf_League_Scorebook.Services
{
    public class TeamServiceTest
    {
        private readonly Mock<ITeamRepository> _teamRepo;
        private readonly Mock<ILeagueRepository> _leagueRepo;
        private readonly Mock<IGolferTeamJunctionRepository> _golferTeamJunctionRepo;
        private readonly Mock<IGolferLeagueJunctionRepository> _golferLeagueJunctionRepo;

        private readonly ITeamService _teamService;

        public TeamServiceTest()
        {
            _teamRepo = new Mock<ITeamRepository>();
            _leagueRepo = new Mock<ILeagueRepository>();
            _golferTeamJunctionRepo = new Mock<IGolferTeamJunctionRepository>();
            _golferLeagueJunctionRepo = new Mock<IGolferLeagueJunctionRepository>();

            _teamService = new TeamService(_teamRepo.Object, _leagueRepo.Object, _golferTeamJunctionRepo.Object, _golferLeagueJunctionRepo.Object);
        }

        [Fact]
        public async Task CreateTeam_GoodValues_ReturnsSuccessful()
        {
            // Arrange

            var request = new CreateTeamRequest
            {
                Golfers = new List<Golfer> { new Golfer { GolferId = 1, FullName = "Caleb Burns" }, new Golfer {GolferId = 2, FullName = "AJ Burns" } },
                TeamName = "Caleb Burns & AJ Burns",
                LeagueId = 1,
            };

            _leagueRepo.Setup(r => r.GetById(request.LeagueId))
                .ReturnsAsync(new League { LeagueId = 1 });

            _teamRepo.Setup(r => r.GetTeamsByLeague(request.LeagueId))
                .ReturnsAsync( new List<Team>{ new Team { TeamName = "Test" } } );

            _golferLeagueJunctionRepo.Setup(r => r.GolferExistsInLeague(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var result =  await _teamService.CreateTeam(request);

            // Assert
            Assert.True(result.IsSuccess);
            _teamRepo.Verify(r => r.Add(It.IsAny<Team>()), Times.Once());
            _golferTeamJunctionRepo.Verify(r => r.Add(It.IsAny<GolferTeamJunction>()), Times.Exactly(2));
            _golferLeagueJunctionRepo.Verify(r => r.Add(It.IsAny<GolferLeagueJunction>()), Times.Exactly(2));
        }

        [Fact]
        public async Task CreateTeam_BadValues_ReturnsFailure()
        {
            // Arrange

            var request = new CreateTeamRequest
            {
                Golfers = new List<Golfer> { new Golfer { GolferId = 1, FullName = "Caleb Burns" }, new Golfer { GolferId = 2, FullName = "AJ Burns" } },
                TeamName = string.Empty,
                LeagueId = 1,
            };

            _leagueRepo.Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync((League)null);

            var expectedOutcome = new List<string>
            {
                "Team name is required.",
                "League is required"
            };

            // Act
            var result = await _teamService.CreateTeam(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedOutcome, result.ValidationErrors);
            _teamRepo.Verify(r => r.Add(It.IsAny<Team>()), Times.Never());
            _golferTeamJunctionRepo.Verify(r => r.Add(It.IsAny<GolferTeamJunction>()), Times.Never());
            _golferLeagueJunctionRepo.Verify(r => r.Add(It.IsAny<GolferLeagueJunction>()), Times.Never());
        }

        [Fact]
        public async Task UpdateTeam_GoodValues_ReturnsSuccessful()
        {
            // Arrange
            var team = new Team
            {
                TeamId = 1,
                TeamName = "No name",
                LeagueId = 1,
            };

            var request = new CreateTeamRequest
            {
                Golfers = new List<Golfer> { new Golfer { GolferId = 1, FullName = "Caleb Burns" }, new Golfer { GolferId = 2, FullName = "AJ Burns" } },
                TeamName = "Caleb Burns & AJ Burns",
                LeagueId = 2,
                TeamId = 1,
            };

            _leagueRepo.Setup(r => r.GetById(request.LeagueId))
                .ReturnsAsync(new League { LeagueId = 1 });

            _teamRepo.Setup(r => r.GetById((int)request.TeamId))
                .ReturnsAsync(team);

            // Act
            var result = await _teamService.UpdateTeam(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Team.LeagueId);
            Assert.Equal("Caleb Burns & AJ Burns", result.Team.TeamName);
            _teamRepo.Verify(r => r.Update(It.IsAny<Team>()), Times.Once());
            _golferTeamJunctionRepo.Verify(r => r.Add(It.IsAny<GolferTeamJunction>()), Times.Exactly(2));
            _golferLeagueJunctionRepo.Verify(r => r.Add(It.IsAny<GolferLeagueJunction>()), Times.Exactly(2));
        }

    }
}
