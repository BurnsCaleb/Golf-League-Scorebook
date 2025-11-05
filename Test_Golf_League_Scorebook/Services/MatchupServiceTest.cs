using Core.DTOs.HoleDTOs;
using Core.DTOs.MatchupDTOs;
using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Models;
using Core.Services;
using Core.Services.Rules;
using Moq;

namespace Test_Golf_League_Scorebook.Services
{
    public class MatchupServiceTest
    {
        private readonly Mock<IMatchupRepository> _mockMatchupRepo;
        private readonly Mock<IScoringRuleService> _mockRuleService;
        private readonly Mock<IRoundService> _mockRoundService;
        private readonly Mock<ILeagueSettingService> _mockLeagueSettingService;
        private readonly Mock<IHoleScoreService> _mockHoleScoreService;
        private readonly Mock<ITeamMatchupJunctionService> _mockTeamMatchupJunctionService;
        private readonly Mock<IGolferMatchupJunctionService> _mockGolferMatchupJunctionService;
        private readonly Mock<IGolferService> _mockGolferService;
        private readonly IMatchupService _matchupService;

        // Good Matchup object


        public MatchupServiceTest()
        {
            _mockMatchupRepo = new Mock<IMatchupRepository>();
            _mockRuleService = new Mock<IScoringRuleService>();
            _mockRoundService = new Mock<IRoundService>();
            _mockLeagueSettingService = new Mock<ILeagueSettingService>();
            _mockHoleScoreService = new Mock<IHoleScoreService>();
            _mockTeamMatchupJunctionService = new Mock<ITeamMatchupJunctionService>();
            _mockGolferMatchupJunctionService = new Mock<IGolferMatchupJunctionService>();
            _mockGolferService = new Mock<IGolferService>();

            _matchupService = new MatchupService(
                _mockMatchupRepo.Object,
                _mockRuleService.Object,
                _mockRoundService.Object,
                _mockLeagueSettingService.Object,
                _mockHoleScoreService.Object,
                _mockTeamMatchupJunctionService.Object,
                _mockGolferMatchupJunctionService.Object,
                _mockGolferService.Object);
        }

        [Fact]
        public void GrabMatchupScore_GoodValue_ReturnsSuccessful()
        {
            // Arrange
            var matchup = new Matchup
            {
                MatchupId = 1,
                Rounds = new List<Round>
                {   new Round
                    {
                        Golfer = new Golfer { GolferId = 1, FullName = "Caleb Burns", Handicap = 8.3 },
                        TeamId = 1,
                        MatchupId = 1,
                        HoleScores = new List<HoleScore>
                        {
                            new HoleScore
                            {
                                HoleId = 1,
                                Hole = new Hole
                                {
                                    HoleId = 1,
                                    HoleNum = 1,
                                    Par = 4,
                                    Distance = 350,
                                    Handicap = 1,
                                },
                                GrossScore = 4,
                                NetScore = 4,
                            }
                        }
                    },
                    new Round
                    {
                        Golfer = new Golfer { GolferId = 2, FullName = "AJ Burns", Handicap = 14.7 },
                        TeamId = 2,
                        MatchupId = 1,
                        HoleScores = new List<HoleScore>
                        {
                            new HoleScore
                            {
                                HoleId = 1,
                                Hole = new Hole
                                {
                                    HoleId = 1,
                                    HoleNum = 1,
                                    Par = 4,
                                    Distance = 350,
                                    Handicap = 1,
                                },
                                GrossScore = 6,
                                NetScore = 5,
                            }
                        }
                    }
                }
            };

            // Act
            var result = _matchupService.GrabMatchupScores(matchup);


            // Assert
            Assert.True(result.Any());
            Assert.Equal(result.Count, 2);
            Assert.Equal("Caleb Burns", result[0].GolferName);
            Assert.Equal("AJ Burns", result[1].GolferName);
        }

        [Fact]
        public async Task GrabMatchupScores_GoodValue_ReturnsSuccessful()
        {
            // Arrange
            var matchup = new Matchup
            {
                MatchupId = 1,
                Rounds = new List<Round>
                {   new Round
                    {
                        Golfer = new Golfer { GolferId = 1, FullName = "Caleb Burns", Handicap = 8.3 },
                        TeamId = 1,
                        MatchupId = 1,
                        HoleScores = new List<HoleScore> { new HoleScore { HoleId = 1, Hole = new Hole { HoleId = 1, HoleNum = 1, Par = 4, Distance = 350, Handicap = 1, }, GrossScore = 4, NetScore = 4,}}
                    },
                    new Round
                    {
                        Golfer = new Golfer { GolferId = 2, FullName = "AJ Burns", Handicap = 14.7 },
                        TeamId = 2,
                        MatchupId = 1,
                        HoleScores = new List<HoleScore> { new HoleScore { HoleId = 1, Hole = new Hole { HoleId = 1, HoleNum = 1, Par = 4, Distance = 350, Handicap = 1, }, GrossScore = 6, NetScore = 5, } }
                    }
                },
                League = new League
                {
                    LeagueSettings = new LeagueSetting { ScoringRule = new ScoringRule { RuleName = "Stroke Play" } }
                }
            };

            var scoreData = new List<GolferHoleScore>
            {
                new GolferHoleScore
                {
                    GolferId = 1,
                    GolferName = "Caleb Burns",
                    TeamId = 1,
                    HoleId = 1,
                    HoleNumber = 1,
                    HolePar = 4,
                    HoleDistance = 350,
                    HoleHandicap = 1,
                    GrossScore = 4,
                    NetScore = 4,
                    GolferHandicap = 8.3
                },
                new GolferHoleScore
                {
                    GolferId = 2,
                    GolferName = "AJ Burns",
                    TeamId = 2,
                    HoleId = 1,
                    HoleNumber = 1,
                    HolePar = 4,
                    HoleDistance = 350,
                    HoleHandicap = 1,
                    GrossScore = 6,
                    NetScore = 5,
                    GolferHandicap = 14.7
                },
            };

            var request = new EndMatchupRequest
            {
                Matchup = matchup,
                ScoreData = scoreData,
                Golfers = matchup.Rounds.Select(r => r.Golfer).ToList(),
                Rounds = matchup.Rounds.ToList()
            };

            _mockRuleService.Setup(r => r.CreateScorer(It.IsAny<ScoringRule>()))
                .Returns(new StrokePlayScorer());

            _mockTeamMatchupJunctionService.Setup(r => r.GetByMatchupTeam(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new TeamMatchupJunction { PointsAwarded = 0 });

            _mockGolferMatchupJunctionService.Setup(r => r.GetByGolferMatchup(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new GolferMatchupJunction { PointsAwarded = 0 });
            
            // Act
            var result = await _matchupService.EndMatchup(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Matchup.HasPlayed == true);
            _mockMatchupRepo.Verify(r => r.Update(It.IsAny<Matchup>()), Times.Once());
        }
    }
}
