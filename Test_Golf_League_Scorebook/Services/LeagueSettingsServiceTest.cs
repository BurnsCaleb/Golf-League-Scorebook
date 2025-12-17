using Core.DTOs.LeagueSettingsDTOs;
using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Models;
using Core.Services;
using Moq;

namespace Test_Golf_League_Scorebook.Services
{
    public class LeagueSettingsServiceTest
    {
        private readonly Mock<ILeagueSettingRepository> _mockSettingRepo;
        private readonly Mock<IScoringRuleRepository> _mockRuleRepo;
        private readonly ILeagueSettingService _leagueSettingService;

        public LeagueSettingsServiceTest()
        {
            _mockSettingRepo = new Mock<ILeagueSettingRepository>();
            _mockRuleRepo = new Mock<IScoringRuleRepository>();
            _leagueSettingService = new LeagueSettingService(_mockSettingRepo.Object, _mockRuleRepo.Object);
        }

        [Fact]
        public async Task CreateLeagueSetting_GoodValues_ReturnsSuccessful()
        {
            // Arrange
            // Create request
            var request = new CreateLeagueSettingRequest
            {
                LeagueSettingsName = "Setting Name",
                PlayDate = "Thursday",
                ScoringRuleId = 1,
                TeamSize = 2
            };

            _mockRuleRepo.Setup(r => r.GetById(1))
                .ReturnsAsync(new ScoringRule { ScoringRuleId = 1 });

            // Act
            var result = await _leagueSettingService.CreateLeagueSetting(request);

            // Assert
            Assert.True(result.IsSuccess);
            _mockSettingRepo.Verify(r => r.Add(It.IsAny<LeagueSetting>()), Times.Once);

        }

        [Fact]
        public async Task CreateLeagueSetting_BadValues_ReturnsFailure()
        {
            // Arrange
            // Create request
            var request = new CreateLeagueSettingRequest
            {
                LeagueSettingsName = string.Empty,
                PlayDate = "Thursday",
                ScoringRuleId = 0,
                TeamSize = 0
            };

            _mockRuleRepo.Setup(r => r.GetById(0))
                .ReturnsAsync((ScoringRule)null);

            var expectedOutcome = new List<string>
            {
                "League Settings name is required.",
                "Scoring rule is required.",
                "Team size must be greater than 0."
            };

            // Act
            var result = await _leagueSettingService.CreateLeagueSetting(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedOutcome, result.ValidationErrors);
            _mockSettingRepo.Verify(r => r.Add(It.IsAny<LeagueSetting>()), Times.Never);
        }

        [Fact]
        public async Task UpdateLeagueSetting_GoodValues_ReturnsSuccessful()
        {
            // Arrange

            // Existing Setting
            var existingSetting = new LeagueSetting
            {
                LeagueSettingsId = 1,
                LeagueSettingsName = "My League",
                PlayDate = "Wednesday",
                ScoringRuleId = 1,
                TeamSize = 1,
            };

            // Create request
            var request = new CreateLeagueSettingRequest
            {
                LeagueSettingsId = 1,
                LeagueSettingsName = "Setting Name",
                PlayDate = "Thursday",
                ScoringRuleId = 2,
                TeamSize = 2
            };

            _mockSettingRepo.Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync(existingSetting);

            _mockRuleRepo.Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync(new ScoringRule { ScoringRuleId = 1 });

            // Act
            var result = await _leagueSettingService.UpdateLeagueSetting(request);

            // Assert
            Assert.True(result.IsSuccess);

            Assert.Equal(request.LeagueSettingsName, result.LeagueSetting.LeagueSettingsName);
            Assert.Equal(request.PlayDate, result.LeagueSetting.PlayDate);
            Assert.Equal(request.ScoringRuleId, result.LeagueSetting.ScoringRuleId);
            Assert.Equal(request.TeamSize, result.LeagueSetting.TeamSize);

            _mockSettingRepo.Verify(r => r.Update(It.IsAny<LeagueSetting>()), Times.Once);
        }
    }
}
